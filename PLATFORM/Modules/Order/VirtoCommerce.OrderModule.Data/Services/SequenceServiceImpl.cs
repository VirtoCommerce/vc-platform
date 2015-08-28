using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Data.Model;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Services
{
    public class SequenceServiceImpl : ServiceBase, IOperationNumberGenerator
    {
        #region Private fields

        //********These settings could be saved in database:

        //Prefix_Date_Seq
        public static string IdTemplate = "{0}{1}-{2}";
        //How many sequence items will be stored in-memory
        public const int SequenceReservationRange = 100;
        //Constant length of counter. Trailing zeros are added to left.
        public const int CounterLength = 5;

        public static string DateFormat = "yyMMdd";

        //***********************************************

        private readonly Func<IСommerceRepository> _repositoryFactory;
        private static readonly object SequenceLock = new object();
        private static readonly InMemorySequenceList InMemorySequences = new InMemorySequenceList();

        #endregion


        public SequenceServiceImpl(Func<IСommerceRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;

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

        public string GenerateNumber(IOperation operation)
        {
            // take upercase chars to form operation type. Or just take 2 first chars
            var s = operation.GetType().Name;
            var objectType = string.Concat(s.Select(c => char.IsUpper(c) ? c.ToString() : ""));
            if (objectType.Length < 2)
            {
                objectType = s.Substring(0, 2).ToUpper();
            }

            lock (SequenceLock)
            {
                InMemorySequences[objectType] = InMemorySequences[objectType] ?? new InMemorySequence(objectType);

                if (InMemorySequences[objectType].IsEmpty || InMemorySequences[objectType].HasExpired)
                {
                    var startCounter = 0;
                    var endCounter = 0;

                    var initializedCounters = false;
                    var retryCount = 0;
                    const int maxTransactionRetries = 3;

                    while (!initializedCounters && retryCount < maxTransactionRetries)
                    {
                        try
                        {
                            InitCounters(objectType, out startCounter, out endCounter);
                            initializedCounters = true;
                        }
                        catch (System.Data.Entity.Infrastructure.DbUpdateException)
                        {
                            //This exception can happen due to deadlock so we can retry transaction several times
                            initializedCounters = false;
                            if (retryCount >= maxTransactionRetries)
                            {
                                throw;
                            }
                        }
                        finally
                        {
                            retryCount++;
                        }
                    }

                    if (initializedCounters)
                    {
                        //Pregenerate
                        InMemorySequences[objectType].Pregenerate(startCounter, endCounter);
                    }
                }

                return string.Format(InMemorySequences[objectType].Next());
            }
        }

        private void InitCounters(string objectType, out int startCounter, out int endCounter)
        {
            //Update Sequences in database
            using (var repository = _repositoryFactory())
            {
                var sequence = repository.Sequences.SingleOrDefault(s => s.ObjectType.Equals(objectType, StringComparison.OrdinalIgnoreCase));
                var originalModifiedDate = sequence != null ? sequence.ModifiedDate : null;

                if (sequence != null)
                {
                    sequence.ModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    sequence = new Sequence { ObjectType = objectType, Value = 0, ModifiedDate = DateTime.UtcNow };
                    repository.Add(sequence);
                }


                CommitChanges(repository);
                //Refresh data to make sure we have latest value in case another transaction was locked
                repository.Refresh(repository.Sequences);
                sequence = repository.Sequences.Single(s => s.ObjectType.Equals(objectType, StringComparison.OrdinalIgnoreCase));
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

                sequence.Value = endCounter;
                //sequence.LastModified = DateTime.UtcNow;
                CommitChanges(repository);
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
                _lastGenerationDateTime = DateTime.UtcNow;
                var generatedItems = new Stack<string>();
                for (var index = startCount; index < endCount; index++)
                {
                    var strCount = index.ToString(CultureInfo.InvariantCulture).PadLeft(CounterLength, '0');
                    generatedItems.Push(string.Format(IdTemplate, _prefix, _lastGenerationDateTime.Value.ToString(DateFormat), strCount));
                }

                //This revereses the sequence
                _sequence = new Stack<string>(generatedItems);
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
