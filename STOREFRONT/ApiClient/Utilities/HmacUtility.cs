#region

using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace VirtoCommerce.ApiClient.Utilities
{
    public static class HmacUtility
    {
        #region Constants

        public const string NameValueSeparator = "=";

        public const string ParameterSeparator = "&";

        #endregion

        #region Public Methods and Operators

        public static string ComputeHash(Func<byte[], HMAC> hmacFactory, string secretKey, string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var keyBytes = ConvertHexStringToBytes(secretKey);
            return ComputeHash(hmacFactory, keyBytes, dataBytes);
        }

        public static string ComputeHash(Func<byte[], HMAC> hmacFactory, byte[] secretKey, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            using (var hmac = hmacFactory(secretKey))
            {
                var hash = hmac.ComputeHash(data, 0, data.Length);
                return ConvertBytesToHexString(hash);
            }
        }

        public static string GetHashString(Func<byte[], HMAC> hmacFactory, string secretKey, NameValuePair[] parameters)
        {
            var data = BuildDataString(parameters);
            return ComputeHash(hmacFactory, secretKey, data);
        }

        #endregion

        #region Methods

        private static string BuildDataString(NameValuePair[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            var builder = new StringBuilder();

            var orderedParameters =
                parameters.Where(p => string.IsNullOrEmpty(p.Name) && !string.IsNullOrEmpty(p.Value))
                    .Union(
                        parameters.Where(p => !string.IsNullOrEmpty(p.Name) && !string.IsNullOrEmpty(p.Value))
                            .OrderBy(i => i.Name))
                    .ToList();

            foreach (var parameter in orderedParameters)
            {
                if (builder.Length > 0)
                {
                    builder.Append(ParameterSeparator);
                }

                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    builder.Append(parameter.Name);
                    builder.Append(NameValueSeparator);
                }

                builder.Append(parameter.Value);
            }

            var data = builder.ToString();
            return data;
        }

        private static string ConvertBytesToHexString(byte[] bytes)
        {
            var builder = new StringBuilder();

            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }

            return builder.ToString();
        }

        private static byte[] ConvertHexStringToBytes(string hexString)
        {
            return
                Enumerable.Range(0, hexString.Length)
                    .Where(i => i % 2 == 0)
                    .Select(i => Convert.ToByte(hexString.Substring(i, 2), 16))
                    .ToArray();
        }

        #endregion
    }

    public class NameValuePair
    {
        #region Constructors and Destructors

        public NameValuePair(string name, string value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Public Properties

        public string Name { get; private set; }

        public string Value { get; private set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(Name);
            builder.Append(", ");
            builder.Append(Value);
            builder.Append("]");
            return builder.ToString();
        }

        #endregion
    }

    public class ApiRequestSignature
    {
        #region Constructors and Destructors

        public ApiRequestSignature()
        {
            Timestamp = DateTime.UtcNow;
            TimestampString = Timestamp.ToString("o", CultureInfo.InvariantCulture);
        }

        #endregion

        #region Public Properties

        public string AppId { get; set; }

        public string Hash { get; set; }

        public DateTime Timestamp { get; private set; }

        public string TimestampString { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static bool TryParse(string input, out ApiRequestSignature parsedValue)
        {
            parsedValue = null;
            var success = false;

            if (input != null)
            {
                var parts = input.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3)
                {
                    if (parts[2].Length == 64)
                    {
                        DateTime timestamp;
                        if (DateTime.TryParseExact(
                            parts[1],
                            "o",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.AdjustToUniversal,
                            out timestamp))
                        {
                            parsedValue = new ApiRequestSignature
                            {
                                AppId = parts[0],
                                TimestampString = parts[1],
                                Hash = parts[2],
                                Timestamp = timestamp,
                            };
                            success = true;
                        }
                    }
                }
            }

            return success;
        }

        public override string ToString()
        {
            return string.Join(";", AppId, TimestampString, Hash);
        }

        #endregion
    }
}
