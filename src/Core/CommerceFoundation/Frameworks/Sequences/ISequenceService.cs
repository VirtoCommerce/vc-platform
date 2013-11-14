using System.ServiceModel;

namespace VirtoCommerce.Foundation.Frameworks.Sequences
{
    [ServiceContract(Namespace = "http://schemas.virtocommerce.com/1.0/sequences/")]
    public interface ISequenceService
    {
        /// <summary>
        /// Gets the next number of sequence identified by type.
        /// </summary>
		/// <param name="fullTypeName">Full type name of the sequence used as key.</param>
        /// <returns>Unique string</returns>
        [OperationContract]
		string GetNext(string key);

    }
}
