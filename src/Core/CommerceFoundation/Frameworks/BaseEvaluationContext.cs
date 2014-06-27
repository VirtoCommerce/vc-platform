using System;
using System.Collections.Generic;

namespace VirtoCommerce.Foundation.Frameworks
{
	public abstract class BaseEvaluationContext: IEvaluationContext
	{
		public object ContextObject { get; set; }

		protected IDictionary<string, object> Context
		{
			get
			{
				return ContextObject as IDictionary<string, object>;
			}
            set
            {
                ContextObject = value;
            }
		}

		public virtual string GeoCity
		{
			get
			{
				return GetStringValue(ContextFieldConstants.GeoCity);
			}
		}

		public virtual string GeoState
		{
			get
			{
				return GetStringValue(ContextFieldConstants.GeoState);
			}
		}

		public virtual string GeoCountry
		{
			get
			{
				return GetStringValue(ContextFieldConstants.GeoCountry);
			}
		}

		public virtual string GeoContinent
		{
			get
			{
				return GetStringValue(ContextFieldConstants.GeoContinent);
			}
		}

		public virtual string GeoZipCode
		{
			get
			{
				return GetStringValue(ContextFieldConstants.GeoZipCode);
			}
		}

		public virtual string GeoConnectionType
		{
			get
			{
				return GetStringValue(ContextFieldConstants.GeoConnectionType);
			}
		}
		
		//this should return timezone
		public virtual TimeSpan GeoTimeZone
		{
			get
			{
                var timespan = GetTimeSpanValue(ContextFieldConstants.GeoIspTopLevel);

                if (timespan.HasValue)
                {
                    return timespan.Value;
                }

				return new TimeSpan();
			}
		}

		public virtual string GeoIpRoutingType
		{
			get
			{
                return GetStringValue(ContextFieldConstants.GeoIpRoutingType);
			}
		}

		public virtual string GeoIspSecondLevel
		{
			get
			{
                return GetStringValue(ContextFieldConstants.GeoIspSecondLevel);
			}
		}

		public virtual string GeoIspTopLevel
		{
			get
			{
                return GetStringValue(ContextFieldConstants.GeoIspTopLevel);
			}
		}

        public virtual int ShopperAge
        {
            get
            {
                var age = GetDecimalValue(ContextFieldConstants.UserAge);

                return age.HasValue ? Convert.ToInt32(age.Value) : 0;
            }
        }

        public virtual string ShopperGender
        {
            get
            {
                return GetStringValue(ContextFieldConstants.UserGender);
            }
        }

        public virtual string Language
        {
            get
            {
                return GetStringValue(ContextFieldConstants.Language);
            }
        }

        #region Navigation

        public virtual string ShopperSearchedPhraseInStore
        {
            get
            {
                return GetStringValue(ContextFieldConstants.StoreSearchPhrase);
            }
        }

        public virtual string ShopperSearchedPhraseOnInternet
        {
            get
            {
                return GetStringValue(ContextFieldConstants.InternetSearchPhrase);
            }
        }

        public virtual string CurrentUrl
        {
            get
            {
                return GetStringValue(ContextFieldConstants.CurrentUrl);
            }
        }

        public virtual string ReferredUrl
        {
            get
            {
                return GetStringValue(ContextFieldConstants.ReferredUrl);
            }
        }
        #endregion

        protected virtual string GetStringValue(string name)
        {
            if (Context != null && Context.Count > 0 && Context.ContainsKey(name))
            {
                return Convert.ToString(Context[name]);
            }

            return String.Empty;
        }

        protected virtual decimal? GetDecimalValue(string name)
        {
            if (Context != null && Context.Count > 0 && Context.ContainsKey(name))
            {
                return Convert.ToDecimal(Context[name]);
            }

            return null;
        }

        protected virtual TimeSpan? GetTimeSpanValue(string name)
        {
            if (Context != null && Context.Count > 0 && Context.ContainsKey(name))
            {
                return ((TimeZoneInfo)Context[name]).BaseUtcOffset;
            }

            return null;
        }
	}
}
