using System;
using System.Linq;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	[Serializable]
	public class MatchingContains : DictionaryElement
	{
        public string Matching = "matching".Localize();
        public string Contains = "contains".Localize();
        public string NotMatching = "not matching".Localize();
        public string NotContains = "not contains".Localize();

        public string MatchingCase = "matching (case sensitive)".Localize();
        public string ContainsCase = "contains (case sensitive)".Localize();
        public string NotMatchingCase = "not matching (case sensitive)".Localize();
        public string NotContainsCase = "not contains (case sensitive)".Localize();

		public MatchingContains(bool useCase, bool useContains)
		{
			var tmp = new[] { Matching, MatchingCase, Contains, ContainsCase, NotMatching, NotMatchingCase, NotContains, NotContainsCase }.ToList();

			if (!useCase)
			{
				tmp.Remove(MatchingCase);
				tmp.Remove(ContainsCase);
				tmp.Remove(NotMatchingCase);
				tmp.Remove(NotContainsCase);
			}
			if (!useContains)
			{
				tmp.Remove(Contains);
				tmp.Remove(NotContains);
				tmp.Remove(ContainsCase);
				tmp.Remove(NotContainsCase);
			}

			AvailableValues = tmp.ToArray();
			DefaultValue = Matching;
		}

		public bool IsMatching
		{
			get
			{
				return ((string)InputValue) == Matching;
			}
			set
			{
				InputValue = value ? Matching : Contains;
			}
		}

		public bool IsContains
		{
			get
			{
				return ((string)InputValue) == Contains;
			}
			set
			{
				InputValue = value ? Contains : Matching;
			}
		}

		public bool IsNotMatching
		{
			get
			{
				return ((string)InputValue) == NotMatching;
			}
			set
			{
				InputValue = value ? NotMatching : NotContains;
			}
		}

		public bool IsNotContains
		{
			get
			{
				return ((string)InputValue) == NotContains;
			}
			set
			{
				InputValue = value ? NotContains : NotMatching;
			}
		}
	}
}
