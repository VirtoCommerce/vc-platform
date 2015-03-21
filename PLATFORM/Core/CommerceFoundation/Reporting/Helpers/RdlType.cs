using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace VirtoCommerce.Foundation.Reporting.Helpers
{
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2010/01/reportdefinition")]
    [XmlRootAttribute(ElementName = "Report", Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2010/01/reportdefinition", IsNullable = false)]
    public class RdlType
    {
        public static RdlType Load(Stream rdlContent)
        {
            XmlDocument _Doc = new XmlDocument();
            _Doc.Load(rdlContent);
            XmlSerializer _XMLSer = new XmlSerializer(typeof(RdlType));
            var rdl = (RdlType)_XMLSer.Deserialize(new StringReader(_Doc.OuterXml));
            rdl.SetReportParametersToDefault();
            
            return rdl;
        }

        [XmlType("DataSource")]
        public class DataSourceType
        {
            [XmlType("ConnectionProperties")]
            public class ConnectionPropertiesType
            {
                public string DataProvider;
                public string ConnectString;
                public bool IntegratedSecurity;
            }

            [XmlAttribute]
            public string Name;
            public ConnectionPropertiesType ConnectionProperties;
        }

        [XmlType("DataSet")]
        public class DataSetsType
        {
            [XmlType("Query")]
            public class QueryType
            {
                [XmlType("QueryParameter")]
                public class QueryParameterType
                {
                    [XmlAttribute]
                    public string Name;
                    public string Value;
                }

                public string DataSourceName;
                public string CommandText;
                public List<QueryParameterType> QueryParameters;
            }

            [XmlAttribute]
            public string Name;
            public QueryType Query;
        }

        [XmlType("ReportParameter")]
        public class ReportParameterType
        {
            [XmlType("ValidValues")]
            public class ValidValuesType
            {
                [XmlType("ParameterValue")]
                public class ParameterValueType
                {
                    public string Value;
                    public string Label;
                }

                public List<ParameterValueType> ParameterValues;
            }

            [XmlType("DefaultValue")]
            public class DefaultValueType
            {
                [XmlType("Value")]
                public class ValueType
                {
                    public string Value;
                }

                [XmlElement("Values")]
                public List<ValueType> Values;
            }

            public enum DataTypeEnum
            {
                Boolean,
                DateTime,
                Integer,
                Float,
                String
            }

            [XmlAttribute]
            public string Name;
            public DataTypeEnum DataType;
            public bool Nullable;
            public DefaultValueType DefaultValue;
            public bool AllowBlank;
            public string Prompt;
            public ValidValuesType ValidValues;
            public bool Hidden;
            public bool MultiValue;

            public bool Visible
            {
                get { return !Hidden; }
            }

            public bool HasValidValues
            {
                get { return ValidValues != null && ValidValues.ParameterValues.Any(); }
            }

            private object _value;
            public object Value {
                get { return _value; }
                set
                {
                    switch (DataType)
                    {
                        case DataTypeEnum.Boolean:
                            _value = (value is bool) ? value : Convert.ToBoolean(value);
                            break;
                        case DataTypeEnum.DateTime:
                            _value = (value is DateTime) ? value : Convert.ToDateTime(value);
                            break;
                        case DataTypeEnum.Float:
                            _value = (value is Double)
                                ? value
                                : Convert.ToDouble(value);
                            break;
                        case DataTypeEnum.Integer:
                            _value = (value is int) ? value : Convert.ToInt32(value);
                            break;
                        case DataTypeEnum.String:
                            _value = value.ToString();
                            break;
                        default:
                            _value = value;
                            break;
                    }
                }
            }
        }

        public string Description;
        public List<DataSourceType> DataSources;
        public List<DataSetsType> DataSets;
        public List<ReportParameterType> ReportParameters;

        public IDictionary<string, object> GetDataSetQueryParameters(string dataSetName)
        {
            var paramList = new Dictionary<string, object>();
            var dataSet = this.DataSets.FirstOrDefault(d => d.Name == dataSetName);
            if (dataSet == null)
            {
                return paramList;
            }

            foreach (var queryParameter in dataSet.Query.QueryParameters)
            {
                var value = EvaluateExpression(queryParameter.Value);
                paramList.Add(queryParameter.Name, value);
            }

            return paramList;
        }

        public object EvaluateExpression(string expression)
        {
            var expr = new RdlExpression(expression)
            {
                GetParameterValue = GetReportParameterValue
            };

            return expr.Evaluate();
        }

        public bool HasReportParameters
        {
            get { return ReportParameters.Any(r => r.Visible); }
        }

        public bool ReportParametersAreValid
        {
            get { return ReportParameters.TrueForAll(p => p.AllowBlank || p.Value != null); }
        }

        public void UpdateReportParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                var reportParam = ReportParameters.FirstOrDefault(r => r.Name == parameter.Key);
                if (reportParam != null)
                {
                    reportParam.Value = parameter.Value;
                }
            }
        }

        public void SetReportParametersToDefault()
        {
            foreach (var parameter in ReportParameters.Where(p=>p.DefaultValue!=null))
            {
                var defaultType = parameter.DefaultValue.Values.FirstOrDefault();
                parameter.Value = defaultType != null ? EvaluateExpression(defaultType.Value) : null;
            }
        }

        private object GetReportParameterValue(string paramName)
        {
            var param = ReportParameters.FirstOrDefault(prm => prm.Name == paramName);
            if (param != null)
            {
                return param.Value;
            }

            throw new RdlExpressionSyntaxException("Report parameter {0} is undefined", paramName);
        }

        public IDictionary<string, object> GetReportParameterValidValues(string paramName)
        {
            var param = ReportParameters.FirstOrDefault(prm => prm.Name == paramName);
            var list = new Dictionary<string, object>();
            if (param != null)
            {
                foreach (var prm in param.ValidValues.ParameterValues)
                {
                    var value = EvaluateExpression(prm.Value);
                    list.Add(prm.Label, value);
                }
            }

            return list;
        }
    }
}
