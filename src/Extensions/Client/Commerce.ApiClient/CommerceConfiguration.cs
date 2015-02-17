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
                if (String.IsNullOrEmpty(this._ConnectionString))
                {
                    this._ConnectionString = ConnectionHelper.GetConnectionString("VirtoCommerce");
                }

                if (!this._ConnectionString.EndsWith("/"))
                {
                    this._ConnectionString += "/";
                }

                return this._ConnectionString;
            }
            set
            {
                this._ConnectionString = value;
            }
        }

        public bool IsCacheEnabled
        {
            get
            {
                return this._IsCacheEnabled;
            }
            set
            {
                this._IsCacheEnabled = false;
            }
        }
        #endregion
    }
}