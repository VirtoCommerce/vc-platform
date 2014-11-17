using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Client
{
    public class SequencesClient
    {
        #region Private fields

		//********These settings could be saved in database:

        //Prefix_Date_Seq
		public static string IdTemplate = "{0}{1}-{2}";
        //How many sequence items will be stored in-memory
		public const int SequenceReservationRange = 100;
        //Constant length of counter. Trailing zeros are added to left.
        public const int CounterLength = 5;

	    public static string DateFormat = "yyyy-MMdd";

		//***********************************************

        private readonly IAppConfigRepository _repository;
        private static readonly object SequenceLock = new object();
		private static readonly InMemorySequenceList InMemorySequences = new InMemorySequenceList();

        #endregion


        public SequencesClient(IAppConfigRepository repository)
        {
            _repository = repository;
            //TODO: changing format is not safe because it can break unique tracking number
            /*Uncomment following lines on your own risk. Dont forget these rules
             * 1. Traking number contains three parts {0}{1}{2}
             * {0}. Prefix is option
             * {1}. Date part is required and must contain all three Year/Month/Day
             * {2}  Counter is required
             */
            //var template = repository.Settings.Expand(x=>x.SettingValues).FirstOrDefault(x => x.Name == "TrackingNumberFormat");
            //if (template != null && template.SettingValues.Any())
            //{
            //    IdTemplate = template.SettingValues[0].ToString();
            //}

            //var dateFormat = repository.Settings.Expand(x=>x.SettingValues).FirstOrDefault(x => x.Name == "TrackingNumberFormatDateFormat");
            //if (dateFormat != null && dateFormat.SettingValues.Any())
            //{
            //    DateFormat = dateFormat.SettingValues[0].ToString();
            //}
        }

		public string GenerateNext(string objectType)
        {
            lock (SequenceLock)
            {
				InMemorySequences[objectType] = InMemorySequences[objectType] ?? new InMemorySequence(objectType);

				if (InMemorySequences[objectType].IsEmpty || InMemorySequences[objectType].HasExpired)
                {
					int startCounter;
					int endCounter;

					//Update Sequences in database
                    using (SqlDbConfiguration.ExecutionStrategySuspension)
                    using (var transaction = new TransactionScope())
                    {
                        var sequence = _repository.Sequences.SingleOrDefault(s => s.ObjectType.Equals(objectType, StringComparison.OrdinalIgnoreCase));

                        var originalModifiedDate = sequence !=null ? sequence.LastModified : null;

                        if (sequence != null)
                        {
                            //Make sequence as volatile data
                            sequence.LastModified = DateTime.UtcNow;
                            _repository.UnitOfWork.Commit();
                        }
                        else
                        {
                            sequence = new Sequence { ObjectType = objectType, Value = 0, LastModified = DateTime.UtcNow};
                            _repository.Add(sequence);
                            _repository.UnitOfWork.Commit();
                        }

                        //Refresh data to make sure we have latest value in case another transaction was locked
                        _repository.Refresh(_repository.Sequences);
                        sequence = _repository.Sequences.Single(s => s.ObjectType.Equals(objectType, StringComparison.OrdinalIgnoreCase));
                        startCounter = sequence.Value;

                        //Sequence in database has expired?
                        if (originalModifiedDate.HasValue && originalModifiedDate.Value.Date < DateTime.UtcNow.Date)
                        {
                            startCounter = 0;
                        }

                        try
                        {
                            endCounter = checked(startCounter + SequenceReservationRange);
                        }
                        catch (OverflowException)
                        {
                            //need to reset
                            startCounter = 0;
                            endCounter = SequenceReservationRange;
                        }

						//Commit changes
						sequence.Value = endCounter;
                        //sequence.LastModified = DateTime.UtcNow;
                        _repository.UnitOfWork.Commit();

						//Transaction success
                        transaction.Complete();
                    }

					//Pregenerate
					InMemorySequences[objectType].Pregenerate(startCounter, endCounter);
                }

                return string.Format(InMemorySequences[objectType].Next());
            }
        }

	    private class InMemorySequence
	    {
		    private readonly string _type;
			private Stack<string> _sequence = new Stack<string>();
		    private DateTime? _lastGenerationDateTime;
		    private readonly string _prefix;

		    public InMemorySequence(string type)
		    {
			    _type = type;
			    _sequence = new Stack<string>();

				_prefix = type.Substring(type.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) + 1);
				_prefix = String.IsNullOrEmpty(_prefix) ? "U" : _prefix.ToUpper();
		    }

		    public string ObjectType
		    {
				get { return _type; }
		    }

			public bool HasExpired
			{
                get { return _lastGenerationDateTime.HasValue && _lastGenerationDateTime.Value.Date < DateTime.UtcNow.Date; }
			}

		    public bool IsEmpty
		    {
			    get { return _sequence.Count == 0; }
		    }

			public string Next()
			{
				return _sequence.Pop();
			}

			public void Pregenerate(int startCount, int endCount)
			{
				var generatedItems = new Stack<string>();
				for (var index = startCount; index < endCount; index++)
				{
					var strCount = index.ToString(CultureInfo.InvariantCulture).PadLeft(CounterLength, '0');
					generatedItems.Push(string.Format(IdTemplate, _prefix, DateTime.UtcNow.ToString(DateFormat), strCount));
				}

				//This revereses the sequence
				_sequence = new Stack<string>(generatedItems);
				_lastGenerationDateTime = DateTime.UtcNow;
			}	
	    }

		private class InMemorySequenceList : List<InMemorySequence>
		{
			public InMemorySequence this[string type]
			{
				get
				{
					return this.FirstOrDefault(i => i.ObjectType.Equals(type, StringComparison.OrdinalIgnoreCase));
				}
				set
				{
					var exitingItem = this[type];

					if (exitingItem != null)
					{
						Remove(exitingItem);
					}
					Add(value);
				}
			}
		}
    }
}