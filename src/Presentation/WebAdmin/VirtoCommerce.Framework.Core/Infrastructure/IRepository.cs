using System;
using System.Linq;

namespace VirtoCommerce.Framework.Core.Infrastructure
{
	public interface IRepository : IDisposable
	{
		/// Get the unit of work in this repository
		/// </summary>
		IUnitOfWork UnitOfWork { get; }

		void Attach<T>(T item) where T : class;

		void Add<T>(T item) where T : class;

		void Update<T>(T item) where T : class;

		void Remove<T>(T item) where T : class;

		IQueryable<T> GetAsQueryable<T>() where T : class;
	}
}
