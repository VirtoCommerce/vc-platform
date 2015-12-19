using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DotLiquid.ViewEngine.Exceptions
{

    public class SaasCompileException : Exception
    {
        public override string Message
        {
            get
            {
                return base.Message + "\n\r" + this.ToString();
            }
        }

        public string SassLine
        {
            get;
            private set;
        }

        public override string ToString()
        {

            return String.Format("Line: {0}\n\rCompiler error: {1}", SassLine, _innerException != null ? _innerException.Message : "");
        }

        private Exception _innerException;

        public SaasCompileException(string filename, string sass, Exception innerException) : base("Failed to compile sass file \"" + filename + "\"")
        {
            _innerException = innerException;
            if (innerException.Message.StartsWith("stdin"))
            {
                var lineNumber = Int32.Parse(innerException.Message.Split(':')[1]);
                this.SassLine = ReadLine(sass, lineNumber);
            }
        }

        private static string ReadLine(string text, int lineNumber)
        {
            var reader = new StringReader(text);

            string line;
            int currentLineNumber = 0;

            do
            {
                currentLineNumber += 1;
                line = reader.ReadLine();
            }
            while (line != null && currentLineNumber < lineNumber);

            return (currentLineNumber == lineNumber) ? line : string.Empty;
        }
    }


}