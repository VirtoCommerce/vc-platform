using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	[DataContract]
	[EntitySet("ObjectLocks")]
	[DataServiceKey("ObjectLockId")]
	public class ObjectLock : StorageEntity
	{
		public ObjectLock()
		{
		}

		private string _ObjectType;
		[DataMember]
		[StringLength(32)]
		[Required]
		[Key]
        [Column(Order=0)]
		public string ObjectType
		{
			get { return _ObjectType; }
			set { SetValue(ref _ObjectType, () => ObjectType, value); }
		}

		private string _ObjectId;
		[DataMember]
		[StringLength(64)]
		[Required]
		[Key]
        [Column(Order = 1)]
		public string ObjectId
		{
			get { return _ObjectId; }
			set { SetValue(ref _ObjectId, () => ObjectId, value); }
		}

		private string _UserId;
		[DataMember]
		[StringLength(64)]
		[Required]
		public string UserId
		{
			get { return _UserId; }
			set { SetValue(ref _UserId, () => UserId, value); }
		}
	}
}
