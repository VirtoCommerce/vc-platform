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

        //How many sequence items will be stored in-memory
        public const int SequenceReservationRange = 100;

        private readonly Func<IСommerceRepository> _repositoryFactory;
        private static readonly object _sequenceLock = new object();
        private static readonly InMemorySequenceList _inMemorySequences = new InMemorySequenceList();

        #endregion


        public SequenceUniqueNumberGeneratorServiceImpl(Func<IСommerceRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Generates unique number using given template, e.g., GenerateNumber("Order{0:yyMMdd}-{1:D5}");
        /// </summary>
        /// <param name="numberTemplate">The number template. Pass the format to be used in string.Format function. Passable parameters: 0 - date (the UTC time of number generation); 1 - the sequence number.</param>
        /// <returns></returns>
        public string GenerateNumber(string numberTemplate)
        {
            lock (_sequenceLock)
            {
                _inMemorySequences[numberTemplate] = _inMemorySequences[numberTemplate] ?? new InMemorySequence(numberTemplate);

                if (_inMemorySequences[numberTemplate].IsEmpty || _inMemorySequences[numberTemplate].HasExpired)
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
                            InitCounters(numberTemplate, out startCounter, out endCounter);
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
                        _inMemorySequences[numberTemplate].Pregenerate(startCounter, endCounter, numberTemplate);
                    }
                }

                return string.Format(_inMemorySequences[numberTemplate].Next());
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

            public InMemorySequence(string type)
            {
                _type = type;
                _sequence = new Stack<string>();
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

            public void Pregenerate(int startCount, int endCount, string numberTemplate)
            {
                _lastGenerationDateTime = DateTime.UtcNow;
                var generatedItems = new Stack<string>();
                for (var index = startCount; index < endCount; index++)
                {
                    generatedItems.Push(string.Format(numberTemplate, _lastGenerationDateTime.Value, index));
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
