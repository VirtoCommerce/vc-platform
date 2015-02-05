using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Templates;
using VirtoCommerce.Web.Client.Extensions;

namespace VirtoCommerce.Web.Client.Services.Templates
{
    /// <summary>
    /// Class TemplateService.
    /// </summary>
	public class TemplateService : ITemplateService
	{
		#region Cache Constants

        /// <summary>
        /// The template cache key
        /// </summary>
		public const string TemplateCacheKey = "XSL:T:{0}";

		#endregion

        /// <summary>
        /// The _lock object
        /// </summary>
		private readonly object _lockObject = new object();
        /// <summary>
        /// The _cache helper
        /// </summary>
		private CacheHelper _cacheHelper;

		#region Private Variables

        /// <summary>
        /// The _is enabled
        /// </summary>
		private readonly bool _isEnabled;
        /// <summary>
        /// The _cache repository
        /// </summary>
		private readonly ICacheRepository _cacheRepository;
        /// <summary>
        /// The _repository
        /// </summary>
		private readonly IAppConfigRepository _repository;

		#endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="cacheRepository">The cache repository.</param>
		public TemplateService(IAppConfigRepository repository, ICacheRepository cacheRepository)
		{
			_repository = repository;
			_cacheRepository = cacheRepository;
			_isEnabled = AppConfigConfiguration.Instance.Cache.IsEnabled;
		}

        /// <summary>
        /// Gets the helper.
        /// </summary>
        /// <value>The helper.</value>
		private CacheHelper Helper
		{
			get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
		}

        /// <summary>
        /// Processes the template.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="context">The context.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>IProcessedTemplate.</returns>
		public IProcessedTemplate ProcessTemplate(string templateName, IDictionary<string, object> context,
		                                          CultureInfo culture)
		{
			IProcessedTemplate returnMessage = new ProcessedTemplate();
			var template = LoadTemplateFromRepository(templateName);

			if (template == null)
			{
				// TODO: write log that template was not found
				return null;
			}

			var templateBody = template.Body;
			returnMessage.Subject = template.Subject;

			// get an appropriate language
			if (template.EmailTemplateLanguages.Any())
			{
				var langTemplate = template.EmailTemplateLanguages.Where(t => t.LanguageCode == culture.Name)
				                           .OrderBy(t => t.Priority).FirstOrDefault();

				if (langTemplate != null)
				{
					templateBody = langTemplate.Body;
					returnMessage.Subject = langTemplate.Subject;
				}
			}
			try
			{
				returnMessage.Type = (EmailTemplateTypes)Enum.Parse(typeof(EmailTemplateTypes), template.Type);

				switch (returnMessage.Type)
				{
					case EmailTemplateTypes.Xsl:
						returnMessage.Body = ProcessXslTemplate(templateBody, context);
						break;
					case EmailTemplateTypes.Html:
						returnMessage.Body = ProcessHtmlTemplate(templateBody, context);
						break;
                    case EmailTemplateTypes.Razor:
                        returnMessage.Body = ProcessRazorTemplate(templateBody, context);
                        break;
					case EmailTemplateTypes.Text:
						returnMessage.Body = ProcessTextTemplate(templateBody, context);
						break;
				}
			}
			catch(Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}

			return returnMessage;
		}

        /// <summary>
        /// Processes the razor template.
        /// </summary>
        /// <param name="templateBody">The template body.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string ProcessRazorTemplate(string templateBody, IDictionary<string, object> context)
        {
            return ViewRenderer.RenderTemplate(templateBody, context);
        }

