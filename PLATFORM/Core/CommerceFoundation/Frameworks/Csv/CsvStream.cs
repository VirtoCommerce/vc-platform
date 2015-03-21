using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Csv
{
	internal class CsvStream
	{
		private const char _CR = '\r';
		private const char _LF = '\n';
		private const char _textQualifier = '"';
		private static readonly string _singleTextQualifierString = _textQualifier.ToString();
		private static readonly string _doubleTextQualifierString = _singleTextQualifierString + _singleTextQualifierString;

		private readonly char[] _buffer = new char[0x1000];
		private bool _endOfLine;
		private bool _endOfStream;
		private int _length;
		private readonly TextReader _reader;
		private readonly TextWriter _writer;
		private char _columnDelimiter;
		private readonly char[] _specialChars;
		private int _pos;
		private bool _previousWasCR;

		public int CurrentLineNumber { get; private set; }
		public long CurrentPosition { get; private set; }
		public long TotalLength { get; private set; }

		internal CsvStream(TextReader reader, TextWriter writer, string columnDelimiter)
		{
			_reader = reader;
			_writer = writer;
			_columnDelimiter = string.IsNullOrEmpty(columnDelimiter) ? ' ' : columnDelimiter[0];
			_specialChars = new[] { _CR, _LF, _textQualifier, _columnDelimiter };
			CurrentLineNumber = 1;
		}

		internal List<string> ReadRow()
		{
			List<string> row = null;

			while (true)
			{
				var item = GetNextItem();

				if (item == null)
					break;

				if (row == null)
					row = new List<string>();

				row.Add(item);
			}

			CurrentPosition = _pos;
			return row;
		}

		internal void WriteRow(IEnumerable<string> items, bool quoteAllFields)
		{
			bool isFirstItem = true;

			foreach (string item in items)
			{
				if (!isFirstItem)
					_writer.Write(_columnDelimiter);

				if (quoteAllFields || item.IndexOfAny(_specialChars) >= 0 || string.IsNullOrWhiteSpace(item))
				{
					_writer.Write(_textQualifier);
					_writer.Write(item.Replace(_singleTextQualifierString, _doubleTextQualifierString));
					_writer.Write(_textQualifier);
				}
				else
					_writer.Write(item);

				isFirstItem = false;
			}

			_writer.Write(_LF);
		}

		private string GetNextItem()
		{
			if (_endOfLine)
			{
				_endOfLine = false;
				return null;
			}

			var isQuoted = false;
			var preData = true;
			var postData = false;
			var builder = new StringBuilder();

			while (true)
			{
				char nextChar = GetNextChar(true);

				if (_endOfStream)
				{
					return builder.Length > 0 ? builder.ToString() : null;
				}

				if (!_previousWasCR && (nextChar == _LF))
					CurrentLineNumber++;

				if (nextChar == _CR)
				{
					CurrentLineNumber++;
					_previousWasCR = true;
				}
				else
					_previousWasCR = false;

				if ((postData || !isQuoted) && nextChar == _columnDelimiter)
					return builder.ToString();

				if ((preData || postData || !isQuoted) && (nextChar == _LF || nextChar == _CR))
				{
					_endOfLine = true;

					if (nextChar == _CR && GetNextChar(false) == _LF)
						GetNextChar(true);

					return builder.ToString();
				}

				if (preData && (nextChar == _textQualifier))
				{
					isQuoted = true;
					preData = false;
				}
				else if (preData)
				{
					preData = false;
					builder.Append(nextChar);
				}
				else if (isQuoted && (nextChar == _textQualifier))
				{
					if (GetNextChar(false) == _textQualifier)
						builder.Append(GetNextChar(true));
					else
						postData = true;
				}
				else
					builder.Append(nextChar);
			}
		}

		private char GetNextChar(bool eat)
		{
			if (_pos >= _length)
			{
				_length = _reader.ReadBlock(_buffer, 0, _buffer.Length);

				if (_length == 0)
				{
					_endOfStream = true;
					return default(char);
				}

				_pos = 0;
			}

			return eat ? _buffer[_pos++] : _buffer[_pos];
		}
	}
}
