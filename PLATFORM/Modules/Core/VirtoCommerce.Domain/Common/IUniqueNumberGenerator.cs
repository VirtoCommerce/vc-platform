namespace VirtoCommerce.Domain.Common
{
    public interface IUniqueNumberGenerator
    {
        string GenerateNumber(string objectTypeName);
        string GenerateNumber(string objectTypeName, string numberTemplate, string dateFormat, int sequenceReservationRange, int counterLength);
    }
}
