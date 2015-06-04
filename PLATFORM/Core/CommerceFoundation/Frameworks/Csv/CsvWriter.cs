using System.Collections.Generic;
using System.IO;

namespace VirtoCommerce.Foundation.Frameworks.Csv
{
	public class CsvWriter
	{
		private CsvStream _csvStream;

		public CsvWriter(TextWriter writer, string columnDelimiter)
		{
			_csvStream = new CsvStream(null, writer, columnDelimiter);
		}

		public void WriteRow(IEnumerable<string> items, bool quoteAllFields)
		{
			_csvStream.WriteRow(items, quoteAllFields);
		}
	}
}
