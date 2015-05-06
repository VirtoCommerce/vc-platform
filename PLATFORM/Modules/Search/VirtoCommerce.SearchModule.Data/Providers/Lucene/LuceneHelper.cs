using System;
using Lucene.Net.Documents;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.SearchModule.Data.Providers.Lucene
{
    public class LuceneHelper
    {

        /// <summary>
        ///     Converts the search document to lucene document
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public static Document ConvertDocument(IDocument document)
        {
            var doc = new Document();
            for (var index = 0; index < document.FieldCount; index++)
            {
                var field = document[index];
                AddFieldToDocument(ref doc, field);
            }

            return doc;
        }


        /// <summary>
        ///     Adds the field to the lucene document.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="field">The field.</param>
        private static void AddFieldToDocument(ref Document doc, IDocumentField field)
        {
            if (field == null)
            {
                return;
            }

            var store = Field.Store.YES;
            var index = Field.Index.NOT_ANALYZED;

            if (field.ContainsAttribute(value: IndexStore.NO))
            {
                store = Field.Store.NO;
            }

            if (field.ContainsAttribute(IndexType.ANALYZED))
            {
                index = Field.Index.ANALYZED;
            }
            else if (field.ContainsAttribute(IndexType.NO))
            {
                index = Field.Index.NO;
            }

            if (field.Value == null)
            {
                return;
            }

            field.Name = field.Name.ToLower();
            if (field.Name == "__key")
            {
                foreach (var val in field.Values)
                {
                    doc.Add(new Field(field.Name, val.ToString(), store, index));
                }
            }
            else if (field.Value is string)
            {
                foreach (var val in field.Values)
                {
                    doc.Add(new Field(field.Name, val.ToString(), store, index));
                    doc.Add(new Field("_content", val.ToString(), Field.Store.NO, Field.Index.ANALYZED));
                }
            }
            else if (field.Value is decimal) // parse prices
            {
                foreach (var val in field.Values)
                {
                    var numericField = new NumericField(field.Name, store, index != Field.Index.NO);
                    numericField.SetDoubleValue(double.Parse(val.ToString()));
                    doc.Add(numericField);
                }
            }
            else if (field.Value is DateTime) // parse dates
            {
                foreach (var val in field.Values)
                {
                    doc.Add(
                        new Field(
                            field.Name, DateTools.DateToString((DateTime)val, DateTools.Resolution.SECOND), store, index));
                }
            }
            else // try detecting the type
            {
                // TODO: instead of auto detecting, use meta data information
                decimal t;
                if (Decimal.TryParse(field.Value.ToString(), out t))
                {
                    foreach (var val in field.Values)
                    {
                        var numericField = new NumericField(field.Name, store, index != Field.Index.NO);
                        numericField.SetDoubleValue(double.Parse(val.ToString()));
                        doc.Add(numericField);
                    }
                }
                else
                {
                    foreach (var val in field.Values)
                    {
                        doc.Add(new Field(field.Name, val.ToString(), store, index));
                    }
                }
            }
        }
    }
}