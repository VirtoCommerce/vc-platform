using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represent helper type allow to construct user name with domain prefix like a domain/userName
    /// </summary>
    public class StorefrontUserName
    {
        private const string _delimiter = @"@@";

        public StorefrontUserName(string userName)
            :this(userName, null)
        {
        }

        public StorefrontUserName(string userName, string domain)
        {
            Domain = domain;
            UserName = userName;
        }

        public string Domain { get; private set; }
        public string UserName { get; private set; }

        public static StorefrontUserName TryParse(string inputString)
        {
            if(inputString == null)
            {
                throw new ArgumentNullException("inputString");
            }

            StorefrontUserName retVal = null;
            var parts = inputString.Split(new[] { _delimiter }, StringSplitOptions.RemoveEmptyEntries);
            if(parts.Count() > 1)
            {
                retVal = new StorefrontUserName(parts[1], parts[0]);
            }
            if(parts.Count() == 1)
            {
                retVal = new StorefrontUserName(parts[0]);
            }
            return retVal;
        }

        public override string ToString()
        {
            string retVal = UserName;
            //if(!string.IsNullOrEmpty(Domain))
            //{
            //    retVal = Domain + _delimiter;
            //}
            //if (!string.IsNullOrEmpty(UserName))
            //{
            //    retVal += UserName;
            //}
            return retVal;
        }
    }
}
