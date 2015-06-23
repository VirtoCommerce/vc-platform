using System;
using System.Collections.Generic;
using System.Linq;
using AvaTax.TaxModule.Web.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Platform.Core.Settings;

namespace AvaTax.TaxModule.Web.Managers
{
    
        public class AvaTaxImpl : ITax, IHaveSettings
        {
            private readonly string _username;
            private readonly string _password;
            private readonly string _serviceUrl;
            private readonly string _companyCode;
            private readonly string _isEnabled;

            private readonly string _code;
            private readonly string _description;
            private readonly string _logoUrl;

            public AvaTaxImpl(string username, string password, string serviceUrl, string companyCode, string isEnabled, string code, string description, string logoUrl, ICollection<SettingEntry> settings)
            {
                _username = username;
                _password = password;
                _code = code;
                _description = description;
                _logoUrl = logoUrl;
                _serviceUrl = serviceUrl;
                _companyCode = companyCode;
                _isEnabled = isEnabled;
                Settings = settings;
            }

            public string Username
            {

                get
                {
                    var retVal = GetSetting(_username);
                    return retVal;
                }
            }

            public string Password
            {
                get
                {
                    var retVal = GetSetting(_password);
                    return retVal;
                }
            }

            public string Code
            {
                get
                {
                    return _code;
                }
            }

            public string Description
            {
                get { return _description; }
            }

            public string LogoUrl
            {
                get { return _logoUrl; }
            }

            public string ServiceUrl
            {
                get
                {
                    var retVal = GetSetting(_serviceUrl);
                    return retVal;
                }
            }

            public string CompanyCode
            {
                get 
                { 
                    var retVal = GetSetting(_companyCode);
                    return retVal;
                }
            }

            public bool IsEnabled
            {
                get
                {
                    var retVal = GetBoolSetting(_isEnabled);
                    return retVal;
                }
            }

            #region IHaveSettings Members

            /// <summary>
            /// Settings of payment method
            /// </summary>
            public ICollection<SettingEntry> Settings { get; set; }

            #endregion

            public string GetSetting(string settingName)
            {
                var setting = Settings.FirstOrDefault(s => s.Name == settingName);

                if (setting == null || string.IsNullOrEmpty(setting.Value))
                    return null;

                return setting.Value;
            }

            public bool GetBoolSetting(string settingName)
            {
                var setting = Settings.FirstOrDefault(s => s.Name == settingName);

                if (setting == null || setting.ValueType != SettingValueType.Boolean)
                    throw new NullReferenceException(string.Format("{0} setting does not exist or not bool"));

                return bool.Parse(setting.Value);
            }

        }
}