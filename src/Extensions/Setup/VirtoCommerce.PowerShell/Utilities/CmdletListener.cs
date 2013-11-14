using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.PowerShell.Cmdlet;

namespace VirtoCommerce.PowerShell.Utilities
{
	[CLSCompliant(false)]
    public class CmdletListener : TextWriterTraceListener
    {
        private readonly DomainCommand _command;
        private readonly ProgressRecord _progress;
        //private readonly MemoryStream _MemoryStream = new MemoryStream();

        public CmdletListener(DomainCommand command, ProgressRecord progress)
        {
            //_MemoryStream
            //base.Writer = new StreamWriter();
            _command = command;
            _progress = progress;
        }

        public override void WriteLine(string message)
        {
            _progress.StatusDescription = message;
            _command.WriteProgress(_progress);
            base.WriteLine(message);
        }

        public override void Close()
        {
            base.Close();
        }
    }


    /*
    public class CmdletStreamWriter : TextWriter
    {
        private readonly DomainCommand _command;
        private readonly ProgressRecord _progress;

        public CmdletStreamWriter(DomainCommand command, ProgressRecord progress, Stream stream)
        {
            _command = command;
            _progress = progress;
            
        }

        public override void Flush()
        {
            _progress.StatusDescription = message;
            _command.WriteProgress(_progress);

            base.Flush();
        }

        public override void WriteLine(string message)
        {
            base.WriteLine(message);
        }

        public override Encoding Encoding
        {
            get { new UTF8Encoding(false); }
        }
    }
     * */
}
