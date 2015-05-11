
namespace Zendesk.HelpdeskModule.Web.Services
{
    public interface IHelpdesk
    {
        string AccessToken { get; }
        string Subdomain { get; }
        string Code { get; }
        string Description { get; }
        string LogoUrl { get; }
    }
}
