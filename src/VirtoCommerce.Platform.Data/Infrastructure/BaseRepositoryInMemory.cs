using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public abstract class BaseRepositoryInMemory : IRepository
    {
        public BaseRepositoryInMemory()
        {
            UnitOfWork = new InMemoryUnitOfWork();
        }

        static class Cache<T> where T : class
        {
            public static InMemoryAsyncEnumerable<T> Values;
        }

        protected static IEnumerable<T> Get<T>() where T : class
        {
            return Cache<T>.Values;
        }

        protected static IList<T> GetList<T>() where T : class
        {
            return Cache<T>.Values.ToList();
        }

        protected static void Set<T>(IEnumerable<T> items) where T : class
        {
            Cache<T>.Values = (InMemoryAsyncEnumerable<T>)items;
        }
        

        public IUnitOfWork UnitOfWork { get; set; }

        public void Add<T>(T item) where T : class
        {
            GetList<T>().Add(item);
        }

        public void Attach<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //Nothing
        }
        
        public void Remove<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
