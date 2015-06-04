using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Common
{
	public abstract class EvaluationContextBase : IEvaluationContext
	{

		public EvaluationContextBase()
		{
			Attributes = new Dictionary<string, string>();
		}

		public object ContextObject { get; set; }

		protected IDictionary<string, string> Attributes { get; set; }

		public string GeoCity { get; set; }

		public string GeoState { get; set; }

		public virtual string GeoCountry { get; set; }

		public virtual string GeoContinent { get; set; }

		public virtual string GeoZipCode { get; set; }

		public virtual string GeoConnectionType { get; set; }

		//this should return timezone
		public virtual string GeoTimeZone { get; set; }

		public virtual string GeoIpRoutingType { get; set; }

		public virtual string GeoIspSecondLevel { get; set; }

		public virtual string GeoIspTopLevel { get; set; }
		public virtual int ShopperAge { get; set; }

		public virtual string ShopperGender { get; set; }

		public virtual string Language { get; set;}


		#region Navigation

		public virtual string ShopperSearchedPhraseInStore { get; set; }

		public virtual string ShopperSearchedPhraseOnInternet { get; set; }

		public virtual string CurrentUrl { get; set; }

		public virtual string ReferredUrl { get; set; }
		#endregion

	
	}

}
