using System.Linq;
using System.Windows;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DragDrop.Utilities;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.DragDrop
{
    public class DefaultDragHandler : IDragSource
    {
        public virtual void StartDrag(DragInfo dragInfo)
        {
            int itemCount = dragInfo.SourceItems.Cast<object>().Count();

            if (itemCount == 1)
            {
                dragInfo.Data = dragInfo.SourceItems.Cast<object>().First();
            }
            else if (itemCount > 1)
            {
                dragInfo.Data = TypeUtilities.CreateDynamicallyTypedList(dragInfo.SourceItems);
            }

            dragInfo.Effects = (dragInfo.Data != null) ? 
                DragDropEffects.Copy | DragDropEffects.Move : 
                DragDropEffects.None;
        }
    }
}
