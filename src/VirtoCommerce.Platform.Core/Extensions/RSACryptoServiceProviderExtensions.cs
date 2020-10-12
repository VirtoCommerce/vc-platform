using System;
using System.Security.Cryptography;
using System.Xml;

namespace VirtoCommerce.Platform.Core.Extensions
{
    /// <summary>
    /// Custom implementation of RSA.FromXmlString/RSA.ToXmlString
    /// because .net core 2.2 not implement it
    /// Remove it when .net core 3 Release
    /// </summary>
#pragma warning disable S101 // Types should be named in PascalCase

    public static class RSACryptoServiceProviderExtensions
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public static void FromXmlStringCustom(this RSACryptoServiceProvider rsa, string xmlString, bool includePrivateParameters = false)
        {
            var parameters = rsa.ExportParameters(includePrivateParameters);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (!xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                throw new ArgumentException("Invalid XML RSA key.");
            }

            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                    case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                    case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                    case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                    case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                    case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                    case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                    case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                }
            }

            rsa.ImportParameters(parameters);
        }

        public static string ToXmlStringCustom(this RSACryptoServiceProvider rsa, bool includePrivateParameters = false)
        {
            var parameters = rsa.ExportParameters(includePrivateParameters);
            if (includePrivateParameters)
            {
                return $@"<RSAKeyValue>
                            <Modulus>{Convert.ToBase64String(parameters.Modulus)}</Modulus>
                            <Exponent>{Convert.ToBase64String(parameters.Exponent)}</Exponent>
                            <P>{Convert.ToBase64String(parameters.P)}</P>
                            <Q>{Convert.ToBase64String(parameters.Q)}</Q>
                            <DP>{Convert.ToBase64String(parameters.DP)}</DP>
                            <DQ>{Convert.ToBase64String(parameters.DQ)}</DQ>
                            <InverseQ>{Convert.ToBase64String(parameters.InverseQ)}</InverseQ>
                            <D>{Convert.ToBase64String(parameters.D)}</D>
                       </RSAKeyValue>";
            }

            return $@"<RSAKeyValue>
                        <Modulus>{Convert.ToBase64String(parameters.Modulus)}</Modulus>
                        <Exponent>{Convert.ToBase64String(parameters.Exponent)}</Exponent>
                      </RSAKeyValue>";
        }
    }
}
