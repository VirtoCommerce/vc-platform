using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Virtoway.WPF.State
{
	#region State XML Data Types

	[XmlRoot("state")]
	public class State
	{
		[XmlElement("element")]
		public readonly List<Element> Elements = new List<Element>();
	}

	[XmlType("element")]
	public class Element
	{
		[XmlAttribute("uid")]
		public string UId;

		[XmlElement("property")]
		public readonly List<Property> Properties = new List<Property>();
	}

	[XmlType("property")]
	public class Property
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlAttribute("value")]
		public string Value;
	}

	#endregion
}