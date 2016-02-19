using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Catalog.Services
{
	/// <summary>
	/// XXX(leter)-XXXXXXXX(number).
	/// </summary>
	public class DefaultSkuGenerator : ISkuGenerator
	{
        private static readonly Random _random = new Random();
        private static readonly object _lockObject = new object();

        #region ISkuGenerator Members

        public string GenerateSku(Model.CatalogProduct product)
		{
			const string leterPart = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string digitPart = "1234567890";
			StringBuilder res = new StringBuilder();

            lock (_lockObject)
            { 
                for (int i = 0; i < 3; i++)
                {
                    res.Append(leterPart[_random.Next(leterPart.Length)]);
                }
                res.Append("-");
                for (int i = 0; i < 8; i++)
                {
                    res.Append(digitPart[_random.Next(digitPart.Length)]);
                }
            }
			return res.ToString();
		}

		#endregion
	}
}
