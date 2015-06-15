#region
using System;
using System.Collections.Generic;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    public class Settings : Drop
    {
        #region Fields
        private readonly string _defaultValue;

        private readonly Dictionary<string, object> _settings;
        #endregion

        #region Constructors and Destructors
        public Settings(Dictionary<string, object> settings, string defaultValue = null)
        {
            this._settings = settings;
            this._defaultValue = defaultValue;
        }
        #endregion

        #region Public Methods and Operators
        public override object BeforeMethod(string method)
        {
            if (String.IsNullOrEmpty(method)) return null;
            return this._settings.ContainsKey(method) ? this._settings[method] : this._defaultValue;
        }
        #endregion
    }
}