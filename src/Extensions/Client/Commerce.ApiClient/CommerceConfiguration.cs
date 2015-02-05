using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient
{
    using VirtoCommerce.ApiClient.Utilities;

    public class CommerceConfiguration
    {
        private string _ConnectionString = String.Empty;
        public string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_ConnectionString))
                {
                    _ConnectionString = ConnectionHelper.GetConnectionString("VirtoCommerce");
                }

                if (!_ConnectionString.EndsWith("/")) _ConnectionString += "/";

                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        private bool _IsCacheEnabled = true;
        public bool IsCacheEnabled
        {
            get
            {
                return _IsCacheEnabled;
            }
            set
            {
                _IsCacheEnabled = false;
            }
        }
    }
}
