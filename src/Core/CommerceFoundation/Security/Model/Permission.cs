using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Security.Model
{
	[DataContract]
	[EntitySet("Permissions")]
	[DataServiceKey("PermissionId")]
	public class Permission : StorageEntity
	{
		public Permission()
		{
			_PermissionId = GenerateNewKey();
		}

		private string _PermissionId;
		[Key]
		[DataMember]
		[StringLength(128)]
		public string PermissionId
		{
			get
			{
				return _PermissionId;
			}
			set
			{
				SetValue(ref _PermissionId, () => this.PermissionId, value);
			}
		}


		private string _Name;
		[Required, StringLength(256)]
		[DataMember]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				SetValue(ref _Name, () => this.Name, value);
			}
		}

		#region Navigation Properties

		private ObservableCollection<RolePermission> _rolePermissions;

		[DataMember]
		public ObservableCollection<RolePermission> RolePermissions
		{
			get
			{
				if (_rolePermissions == null)
				{
					_rolePermissions = new ObservableCollection<RolePermission>();
				}
				return _rolePermissions;
			}
		}

		#endregion

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			bool retVal = false;
			Permission perm = obj as Permission;
			if (perm != null)
			{
				retVal = Name == perm.Name;
			}
			else if (obj is string)
			{
				string stringPerm = obj as string;
				retVal = Name == Name;
			}
			else
			{
				//default impl
				base.Equals(obj);
			}

			return retVal;
		}

	}
}
