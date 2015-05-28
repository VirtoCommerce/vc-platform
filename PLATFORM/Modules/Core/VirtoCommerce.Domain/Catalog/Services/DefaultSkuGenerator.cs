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
		#region ISkuGenerator Members

		public string GenerateSku(Model.CatalogProduct product)
		{
			const string leterPart = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string digitPart = "1234567890";
			StringBuilder res = new StringBuilder();

			Random rnd = new Random();
			for (int i = 0; i < 3; i++)
			{
				res.Append(leterPart[rnd.Next(leterPart.Length)]);
			}
			res.Append("-");
			for (int i = 0; i < 8; i++)
			{
				res.Append(digitPart[rnd.Next(leterPart.Length)]);
			}
			return res.ToString();
		}

		#endregion
	}
}
