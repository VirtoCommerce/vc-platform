using System;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	[Serializable]
	public class CompareConditions : DictionaryElement
	{
		public string GreaterThan = "greater than";
		public string GreaterOrEqual = "greater than or equals";
		public string LessThan = "less than";
		public string LessOrEqual = "less than or equals";

		public string Matching = "matching ";
		public string NotMatching = "not matching ";

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
