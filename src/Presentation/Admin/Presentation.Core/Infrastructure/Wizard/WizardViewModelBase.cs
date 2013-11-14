using System;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard
{
    public abstract class WizardViewModelBase : WizardViewModelBare, IWizardStep
    {
        #region IWizardStep Members

        public virtual string Comment
        {
            get { return string.Empty; }
        }

        public abstract string Description { get; }

        public abstract bool IsValid { get; }

        public abstract bool IsLast { get; }

        // derived classes should implement IWizardStep and use this member
        public event EventHandler StepIsValidChangedEvent;

        protected virtual void OnIsValidChanged()
        {
            var tmp = StepIsValidChangedEvent;
            if (tmp != null)
            {
                tmp(this, null);
            }
        }

        #endregion
    }
}
