using System;
using System.IO;
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
            var cryptoServiceProvider = new T();
            return ComputeHash(instance, cryptoServiceProvider);
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
            var cryptoServiceProvider = new T { Key = key };
            return ComputeHash(instance, cryptoServiceProvider);
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
            return ComputeHash(instance, MD5.Create());
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
            return ComputeHash(instance, SHA1.Create());
        }

        private static string ComputeHash<T>(object instance, T cryptoServiceProvider) where T : HashAlgorithm
        {
            var xmlSerializer = new XmlSerializer(instance.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, instance);
                cryptoServiceProvider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(textWriter.ToString()));
                return Convert.ToBase64String(cryptoServiceProvider.Hash);
            }
        }

        public static Nullable<T> ToNullable<T>(this object obj) where T : struct
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is string objString)
            {
                return objString.ToNullable<T>();
            }

            var objType = typeof(T);
            if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                objType = Nullable.GetUnderlyingType(objType);
            }
            return (T)Convert.ChangeType(obj, objType);
        }

        public static T CloneTyped<T>(this T instance) where T : ICloneable
        {
            return (T)instance.Clone();
        }
    }
}
