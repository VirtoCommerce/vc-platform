using System;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    [Serializable]
    public class GenderElement : DictionaryElement
    {
        private const string Male = "male",
                            Female = "female";

        public GenderElement()
        {
            AvailableValues = new [] { Male, Female };
            DefaultValue = Male;
        }

        public bool IsMale
        {
            get
            {
                return ((string)InputValue) == Male;
            }
        }

		public bool IsFemale
		{
			get
			{
				return ((string)InputValue) == Female;
			}
		}
    }
}
