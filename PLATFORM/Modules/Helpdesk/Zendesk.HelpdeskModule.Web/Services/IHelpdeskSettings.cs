
namespace Zendesk.HelpdeskModule.Web.Services
{
    public interface IHelpdeskSettings
    {
        string AccessToken { get; set; }
        string Subdomain { get; }
        string CustomerEmail { get; }
    }
}
