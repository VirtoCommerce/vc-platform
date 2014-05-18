using System;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	[Serializable]
	public class ExactlyLeast : DictionaryElement
	{
        public string Exactly = "Exactly".Localize();
        public string Least = "At least".Localize();

		public ExactlyLeast()
		{
			AvailableValues = new[] { Exactly, Least };
			DefaultValue = Least;
		}

		public bool IsExactly
		{
			get
			{
				return ((string)InputValue) == Exactly;
			}
			set
			{
				InputValue = value ? Exactly : Least;
			}
		}
	}
}
