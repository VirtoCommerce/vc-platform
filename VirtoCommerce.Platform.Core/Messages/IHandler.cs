using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Messages
{
    public interface IHandler<in T> where T : IMessage
    {
        Task Handle(T message);
    }
}
