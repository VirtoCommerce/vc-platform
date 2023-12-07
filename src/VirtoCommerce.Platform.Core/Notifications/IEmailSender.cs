using System;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notifications
{
    [Obsolete("Use notification module instead")]
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
