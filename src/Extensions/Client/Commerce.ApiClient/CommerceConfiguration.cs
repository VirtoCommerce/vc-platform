#region

using System;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient
{

    #region

    #endregion

    public class CommerceConfiguration
    {
        #region Fields

        private string _ConnectionString = String.Empty;

        private bool _IsCacheEnabled = true;

        #endregion

        #region Public Properties

        public string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_ConnectionString))
                {
                    _ConnectionString = ConnectionHelper.GetConnectionString("VirtoCommerce");
                }

                if (!_ConnectionString.EndsWith("/"))
                {
                    _ConnectionString += "/";
                }

                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

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

        #endregion
    }
}
