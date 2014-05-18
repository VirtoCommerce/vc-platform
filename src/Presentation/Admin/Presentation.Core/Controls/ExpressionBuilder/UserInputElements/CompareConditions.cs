using System;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	[Serializable]
	public class CompareConditions : DictionaryElement
	{
		public string GreaterThan = "greater than".Localize();
        public string GreaterOrEqual = "greater than or equals".Localize();
        public string LessThan = "less than".Localize();
        public string LessOrEqual = "less than or equals".Localize();

        public string Matching = "matching ".Localize();
        public string NotMatching = "not matching ".Localize();

		public CompareConditions(bool useNotMatching)
		{
			AvailableValues = useNotMatching ? 
				new[] { GreaterThan, GreaterOrEqual, LessThan, LessOrEqual, Matching, NotMatching} : 
				new[] { GreaterThan, GreaterOrEqual, LessThan, LessOrEqual, Matching };
			DefaultValue = Matching;
		}

		public bool IsMatching
		{
			get { return Equals(InputValue, Matching); }
		}

		public bool IsNotMatching
		{
			get { return Equals(InputValue, NotMatching); }
		}

		public bool IsGreaterThan
		{
			get { return Equals(InputValue, GreaterThan); }
		}

		public bool IsGreaterThanOrEqual
		{
			get { return Equals(InputValue, GreaterOrEqual); }
		}

		public bool IsLessThan
		{
			get { return Equals(InputValue, LessThan); }
		}

		public bool IsLessThanOrEqual
		{
			get { return Equals(InputValue, LessOrEqual); }
		}
	}
}
