namespace VirtoCommerce.Foundation.Data.Azure.CQRS
{
	public enum GetMessageResult
	{
		Success,
		Empty,
		Exception,
		Retry
	}
}
