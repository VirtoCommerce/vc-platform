using System.Collections.Generic;

namespace Zendesk.HelpdeskModule.Web.Services
{
    interface IHelpdeskManager
    {
        void RegisterHelpdesk(IHelpdesk helpdeskModule);
        IEnumerable<IHelpdesk> Helpdesks { get; }
    }
}
