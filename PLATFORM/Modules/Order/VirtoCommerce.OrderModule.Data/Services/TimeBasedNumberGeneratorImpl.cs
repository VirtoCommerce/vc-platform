using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Services;

namespace VirtoCommerce.OrderModule.Data.Services
{
	public class TimeBasedNumberGeneratorImpl : IOperationNumberGenerator
	{
        #region Private fields

        //********These settings could be saved in database:

        //Prefix_Date_Seq
        public const string IdTemplate = "{0}{1}-{2}";
        //How many sequence items will be stored in-memory
        public const int SequenceReservationRange = 100;
        //Constant length of counter. Trailing zeros are added to left.
        public const int CounterLength = 5;

        public const string DateFormat = "yyMMdd";

        //***********************************************

        private static readonly object SequenceLock = new object();
        private static readonly InMemorySequenceList InMemorySequences = new InMemorySequenceList();

        #endregion

		#region IOperationNumberGenerator Members

		public string GenerateNumber(Domain.Order.Model.Operation operation)
		{
            //var now = DateTime.UtcNow;
            //var retVal = operation.GetType().Name.Substring(0, 2).ToUpper() + now.DayOfYear.ToString("000") + now.TimeOfDay.Minutes.ToString("00");
            //return retVal;
            var objectType = operation.GetType().Name.Substring(0, 2).ToUpper();
		    const int startNumber = 0;
		    const int increment = 1;

            lock (SequenceLock)
            {
                InMemorySequences[objectType] = InMemorySequences[objectType] ?? new InMemorySequence(objectType);

                if (InMemorySequences[objectType].IsEmpty || InMemorySequences[objectType].HasExpired)
                {
                    var startCounter = startNumber;
                    var endCounter = startCounter + SequenceReservationRange * increment;
                    
                    //Pregenerate
                    InMemorySequences[objectType].Pregenerate(startCounter, endCounter, increment);
                }

                return string.Format(InMemorySequences[objectType].Next());
            }
		}

		#endregion

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

            public void Pregenerate(int startCount, int endCount, int increment)
            {
                var generatedItems = new Stack<string>();
                for (var index = startCount; index < endCount; index += increment)
                {
                    var strCount = index.ToString(CultureInfo.InvariantCulture).PadLeft(CounterLength, '0');
                    generatedItems.Push(string.Format(IdTemplate, _prefix, DateTime.UtcNow.ToString(DateFormat), strCount));
                }

                //This reverses the sequence
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
