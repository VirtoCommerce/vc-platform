using System;
using System.Xml.Serialization;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ConfigurationUtility.Main.Models
{
	public class Project : NotifyPropertyChanged
	{
		private string _browseUrl;

		[XmlAttribute(AttributeName = "id")]
		public Guid Id { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute]
		public DateTime Created { get; set; }
		[XmlAttribute(AttributeName = "browseUrl")]
		public string BrowseUrl
		{
			get { return _browseUrl; }
			set { _browseUrl = value; OnPropertyChanged(); }
		}

		[XmlIgnore]
		public Status Status { get; set; }
		[XmlElement(ElementName = "location")]
		public ProjectLocation Location { get; set; }
	}

	public enum LocationType
	{
		FileSystem
	}

	public enum Status
	{
		Online,
		Offline
	}

	public class ProjectLocation
	{
		[XmlAttribute(AttributeName = "type")]
		public LocationType Type { get; set; }
		[XmlAttribute(AttributeName = "url")]
		public string Url { get; set; }
		[XmlAttribute(AttributeName = "localPath")]
		public string LocalPath { get; set; }

		public override string ToString()
		{
			return LocalPath;
		}
	}
}
