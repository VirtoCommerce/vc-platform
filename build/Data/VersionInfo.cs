using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Nuke.Common.Utilities;

namespace Data
{
    public class VersionInfo
    {
        private XmlDocument _xml;
        private readonly string versionPrefixXPath = "//*/VersionPrefix";
        private readonly string versionSuffixXPath = "//*/VersionSuffix[not(@Condition)]";
        public VersionInfo(string Path)
        {
            _xml = new XmlDocument();
            _xml.Load(Path);
        }

        public string VersionPrefix => _xml.SelectSingleNode(versionPrefixXPath).InnerText;
        public string VersionSuffix => _xml.SelectSingleNode(versionSuffixXPath).InnerText;
        public string SemVer => VersionSuffix.IsNullOrEmpty() ? VersionPrefix : $"{VersionPrefix}-{VersionSuffix}";
    }
}
