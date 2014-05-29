using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Client;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Frameworks.Extensions
{
	public static class IQueryableExtensions
	{
		public static IQueryable<TEntity> Expand<TEntity>(this IQueryable<TEntity> queryable, string path)
			where TEntity : class
		{
			var objectQuery = queryable as DataServiceQuery<TEntity>;
            if (objectQuery != null) //Is DataServiceQuery
            {
                return ((DataServiceQuery<TEntity>)objectQuery).Expand(path);
            }
            else // probably mock
            {
                path = path.Replace('/', '.');
                queryable = queryable.Include(path);
                return queryable;
            }
		}

		public static IQueryable<TEntity> Expand<TEntity>(this IQueryable<TEntity> queryable, params Expression<Func<TEntity, object>>[] includes)
			where TEntity : class
		{
			if (queryable is DataServiceQuery<TEntity>)
			{
				queryable = includes.Aggregate(queryable,
							  (current, include) => ((DataServiceQuery<TEntity>)current).Expand(include));
			}
			else
			{
				if (includes != null)
				{
					queryable = includes.Aggregate(queryable,
							  (current, include) => current.Include(include));
				}

			}
			return queryable;
		}

		public static IQueryable<TEntity> ExpandAll<TEntity>(this IQueryable<TEntity> queryable)
			where TEntity : class
		{
			var delimiter = queryable is DataServiceQuery<TEntity> ? "/" : ".";
		    var includePath = GetIncludePath(typeof (TEntity), null);
		    if (!string.IsNullOrEmpty(includePath))
		    {
                var parts = includePath.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
		        queryable = parts.Aggregate(queryable, (current, part) => current.Expand(part.Replace("/", delimiter)));
		    }
		    return queryable;
		}

		public static string GetIncludePath(Type type, string prefix)
		{
			string retVal = null;

			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				var propType = propertyInfo.PropertyType;
				//Is collection
				if (propType.IsGenericType 
					&& propType.GetInterface((typeof(ICollection).Name)) != null
					&& !propertyInfo.IsHaveAttribute(typeof(NotMappedAttribute)) )
				{
					var includePath = (prefix == null ? null : prefix + "/") + propertyInfo.Name;
					var collectionItemType = propType.GetGenericArguments().FirstOrDefault();
					if (collectionItemType != null)
					{
                        if (collectionItemType == type)
                            retVal += includePath + ",";
                        else
						    retVal += GetIncludePath(collectionItemType, includePath) + ",";
					}
					else
					{
						retVal += includePath + ",";
					}
				}
                    /*
				else if (propType.IsDerivativeOf(typeof(StorageEntity)))
				{
					retVal += (prefix == null ? null : prefix + "/") + propertyInfo.Name + ",";
				}
                     * */

			}

			return retVal ?? prefix;
		}
	}
}
