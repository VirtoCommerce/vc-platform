using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Services.Client;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Services;

namespace VirtoCommerce.Foundation.Data
{
	public abstract class DSClientBase : DataServiceContext, IRepository, IUnitOfWork
	{
		readonly IFactory _entityFactory;
		private readonly ISecurityTokenInjector _securityTokenInjector;
		public ObservableChangeTracker ChangeTracker { get; private set; }

		public DSClientBase(string relativeUri, IFactory factory, ISecurityTokenInjector securityTokenInjector)
			: this(new Uri(relativeUri), factory, securityTokenInjector)
		{
		}

		public DSClientBase(Uri serviceUri, IFactory factory, ISecurityTokenInjector securityTokenInjector)
			: base(serviceUri, DataServiceProtocolVersion.V3)
		{
			_securityTokenInjector = securityTokenInjector;

			this.MergeOption = MergeOption.PreserveChanges;
			this.IgnoreResourceNotFoundException = true;

			_entityFactory = factory;

			ResolveName = (clientType =>
			{
				clientType = _entityFactory.GetBaseType(clientType);
				return string.Concat(this.GetType().Namespace + ".", clientType.Name);
				//return string.Concat(clientType.Namespace + ".", clientType.Name);
			});

			ResolveType = (entitySetName =>
			{
				return _entityFactory.GetEntityTypeByStringName(entitySetName.Split('.').Last());
			});

			IgnoreResourceNotFoundException = true;
			WritingEntity += this.DSCatalogClient_WritingEntity;
			SendingRequest += DSClientBase_SendingRequest;
			ReadingEntity += this.DSCatalogClient_ReadingEntity;

			ChangeTracker = CreateChangeTracker();
		}

		#region IRepository Members

		public IUnitOfWork UnitOfWork
		{
			get { return this; }
		}

		public void Add<T>(T item) where T : class
		{
			ChangeTracker.Add(item);
		}

		public void Remove<T>(T item) where T : class
		{
			ChangeTracker.Remove(item);
		}

		public void Update<T>(T item) where T : class
		{
			if (!ChangeTracker.IsAttached(item)) // consistent implementation with a repository class
			{
				ChangeTracker.Attach(item);
			}
			ChangeTracker.Update(item);
		}

		public void Attach<T>(T item) where T : class
		{
			if (ChangeTracker.IsAttached(item))
			{
#if DEBUG
				Debug.WriteLine("Object {0} already attached", item.ToString());
#endif
				return;
			}
			ChangeTracker.Attach(item);
		}

		public bool IsAttachedTo<T>(T item) where T : class
		{
			return ChangeTracker.IsAttached(item);
		}

		public IQueryable<T> GetAsQueryable<T>() where T : class
		{
			return CreateQuery<T>(ResolveEntitySetName(typeof(T)));
		}

	    public void Refresh(IEnumerable collection)
	    {
	        throw new NotImplementedException();
	    }

	    #endregion

		#region IUnitOfWork Members

		public int Commit()
		{
			var currentTime = DateTime.UtcNow;
			foreach (var entry in this.Entities.Where(x => x.State != EntityStates.Unchanged))
			{
				if (entry.Entity is IModifiedDateTimeFields)
				{
					var entity = entry.Entity as IModifiedDateTimeFields;

					if (entry.State == EntityStates.Added)
					{
						entity.Created = currentTime;
					}
					else if (entry.State == EntityStates.Modified)
					{
						//ParentValidator.ValidateEntity(this, entity, entry.Entity.GetType());
					}

					entity.LastModified = currentTime;
				}
			}


			// turn tracking off, since we don't need to reload entities
			var previousMergeOption = MergeOption;
			MergeOption = MergeOption.NoTracking;

			// save changes
			var count = SaveChanges(SaveChangesOptions.Batch).Count();

			//restore previous merge option
			MergeOption = previousMergeOption;

			ChangeTracker.MarkAllUnchanged();
			return count;
		}

		public void CommitAndRefreshChanges()
		{
			Commit();

			ChangeTracker.Dispose();

			ChangeTracker = CreateChangeTracker();
		}

		public void RollbackChanges()
		{
			throw new NotImplementedException();
		}
		#endregion

		void DSCatalogClient_ReadingEntity(object sender, ReadingWritingEntityEventArgs e)
		{
			if (MergeOption != MergeOption.NoTracking)
			{
				ChangeTracker.Attach(e.Entity);
			}
		}

		void DSClientBase_SendingRequest(object sender, SendingRequestEventArgs e)
		{
			if (_securityTokenInjector != null)
			{
				_securityTokenInjector.InjectToken(e.RequestHeaders);
			}
		}

