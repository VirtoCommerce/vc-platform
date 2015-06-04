using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Frameworks.CQRS.Events;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.PowerShell.Cmdlet;

namespace VirtoCommerce.PowerShell.SearchSetup
{
	[CLSCompliant(false)]
    public class ProgressObserver : ISystemObserver
    {
        private readonly DomainCommand _command;
        private readonly ProgressRecord _progress;

        public ProgressObserver(DomainCommand command, ProgressRecord progress)
        {
            _command = command;
            _progress = progress;
        }

        public void Notify(ISystemEvent systemEvent)
        {
            if (systemEvent is ConsumeEnd)
            {
                var consume = systemEvent as ConsumeEnd;

                if (consume.Message is SearchIndexMessage)
                {
                    var msg = consume.Message as SearchIndexMessage;
                    var displayMessage = String.Format(
                        "Processing document {0} of {1} - {2}%",
                        msg.Partition.Start,
                        msg.Partition.Total,
                        (int)(msg.Partition.Start * 100 / msg.Partition.Total));
                    _progress.StatusDescription = displayMessage;
                    _progress.PercentComplete = (int)(msg.Partition.Start * 100 / msg.Partition.Total);

                    _command.SafeWriteProgress(_progress);
                }
            }
        }
    }
}
