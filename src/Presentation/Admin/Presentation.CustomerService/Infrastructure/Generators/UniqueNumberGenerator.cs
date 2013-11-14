using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Generators
{
	public class UniqueNumberGenerator
	{

		/// <summary>
		/// http://www.codeproject.com/Articles/14403/Generating-Unique-Keys-in-Net
		/// </summary>
		/// <returns></returns>
		public static string GetUniqueNumber()
		{
			//string result = DateTime.Now.Year.ToString() + Guid.NewGuid().ToString().GetHashCode().ToString("x");
			//return result;

			int maxSize = 8;
			char[] chars = new char[62];
			string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			chars = a.ToCharArray();
			int size = maxSize;
			byte[] data = new byte[1];

			var crypto = new RNGCryptoServiceProvider();
			crypto.GetNonZeroBytes(data);
			data = new byte[size];
			crypto.GetNonZeroBytes(data);
			var result = new StringBuilder(size);

			foreach (byte b in data)
			{
				result.Append(chars[b % (chars.Length - 1)]);
			}

			return "CS" + result.ToString();

		}


	}
}
