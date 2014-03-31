
namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
	// class for simple data models used for presentation.
	public class KeyValuePair<K, V>
	{
		public K Key { get; set; }
		public V Value { get; set; }
	}

	public class KeyValuePair_string_string : KeyValuePair<string, string> { }


}
