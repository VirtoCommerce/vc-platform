using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Frameworks
{
    public class ParentValidator
    {
		private static readonly ConcurrentDictionary<Type, List<ParentPropertyDesc>> _parentAttributes = new ConcurrentDictionary<Type, List<ParentPropertyDesc>>();

        public static bool IsRemovable(Type type)
        {
            if (!_parentAttributes.ContainsKey(type))
            {
                var properties = from attributedProperty in type.GetProperties()
                                 select new
                                 {
                                     attributedProperty,
                                     attributes = attributedProperty.GetCustomAttributes(true).Where(attribute => attribute is ParentAttribute),
                                     foreignKeyAttributes = attributedProperty.GetCustomAttributes(true).Where(attribute => attribute is ForeignKeyAttribute)
                                 };

                properties = properties.Where(p => p.attributes.Any()).ToArray();

                var propertyExitsWithAttributes = properties.Any();

                if (!propertyExitsWithAttributes)
                {
                    _parentAttributes.TryAdd(type, null);
                }
                else
                {
					//Support for multiple Parent attributes
	                var desc = properties.Select(p => new ParentPropertyDesc
		                {
			                Name = p.attributedProperty.Name,
			                ForeignKey =
				                p.foreignKeyAttributes.OfType<ForeignKeyAttribute>().Select(fk => fk.Name).FirstOrDefault()
		                }).ToList();

					_parentAttributes.TryAdd(type, desc);
                }
            }

            if (_parentAttributes[type] != null && _parentAttributes[type].Any(p=>!string.IsNullOrEmpty(p.Name)))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Validates the entity.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="type">The type.</param>
        /// <returns>true if entity is removed</returns>
        public static bool ValidateEntity(DbContext context, DbEntityEntry entity, Type type)
        {
            if (entity.State == EntityState.Modified)
            {
                if(IsRemovable(type))
                {
					foreach (var parentAttribute in _parentAttributes[type])
	                {
						//Parent must have ForeignKey
		                if (String.IsNullOrWhiteSpace(parentAttribute.ForeignKey))
		                {
			                continue;
		                }

						//Navigation property must be null
		                if (entity.Reference(parentAttribute.Name).CurrentValue != null)
		                {
			                continue;
		                }
		                var fkProperty = entity.Property(parentAttribute.ForeignKey);

						//ForegnKey must be modified
						if (!fkProperty.IsModified)
						{
							continue;
						}

		                var isFkNullable = ReferenceEquals(fkProperty.CurrentValue, null);
		                if (!isFkNullable)
		                {
			                var fkType = fkProperty.CurrentValue.GetType();
			                isFkNullable = !fkType.IsValueType || Nullable.GetUnderlyingType(fkType) != null;
		                }

		                //ForeignKey must be null if type is nullable 
						//(Thats how EF work when removing item from collection)
						if (isFkNullable && fkProperty.CurrentValue != null)
		                {
			                continue;
		                }

		                context.Set(type).Remove(entity.Entity);
		                return true;
	                }
                }
            }
            return false;
        }

        private class ParentPropertyDesc
        {
            public string Name { get; set; }
            public string ForeignKey { get; set; }
        }
    }
}
