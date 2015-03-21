namespace VirtoCommerce.Foundation.Frameworks.Email
{
    public interface IEmailService
    {
        bool SendEmail(IEmailMessage message);
    }
}
