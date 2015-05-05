using System;
using System.Collections.Generic;
using System.Linq;

namespace Zendesk.HelpdeskModule.Web.Services
{
    public class InMemoryHelpdeskManager: IHelpdeskManager
    {
        private readonly List<IHelpdesk> _helpdesks = new List<IHelpdesk>();

        #region IHelpdeskManager Members

        public void RegisterHelpdesk(IHelpdesk helpdesk)
        {
            if (helpdesk == null)
            {
                throw new ArgumentNullException("helpdesk");
            }
            if (_helpdesks.Any(x => x.Code == helpdesk.Code))
            {
                throw new OperationCanceledException(helpdesk.Code + " already registered");
            }
            _helpdesks.Add(helpdesk);
        }

        public IEnumerable<IHelpdesk> Helpdesks
        {
            get { return _helpdesks.AsReadOnly(); }
        }

        #endregion
    }
}