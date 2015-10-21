using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotLiquid.Exceptions;
using DotLiquid.FileSystems;

namespace DotLiquid.ViewEngine.FileSystems
{
    public class ThemeLiquidFileSystem : IFileSystem
    {
        private readonly string _basePath;
        private const string _liquidTemplateFormat = "{0}.liquid";
        public ThemeLiquidFileSystem(string basePath) 
        {
            _basePath = basePath;
        }

        #region IFileSystem members
        public string ReadTemplateFile(Context context, string templateName)
        {
            return ReadTemplateByName((string)context[templateName]);
        }
        #endregion

        public string ReadTemplateByName(string templateName)
        {
            if (templateName == null || !Regex.IsMatch(templateName, @"^[a-zA-Z0-9_]+$"))
                throw new FileSystemException("Error - Illegal template name '{0}'", templateName);


            var baseDirectory = new DirectoryInfo(_basePath);
            var templateFile = baseDirectory.GetFiles(String.Format(_liquidTemplateFormat, templateName), SearchOption.AllDirectories).FirstOrDefault();

            if (templateFile == null)
                throw new FileSystemException("Error - No such template {0} . Looked in the following locations:<br />{1}", templateName, _basePath);

            using (var stream = templateFile.Open(FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
