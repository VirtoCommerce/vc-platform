using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notifications
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
