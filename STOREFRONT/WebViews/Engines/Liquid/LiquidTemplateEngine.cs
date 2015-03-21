#region
using System.Collections.Generic;
using System.IO;
using DotLiquid;
using DotLiquid.FileSystems;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid
{

    #region
    #endregion

    public class LiquidTemplateEngine : ITemplateEngine
    {
        #region Fields
        private readonly string _sourceFolder = "";
        #endregion

        #region Constructors and Destructors
        public LiquidTemplateEngine(string sourceFolder)
        {
            //Template.NamingConvention = new RubyNamingConvention();
            DotLiquid.Liquid.UseRubyDateFormat = true;
            this._sourceFolder = sourceFolder;
            this.Initialize();
        }
        #endregion

        #region Public Methods and Operators
        public bool CanProcess(string inputType, string outputType)
        {
            return true;
        }

        public string Process(string contents, IDictionary<string, dynamic> attributes)
        {
            var data = this.CreatePageData(contents, attributes);
            var template = Template.Parse(contents);
            Template.FileSystem = new Includes(this._sourceFolder);
            var output = template.Render(data);
            return output;
        }
        #endregion

        #region Methods
        private Hash CreatePageData(string contents, IDictionary<string, dynamic> attributes)
        {
            var y = Hash.FromDictionary(attributes);
            return y;
        }

        private void Initialize()
        {
            Template.RegisterFilter(typeof(CommonFilters));
        }
        #endregion

        internal class Includes : IFileSystem
        {
            #region Constructors and Destructors
            public Includes(string root)
            {
                this.Root = root;
            }
            #endregion

            #region Public Properties
            public string Root { get; set; }
            #endregion

            #region Public Methods and Operators
            public string ReadTemplateFile(Context context, string templateName)
            {
                var include = Path.Combine(this.Root, "snippets", templateName);
                if (File.Exists(include))
                {
                    return File.ReadAllText(include);
                }
                return string.Empty;
            }
            #endregion
        }
    }
}