using System.IO;

namespace VirtoCommerce.Foundation.Frameworks.Csv
{
	public class CsvReader
	{
		private readonly CsvStream _csvStream;

		public int CurrentLineNumber { get { return _csvStream.CurrentLineNumber; } }
		public long CurrentPosition { get {return _csvStream.CurrentPosition; } }
		public long StreamLength { get { return _csvStream.TotalLength; } }

		public CsvReader(TextReader reader, string columnDelimiter)
		{
			_csvStream = new CsvStream(reader, null, columnDelimiter);
		}
		
		public string[] ReadRow()
		{
			string[] items = null;

			var row = _csvStream.ReadRow();
			
			if (row != null)
				items = row.ToArray();

			return items;
		}
	}
}
