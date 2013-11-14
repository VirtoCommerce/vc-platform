#region Usings

using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

#endregion

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs.DialogWithoutDefaultButtons
{
    public abstract class InteractionDialogActionBase : TriggerAction<FrameworkElement>
    {
        #region TriggerAction Overrides
        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;

            if (args == null)
            {
                return;
            }

            var interactionDialog = GetInteractionDialog(args.Context);
            var callback = args.Callback;

            EventHandler handler = null;
            
            handler = (o, e) =>
            {
                interactionDialog.Closed -= handler;
                callback();
            };

            interactionDialog.Closed += handler;

            interactionDialog.ShowDialog();
        }
        #endregion

        #region Overridables
        protected abstract Window GetInteractionDialog(Notification notification);
        #endregion
    }
}
