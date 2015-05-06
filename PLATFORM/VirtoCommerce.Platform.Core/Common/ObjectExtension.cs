using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Common
{
	/// <summary>
	///     Extension methods applied to the <see cref="object"/> type.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		///     Gets a hash of the current instance.
		/// </summary>
		/// <typeparam name="T">
		///     The type of the Cryptographic Service Provider to use.
		/// </typeparam>
		/// <param name="instance">
		///     The instance being extended.
		/// </param>
		/// <returns>
		///     A base 64 encoded string representation of the hash.
		/// </returns>
		public static string GetHash<T>(this object instance) where T : HashAlgorithm, new()
		{
			T cryptoServiceProvider = new T();
			return computeHash(instance, cryptoServiceProvider);
		}

		/// <summary>
		///     Gets a key based hash of the current instance.
		/// </summary>
		/// <typeparam name="T">
		///     The type of the Cryptographic Service Provider to use.
		/// </typeparam>
		/// <param name="instance">
		///     The instance being extended.
		/// </param>
		/// <param name="key">
		///     The key passed into the Cryptographic Service Provider algorithm.
		/// </param>
		/// <returns>
		///     A base 64 encoded string representation of the hash.
		/// </returns>
		public static string GetKeyedHash<T>(this object instance, byte[] key) where T : KeyedHashAlgorithm, new()
		{
			T cryptoServiceProvider = new T { Key = key };
			return computeHash(instance, cryptoServiceProvider);
		}

		/// <summary>
		///     Gets a MD5 hash of the current instance.
		/// </summary>
		/// <param name="instance">
		///     The instance being extended.
		/// </param>
		/// <returns>
		///     A base 64 encoded string representation of the hash.
		/// </returns>
		public static string GetMD5Hash(this object instance)
		{
			return instance.GetHash<MD5CryptoServiceProvider>();
		}

		/// <summary>
		///     Gets a SHA1 hash of the current instance.
		/// </summary>
		/// <param name="instance">
		///     The instance being extended.
		/// </param>
		/// <returns>
		///     A base 64 encoded string representation of the hash.
		/// </returns>
		public static string GetSHA1Hash(this object instance)
		{
			return instance.GetHash<SHA1CryptoServiceProvider>();
		}

		private static string computeHash<T>(object instance, T cryptoServiceProvider) where T : HashAlgorithm, new()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(instance.GetType());

			using (StringWriter textWriter = new StringWriter())
			{
				xmlSerializer.Serialize(textWriter, instance);
				cryptoServiceProvider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(textWriter.ToString()));
				return Convert.ToBase64String(cryptoServiceProvider.Hash);
			}
		}
	}
}