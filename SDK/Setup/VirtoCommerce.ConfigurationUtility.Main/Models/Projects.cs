using System.Collections.Generic;
using System.Xml.Serialization;

namespace VirtoCommerce.ConfigurationUtility.Main.Models
{
	[XmlRoot(ElementName = "Projects")]
	public class Projects : List<Project>
	{
	}
}