        /// <summary>
        /// Processes the text template.
        /// </summary>
        /// <param name="textBody">The text body.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
		private string ProcessTextTemplate(string textBody, IDictionary<string, object> context)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Processes the HTML template.
        /// </summary>
        /// <param name="htmlBody">The HTML body.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
		private string ProcessHtmlTemplate(string htmlBody, IDictionary<string, object> context)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Processes the XSL template.
        /// </summary>
        /// <param name="xslBody">The XSL body.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
		private string ProcessXslTemplate(string xslBody, IDictionary<string, object> context)
		{
			// 1. Serialize all context variables into XML            
			using (var stream = new MemoryStream())
			{
				// Start creating xml document
				using (var exportWriter = new NoNamespacesXmlTextWriter(stream))
				{
					// Start the Xml Document
					exportWriter.WriteStartDocument();

					exportWriter.WriteStartElement("ContextDoc", "");

					// Cycle through dictionary
					foreach (var key in context.Keys)
					{
						var contextObject = context[key];
						var serializer = new DataContractSerializer(contextObject.GetType());
						serializer.WriteObject(exportWriter, contextObject);
					}

					exportWriter.WriteEndElement(); // End of ContextDoc
					exportWriter.WriteEndDocument();
					exportWriter.Flush();
					//exportWriter.Close();

					// 2. Locate XSL Template
					using (var xslStream = new MemoryStream())
					{
						var bytes = Encoding.UTF8.GetBytes(xslBody);
						xslStream.Write(bytes, 0, bytes.Length);
						xslStream.Position = 0;

						using (var reader = XmlReader.Create(xslStream))
						{
							var xslt = new XslCompiledTransform();
							xslt.Load(reader);

							// 3. Transform Content
							stream.Position = 0;
							var pathDoc = new XPathDocument(stream);

							using (var writer = new StringWriter())
							{
								xslt.Transform(pathDoc, null, writer);

								// Return contents
								return writer.ToString();
							}
						}
					}
				}
			}
		}

        /// <summary>
        /// Loads the template from repository.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>EmailTemplate.</returns>
		private EmailTemplate LoadTemplateFromRepository(string name)
		{
			var templates = LoadTemplates();
			var template = (from t in templates where t.Name == name select t).SingleOrDefault();

			return template;
		}

        /// <summary>
        /// Loads the templates.
        /// </summary>
        /// <returns>IEnumerable{EmailTemplate}.</returns>
		private IEnumerable<EmailTemplate> LoadTemplates()
		{
			lock (_lockObject)
			{
				return Helper.Get(
                    CacheHelper.CreateCacheKey(Foundation.Constants.EmailTemplateCachePrefix, string.Format(TemplateCacheKey, "all")),
					() => _repository.EmailTemplates.ExpandAll().ToArray(),
					AppConfigConfiguration.Instance.Cache.DisplayTemplateMappingsTimeout,
					_isEnabled);
			}
		}

        /// <summary>
        /// Class NoNamespacesXmlTextWriter.
        /// </summary>
		public class NoNamespacesXmlTextWriter : XmlTextWriter
		{
            /// <summary>
            /// The _settings
            /// </summary>
			private XmlWriterSettings _settings;

            /// <summary>
            /// Initializes a new instance of the <see cref="NoNamespacesXmlTextWriter"/> class.
            /// </summary>
            /// <param name="stream">The stream.</param>
			public NoNamespacesXmlTextWriter(Stream stream)
				: base(stream, null)
			{
			}

            /// <summary>
            /// Gets the <see cref="T:System.Xml.XmlWriterSettings" /> object used to create this <see cref="T:System.Xml.XmlWriter" /> instance.
            /// </summary>
            /// <value>The settings.</value>
            /// <returns>The <see cref="T:System.Xml.XmlWriterSettings" /> object used to create this writer instance. If this writer was not created using the <see cref="Overload:System.Xml.XmlWriter.Create" /> method, this property returns null.</returns>
			public override XmlWriterSettings Settings
			{
				get
				{
					return _settings ?? (_settings = new XmlWriterSettings
						{
							Indent = true,
							NamespaceHandling = NamespaceHandling.OmitDuplicates
						});
				}
			}

            /// <summary>
            /// Writes the specified start tag and associates it with the given namespace and prefix.
            /// </summary>
            /// <param name="prefix">The namespace prefix of the element.</param>
            /// <param name="localName">The local name of the element.</param>
            /// <param name="ns">The namespace URI to associate with the element. If this namespace is already in scope and has an associated prefix then the writer automatically writes that prefix also.</param>
			public override void WriteStartElement(string prefix, string localName, string ns)
			{
				base.WriteStartElement(null, localName, "");
			}
		}
	}


    
}