using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace VirtoCommerce.Foundation.Frameworks.Common
{
	public static class TransformXML
	{
		public static string TransformXml(string xml, string xslt)
		{
			string output = string.Empty;
			output = ClearNamespaces(xml);
			output = Transform(output, xslt);
			return output;
		}

		private static string Transform(string xml, string xslt)
		{
			string output = string.Empty;

			XPathDocument xpd = new XPathDocument(new StringReader(xml));

			XslCompiledTransform transform = new XslCompiledTransform(true);
			transform.Load(new XmlTextReader(xslt, XmlNodeType.Document, null));

			StringWriter sr = new StringWriter();
			transform.Transform(xpd.CreateNavigator(), null, sr);
			output = sr.ToString();

			return output;
		}

		private static string ClearNamespaces(string xml)
		{
			return Transform(xml, removeNamespacesXsl);
		}

		private const string removeNamespacesXsl = @"<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
			<xsl:output method='xml' indent='no'/>
			<xsl:template match='/|comment()|processing-instruction()'>
				<xsl:copy>
				  <xsl:apply-templates/>
				</xsl:copy>
			</xsl:template>

			<xsl:template match='*'>
				<xsl:element name='{local-name()}'>
				  <xsl:apply-templates select='@*|node()'/>
				</xsl:element>
			</xsl:template>

			<xsl:template match='@*'>
				<xsl:attribute name='{local-name()}'>
				  <xsl:value-of select='.'/>
				</xsl:attribute>
			</xsl:template>
			</xsl:stylesheet>";
	}
}
