﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Domain.Tax.Model
{
    /// <summary>
    /// Represent base class for all potential tax calculation integrations
    /// </summary>
    public abstract class TaxProvider : Entity, IHaveSettings
    {
        public TaxProvider(string code)
        {
            Id = Guid.NewGuid().ToString("N");
            Code = code;
            IsActive = false;
        }

        /// <summary>
        /// Method identity property (System name)
        /// </summary>
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public int Priority { get; set; }

        public abstract IEnumerable<TaxRate> CalculateRates(IEvaluationContext context);

        #region IHaveSettings Members

        public ICollection<SettingEntry> Settings { get; set; }

        #endregion

        public string GetSetting(string settingName)
        {
            var setting = Settings.FirstOrDefault(s => s.Name == settingName);
                        
            return setting != null ? setting.Value : string.Empty;
        }
    }
}
