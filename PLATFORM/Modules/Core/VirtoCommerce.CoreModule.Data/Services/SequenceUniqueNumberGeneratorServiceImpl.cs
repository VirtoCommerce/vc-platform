using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.CoreModule.Data.Model;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CoreModule.Data.Services
{
    public class SequenceUniqueNumberGeneratorServiceImpl : ServiceBase, IUniqueNumberGenerator
    {
        #region Private fields

        //********These settings could be saved in database:

        //Prefix_Date_Seq
        string _numberTemplate;
        //How many sequence items will be stored in-memory
        int _sequenceReservationRange;
        // the length of counter. Trailing zeros are added to left.
        int _counterLength;

        string _dateFormat;

        //***********************************************

        private readonly Func<IСommerceRepository> _repositoryFactory;
        private static readonly object SequenceLock = new object();
        private static readonly InMemorySequenceList InMemorySequences = new InMemorySequenceList();

        #endregion


        public SequenceUniqueNumberGeneratorServiceImpl(Func<IСommerceRepository> repositoryFactory)
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

        public string GenerateNumber(string objectTypeName)
        {
            return GenerateNumber(objectTypeName, "{0}{1}-{2}");
        }
        public string GenerateNumber(
          string objectTypeName,
          string numberTemplate,
          string dateFormat = "yyMMdd",
          int sequenceReservationRange = 100,
          int counterLength = 5)
        {
            _numberTemplate = numberTemplate;
            _dateFormat = dateFormat;
            _sequenceReservationRange = sequenceReservationRange;
            _counterLength = counterLength;

            // take upercase chars to form operation type. Or just take 2 first chars
            var objectType = string.Concat(objectTypeName.Select(c => char.IsUpper(c) ? c.ToString() : ""));
            if (objectType.Length < 2)
            {
                objectType = objectTypeName.Substring(0, 2).ToUpper();
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
                        InMemorySequences[objectType].Pregenerate(startCounter, endCounter, _numberTemplate, _dateFormat, _counterLength);
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
                    endCounter = checked(startCounter + _sequenceReservationRange);
                }
                catch (OverflowException)
                {
                    //need to reset
                    startCounter = 0;
                    endCounter = _sequenceReservationRange;
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

            public void Pregenerate(int startCount, int endCount, string numberTemplate, string dateFormat, int counterLength)
            {
                _lastGenerationDateTime = DateTime.UtcNow;
                var generatedItems = new Stack<string>();
                for (var index = startCount; index < endCount; index++)
                {
                    var strCount = index.ToString(CultureInfo.InvariantCulture).PadLeft(counterLength, '0');
                    generatedItems.Push(string.Format(numberTemplate, _prefix, _lastGenerationDateTime.Value.ToString(dateFormat), strCount));
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
