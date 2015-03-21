
namespace VirtoCommerce.Scheduling
{
    public interface IJobActivity
    {
        void Execute(IJobContext context);
    }
}
