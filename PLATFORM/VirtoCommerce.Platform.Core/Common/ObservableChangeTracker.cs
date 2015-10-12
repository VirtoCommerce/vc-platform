using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    public class ObservableChangeTracker : IDisposable
    {
        public Action<object> RemoveAction { get; set; }
        public Action<object> AddAction { get; set; }

     
        public void Attach(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var flatObservableCollections = GetCollectionsRecursive(item);
            foreach (var collection in flatObservableCollections)
            {
                collection.CollectionChanged += (sender, args) =>
                {
                    if (args.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (var newItem in args.NewItems)
                        {
                            if (AddAction != null)
                            {
                                AddAction(newItem);
                            }
                        }
                    }
                    else if (args.Action == NotifyCollectionChangedAction.Remove)
                    {
                        foreach (var oldItem in args.OldItems)
                        {
                            if (RemoveAction != null)
                            {
                                RemoveAction(oldItem);
                            }
                        }
                    }
                };
            }
        }

        public void Dispose()
        {
          
        }


        private static INotifyCollectionChanged[] GetCollectionsRecursive(object obj)
        {
            var retVal = new List<INotifyCollectionChanged>();

            var objectType = obj.GetType();

            var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var collections = properties.Where(x => x.PropertyType.GetInterface(typeof(INotifyCollectionChanged).Name) != null)
                                    .Select(x => (INotifyCollectionChanged)x.GetValue(obj)).ToList();

            foreach (var collection in collections.Cast<IEnumerable>())
            {
                foreach (var objectOfCollection in collection)
                {
                    retVal.AddRange(GetCollectionsRecursive(objectOfCollection));
                }
            }

            retVal.AddRange(collections);
            return retVal.ToArray();


        }
    }
}
