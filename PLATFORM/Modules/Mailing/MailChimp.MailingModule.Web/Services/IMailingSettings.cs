namespace MailChimp.MailingModule.Web.Services
{
    public interface IMailingSettings
    {
        string AccessToken { get; }
        string DataCenter { get; }
        string SubscribersListId { get; }
        string Code { get; }
        string Description { get; }
        string LogoUrl { get; }
    }
}