		void DSCatalogClient_WritingEntity(object sender, ReadingWritingEntityEventArgs e)
		{
			// e.Data gives you the XElement for the Serialization of the Entity
			//Using XLinq , you can add/Remove properties to the element Payload
			var xnEntityProperties = XName.Get("properties", e.Data.GetNamespaceOfPrefix("m").NamespaceName);
			XElement xePayload = null;
			foreach (var property in e.Entity.GetType().GetProperties())
			{
				var doNotSerializeAttributes = property.GetCustomAttributes(typeof(DoNotSerializeAttribute), false);
				if (doNotSerializeAttributes.Length > 0)
				{
					if (xePayload == null)
					{
						xePayload = e.Data.Descendants().Where<XElement>(xe => xe.Name == xnEntityProperties).First<XElement>();
					}
					//The XName of the property we are going to remove from the payload
					var xnProperty = XName.Get(property.Name, e.Data.GetNamespaceOfPrefix("d").NamespaceName);
					//Get the Property of the entity you don't want sent to the server
					foreach (var xeRemoveThisProperty in xePayload.Descendants(xnProperty).ToList())
					{
						//Remove this property from the Payload sent to the server
						xeRemoveThisProperty.Remove();
					}
				}
			}
		}

		private ObservableChangeTracker CreateChangeTracker()
		{
			var retVal = new ObservableChangeTracker();

			retVal.KeySelector = (x) =>
			{
				string key = null;
				if (x != null)
				{
					//old version
					var propInfo = x.GetType().FindPropertiesWithAttribute(typeof(KeyAttribute)).First();
					key = String.Format("{0}-{1}", x.GetType(), propInfo.GetValue(x, null));

					//var propInfo = x.GetType().FindPropertiesWithAttribute(typeof (KeyAttribute)).FirstOrDefault();
					//if (propInfo != null)
					//{
					//    key = String.Format("{0}-{1}", x.GetType(), propInfo.GetValue(x, null));
					//}
				}
				return key;
			};

			retVal.AddAction = (x) =>
			{
				var name = ResolveEntitySetName(x);
				AddObject(name, x);
			};

			retVal.AttachAction = (x) =>
			{
				if (GetEntityDescriptor(x) == null)
				{
					var name = ResolveEntitySetName(x);
					AttachTo(name, x);
				}
			};

			retVal.DettachAction = (x) =>
			{
				if (GetEntityDescriptor(x) != null)
				{
					Detach(x);
				}
			};


			retVal.RemoveAction = (x) =>
			{
				DeleteObject(x);
			};

			retVal.UpdateAction = (x) =>
			{
				if (GetEntityDescriptor(x) != null)
				{
					UpdateObject(x);
				}
			};

			retVal.AttachRelationAction = (source, property, target) =>
				AttachRelation(source, property, target);
			retVal.AddNewOneToManyRelationAction = (source, property, target) =>
				AddNewOneToManyRelation(source, property, target);
			retVal.SetManyToOneRelationAction = (source, property, target) =>
			{
				this.SetLink(source, property, target);
			};

			retVal.RemoveManyToOneRelationAction = (source, property, target) =>
			{
				SetLink(source, property, null);
			};
			retVal.RemoveOneToManyRelationAction = (source, property, target) =>
			{
				// this code generates a lot of deletes which should be handled by the cascade deletes instead
				//DeleteLink(source, property, target);

				// mark target object updated, so we can send it to the server and delete it if it can't exist without parent
				//Update(target);
			};

			return retVal;
		}

		private void AttachRelation(object source, string property, object target)
		{
			if (!Links.Any(x => x.Source == source && x.SourceProperty == property && x.Target == target))
			{
				this.AttachLink(source, property, target);
			}
		}

		private void AddNewOneToManyRelation(object source, string property, object target)
		{
			if (!Links.Any(x => x.Source == source && x.SourceProperty == property && x.Target == target))
			{
				this.AddLink(source, property, target);
			}
		}

		private string ResolveEntitySetName(object item)
		{
			return ResolveEntitySetName(item.GetType());
		}

		private static string ResolveEntitySetName(PropertyInfo propertyInfo)
		{
			return ResolveEntitySetName(propertyInfo.PropertyType);
		}

		private static string ResolveEntitySetName(Type type)
		{
			var entitySetAttribute = (EntitySetAttribute)type.GetCustomAttributes(typeof(EntitySetAttribute), true).FirstOrDefault();

			if (entitySetAttribute != null)
				return entitySetAttribute.EntitySet;
			else
			{
				if (type.IsGenericType)
				{
					return ResolveEntitySetName(type.GetGenericArguments()[0]);
				}

			}

			return null;
		}

		private static Uri GetConnectionString(string baseUriName, string relativeUri)
		{
			if (ConfigurationManager.ConnectionStrings[baseUriName] != null)
			{
				var connection = ConfigurationManager.ConnectionStrings[baseUriName].ConnectionString;
				return new Uri(new Uri(connection), relativeUri);
			}
			else if (baseUriName.StartsWith("http://") || baseUriName.StartsWith("https://"))
			{
				return new Uri(new Uri(baseUriName), relativeUri);
			}

			return new Uri(relativeUri);
		}

		#region IDisposable Members

		public void Dispose()
		{
			//System.Diagnostics.Debug.WriteLine(this.GetType().Name + " " + this.GetHashCode() + " Disposed");

			Dispose(true);

			GC.SuppressFinalize(this);
		}

		private bool _disposed;
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this._disposed)
			{
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{
					ChangeTracker.Dispose();
				}

				// Call the appropriate methods to clean up
				// unmanaged resources here.
				// If disposing is false,
				// only the following code is executed.

				// Note disposing has been done.
				_disposed = true;

			}
		}
		#endregion
	}
}

