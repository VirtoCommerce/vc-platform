using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Assets.Model.Exceptions
{
	public enum AssetErrorCode
	{
		None = 0,
		ServiceInternalError = 1,
		ServiceTimeout = 2,
		ServiceIntegrityCheckFailed = 3,
		TransportError = 4,
		ServiceBadResponse = 5,
		ItemNotFound = 6,
		FolderNotFound = 8,
		BlobNotFound = 9,
		AuthenticationFailure = 10,
		AccessDenied = 11,
		ItemAlreadyExists = 12,
		FolderAlreadyExists = 13,
		BlobAlreadyExists = 14,
		BadRequest = 15,
		ConditionFailed = 16,
		BadGateway = 17,
		NotImplemented = 18
	}
}
