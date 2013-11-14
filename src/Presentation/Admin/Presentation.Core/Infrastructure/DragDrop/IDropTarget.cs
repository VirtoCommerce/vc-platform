
namespace VirtoCommerce.ManagementClient.Core.Infrastructure.DragDrop
{
    public interface IDropTarget
    {
        void DragOver(DropInfo dropInfo);
        void Drop(DropInfo dropInfo);
    }
}
