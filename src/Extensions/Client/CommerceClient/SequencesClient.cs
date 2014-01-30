using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;

namespace VirtoCommerce.Client
{
    public class SequencesClient
    {
        #region Private fields

		//********These settings could be saved in database:

        //Prefix_Date_Seq
		public const string IdTemplate = "{0}{1}-{2}";
        //How many sequence items will be stored in-memory
		public const int SequenceReservationRange = 100;
        //Constant length of counter. Trailing zeros are added to left.
        public const int CounterLength = 5;

	    public const string DateFormat = "yyyy-MMdd";

		//***********************************************

        private readonly IAppConfigRepository _repository;
        private static readonly object SequenceLock = new object();
		private static readonly InMemorySequenceList InMemorySequences = new InMemorySequenceList();

        #endregion


        public SequencesClient(IAppConfigRepository repository)
        {
            _repository = repository;
        }

		public string GenerateNext(string objectType)
        {
            lock (SequenceLock)
            {
				InMemorySequences[objectType] = InMemorySequences[objectType] ?? new InMemorySequence(objectType);

				if (InMemorySequences[objectType].IsEmpty || InMemorySequences[objectType].HasExpired)
                {
					var startCounter = 0;
					var endCounter = SequenceReservationRange;

					//Update Sequences in database
                    using (var transaction = new TransactionScope())
                    {
                        var sequence = _repository.Sequences.SingleOrDefault(
                            s => s.ObjectType.Equals(objectType, StringComparison.OrdinalIgnoreCase));

                        if (sequence != null)
                        {
	                        startCounter = sequence.Value;

							//Sequence in database has expired?
	                        if (sequence.LastModified.HasValue && sequence.LastModified.Value.Date < DateTime.UtcNow.Date)
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
                        }
                        else
                        {
                            sequence = new Sequence { ObjectType = objectType };
                            _repository.Add(sequence);
                        }

						//Commit changes
						sequence.Value = endCounter;
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
				get { return _lastGenerationDateTime.HasValue && _lastGenerationDateTime.Value.Date < DateTime.Now.Date; }
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
					generatedItems.Push(string.Format(IdTemplate, _prefix, DateTime.Now.ToString(DateFormat), strCount));
				}

				//This revereses the sequence
				_sequence = new Stack<string>(generatedItems);
				_lastGenerationDateTime = DateTime.Now;
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