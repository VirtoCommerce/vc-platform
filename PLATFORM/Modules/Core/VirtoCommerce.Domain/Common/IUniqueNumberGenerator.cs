namespace VirtoCommerce.Domain.Common
{
    public interface IUniqueNumberGenerator
    {
        /// <summary>
        /// Generates unique number using given template. The template depends on implementation.
        /// </summary>
        /// <param name="numberTemplate">The number template.</param>
        /// <returns></returns>
        string GenerateNumber(string numberTemplate);
    }
}
