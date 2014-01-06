using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace VirtoCommerce.Foundation.Reporting.Helpers
{
    public class RdlFile
    {
        private readonly XDocument _rdlDocument;
        private readonly XNamespace _ns;

        public struct DataSource
        {
            public string Name;
            public string ConnectionString;
        }

        public struct DataSet
        {
            public string Name;
            public string CommandText;
            public DataSource DataSource;
        }

        public RdlFile(Stream rdlContent)
        {
            _rdlDocument = XDocument.Load(rdlContent);
            if (_rdlDocument.Root != null)
            {
                _ns = _rdlDocument.Root.GetDefaultNamespace();
            }
        }

        private IEnumerable<DataSource> _dataSources;
        public IEnumerable<DataSource> DataSources
        {
            get {
                if (_dataSources == null)
                {
                    ParseDataSources();
                }

                return _dataSources ?? new List<DataSource>();
            }
        }

        private IEnumerable<DataSet> _dataSets;

        public IEnumerable<DataSet> DataSets
        {
            get {
                if (_dataSets == null)
                {
                    ParseDataSets();
                }

                return _dataSets ?? new List<DataSet>();
            }
        }

        public string ReportDefinition
        {
            get { return _rdlDocument.ToString(); }
        }

        private string _description = null;
        public string Description
        {
            get
            {
                if (_description == null)
                {
                    ParseDescription();
                }

                return _description ?? String.Empty;
            }
        }

        private void ParseDescription()
        {
            if (_rdlDocument.Root != null)
            {
                var dsrElement = _rdlDocument.Root.Element(_ns + "Description");
                if (dsrElement != null)
                {
                    _description = dsrElement.Value;
                }
            }

            if (_description == null)
            {
                _description = String.Empty;
            }
        }

        private void ParseDataSources()
        {
            if (_rdlDocument.Root != null)
            {
                var dsrElement = _rdlDocument.Root.Element(_ns + "DataSources");
                if (dsrElement != null)
                {
                    _dataSources = (from xElement in dsrElement.Elements()
                        let connProp = xElement.Element(_ns + "ConnectionProperties")
                        let csBuilder = new SqlConnectionStringBuilder
                        {
                            ConnectionString = connProp.Element(_ns + "ConnectString").Value,
                            IntegratedSecurity = true
                        }
                        select new DataSource
                        {
                            Name = xElement.Attribute("Name").Value,
                            ConnectionString = csBuilder.ConnectionString
                        }).ToList();
                }
            }

            if (_dataSources == null)
            {
                _dataSources = new List<DataSource>();
            }
        }

        private void ParseDataSets()
        {
            if (_rdlDocument.Root != null)
            {
                var dstElement = _rdlDocument.Root.Element(_ns + "DataSets");
                if ( dstElement!=null )
                {
                    _dataSets = (from xElement in dstElement.Elements()
                    let query = xElement.Element(_ns + "Query")
                    let dataSourceName = query.Element(_ns + "DataSourceName").Value
                    let commandText = query.Element(_ns + "CommandText").Value
                    select new DataSet
                    {
                        Name = xElement.Attribute("Name").Value,
                        CommandText = commandText,
                        DataSource = DataSources.First(d => d.Name == dataSourceName)
                    }).ToList();
                }
            }

            if (_dataSets == null)
            {
                _dataSets = new List<DataSet>();
            }
        }
    }
}
