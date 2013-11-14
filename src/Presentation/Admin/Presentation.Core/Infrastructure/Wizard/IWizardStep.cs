using System;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard
{
    public interface IWizardStep : IViewModel
    {
        string Comment { get; }

        string Description { get; }

        bool IsValid { get; }

        event EventHandler StepIsValidChangedEvent;

        bool IsLast { get; }
    }
}
