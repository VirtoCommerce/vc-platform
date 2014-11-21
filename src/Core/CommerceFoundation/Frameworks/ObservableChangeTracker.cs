using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.Foundation.Frameworks
{
    internal class Relation
    {
        public object Source { get; set; }
        public string PropertyName { get; set; }
    }
    internal sealed class OneToManyRelation : Relation
    {
        public ICollection Target { get; set; }
    }

    internal sealed class ManyToOneRelation : Relation
    {
        public object Target { get; set; }
    }

    public class ObservableChangeTracker : IDisposable
    {
        private object _lockObject = new object();
        private List<Relation> _relations = new List<Relation>();
        private Dictionary<INotifyCollectionChanged, NotifyCollectionChangedEventHandler> _registeredCollectionHangedEventListener = new Dictionary<INotifyCollectionChanged, NotifyCollectionChangedEventHandler>();
        private Dictionary<INotifyPropertyChanged, PropertyChangedEventHandler> _registeredPropertyChangedEventListener = new Dictionary<INotifyPropertyChanged, PropertyChangedEventHandler>();

        public Func<object, object> KeySelector { get; set; }

        public Action<object> DettachAction { get; set; }
        public Action<object> AttachAction { get; set; }
        public Action<object> UpdateAction { get; set; }
        public Action<object> RemoveAction { get; set; }
        public Action<object> AddAction { get; set; }

        public Action<object, string, object> PropertyChangedAction { get; set; }

        public Action<object, string, object> SetManyToOneRelationAction { get; set; }
        public Action<object, string, object> AddNewOneToManyRelationAction { get; set; }

        public Action<object, string, object> RemoveManyToOneRelationAction { get; set; }
        public Action<object, string, object> RemoveOneToManyRelationAction { get; set; }

        public Action<object, string, object> AttachRelationAction { get; set; }

        public bool SubscribeOnlyMode { get; set; }


        public ObservableChangeTracker()
        {
            //Debug.WriteLine(this.GetType().Name + " " + this.GetHashCode() + " Created");

            //Default key selector
            KeySelector = (x) => x;
        }

        private Dictionary<object, TrackingEntry> _TrackingEntries = new Dictionary<object, TrackingEntry>();
        public ReadOnlyCollection<TrackingEntry> TrackingEntries
        {
            get
            {
                return _TrackingEntries.Values.ToList().AsReadOnly();
            }
        }


        public void MarkAllUnchanged()
        {
            foreach (var entry in TrackingEntries)
            {
                TryChangeState(entry, EntryState.Unchanged);
            }
        }


        public void Attach(object item)
        {
            var storageEntity = item/* as StorageEntity*/;
            InternalAttach(storageEntity);
            TraverseRelation(item, new List<object>(), AttachAction, AttachRelationAction, AttachRelationAction);
        }


        public void Add(object item)
        {
            if (IsAttached(item))
            {
                throw new InvalidOperationException("item already tracked");
            }

            Action<object> addAction = (x) =>
            {
                if (!IsAttached(x))
                {
                    InternalAttach(x, needAttachRelations: false);

                    if (TryChangeState(GetTrackingEntry(x), EntryState.Added))
                    {
                        if (AddAction != null)
                        {
                            AddAction(x);
                        }
                    }
                }
            };

            TraverseRelation(item, new List<object>(), addAction, SetManyToOneRelationAction, AddNewOneToManyRelationAction, false, true);

        }

        public void Remove(object item)
        {
            var manyToOneRelation = _relations.OfType<ManyToOneRelation>().Where(x => x.Source == item);
            Action<object> removeAction = (x) =>
            {
                // if object is just added and needs to be deleted, simply detach it from the state
                var entry = GetTrackingEntry(x);
                if (entry != null && entry.EntryState == EntryState.Added)
                {
                    InternalDettach(x, needDettachRelations: true);
                    if (DettachAction != null)
                    {
                        DettachAction(x);
                    }
                }
                else if (TryChangeState(entry, EntryState.Deleted))
                {
                    if (RemoveAction != null)
                    {
                        RemoveAction(x);
                    }
                }
            };
            TraverseRelation(item, new List<object>(), removeAction,
                             manyToOneRelationAction: null,
                             oneToManyRelationAction: RemoveOneToManyRelationAction,
                             processManyToOneRelations: false,
                             processOneToManyRelations: true);
        }

        public void Update(object item)
        {
            var storageEntity = item as StorageEntity;
            var trackingEntry = GetTrackingEntry(storageEntity);
            if (trackingEntry == null)
            {
                throw new InvalidOperationException("item not tracking");
            }
            Action<object> updateAction = (x) =>
            {
                if (TryChangeState(GetTrackingEntry(x), EntryState.Modified))
                {
                    if (UpdateAction != null)
                    {
                        UpdateAction(x);
                    }
                }
            };
            TraverseRelation(item, new List<object>(), updateAction, null, AddNewOneToManyRelationAction, processManyToOneRelations: false);
        }


        public TrackingEntry GetTrackingEntry(object entity)
        {
            TrackingEntry retVal;
            _TrackingEntries.TryGetValue(KeySelector(entity), out retVal);
            return retVal;

        }

        #region Private methods

        private void InternalAttach(object item, bool needAttachRelations = true)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            Action<object> attachAction = (x) =>
            {
                var trackingEntry = GetTrackingEntry(x);
                if (trackingEntry == null)
                {
                    trackingEntry = new TrackingEntry() { Entity = x, EntryState = EntryState.Unchanged };
                }
                trackingEntry.Entity = x;

                _TrackingEntries[KeySelector(x)] = trackingEntry;

                if (!trackingEntry.IsSubscribed)
                {
                    lock (_lockObject)
                    {
                        if (!trackingEntry.IsSubscribed)
                        {
                            PopulateRelations(x);
                            SubscribeObjectChanges(x);
                            trackingEntry.IsSubscribed = true;
                        }
                    }
                }
            };

            TraverseRelation(item, new List<object>(), attachAction, null, null, needAttachRelations, needAttachRelations);
        }

        private void InternalDettach(object item, bool needDettachRelations = true)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            Action<object> detachAction = (x) =>
            {
                var trackingEntry = GetTrackingEntry(x);
                if (trackingEntry == null)
                {
                    return;
                }

                if (trackingEntry.IsSubscribed)
                {
                    lock (_lockObject)
                    {
                        if (trackingEntry.IsSubscribed)
                        {
                            UnsubscribeObjectChanges(x);
                        }
                    }
                }

                _TrackingEntries.Remove(KeySelector(x));
            };

            TraverseRelation(item, new List<object>(), detachAction, null, null, needDettachRelations, needDettachRelations);
        }


        private bool TryChangeState(TrackingEntry trackingEntity, EntryState newState)
        {
            if (trackingEntity == null)
            {
                throw new ArgumentNullException("trackingEntity");
            }

            var error = String.Format("Unable to change entity state from {0} -> {1}", trackingEntity.EntryState, newState);
            bool throwError = false;
            if (newState == EntryState.Modified)
            {
                switch (trackingEntity.EntryState)
                {
                    case EntryState.Detached:
                    case EntryState.Unchanged:
                        trackingEntity.EntryState = EntryState.Modified;
                        break;
                    case EntryState.Added:
                    case EntryState.Deleted:
                    case EntryState.Modified:
                        break;
                    default:
                        throwError = true;
                        break;
                }
            }
            else if (newState == EntryState.Added)
            {
                switch (trackingEntity.EntryState)
                {
                    case EntryState.Detached:
                    case EntryState.Unchanged:
                        trackingEntity.EntryState = EntryState.Added;
                        break;
                    default:
                        throwError = true;
                        break;
                }
            }
            else if (newState == EntryState.Deleted)
            {
                switch (trackingEntity.EntryState)
                {
                    case EntryState.Unchanged:
                    case EntryState.Modified:
                        trackingEntity.EntryState = EntryState.Deleted;
                        break;
                    default:
                        throwError = true;
                        break;
                }
            }
            else if (newState == EntryState.Unchanged)
            {
                trackingEntity.EntryState = EntryState.Unchanged;
            }

            if (throwError)
            {
                Debug.WriteLine(error);
                //  throw new InvalidOperationException(error);
            }
            return !throwError;
        }


        private void TraverseRelation(object item, List<object> visitHistory, Action<object> entityAction,
                                      Action<object, string, object> manyToOneRelationAction, Action<object, string, object> oneToManyRelationAction,
                                      bool processManyToOneRelations = true, bool processOneToManyRelations = true)
        {
            if (item == null)
            {
                throw new ArgumentNullException("trackingEntry");
            }
            //Prevent circular dependency
            if (visitHistory.Contains(item))
            {
                return;
            }

            visitHistory.Add(item);

            if (entityAction != null)
            {
                entityAction(item);
            }

            var relations = _relations.Where(x => x.Source == item).ToList();

            if (processOneToManyRelations)
            {
                //Recurively change state for all related objects
                foreach (var oneToManyRelation in relations.OfType<OneToManyRelation>())
                {
                    foreach (var childrenEntity in oneToManyRelation.Target)
                    {
                        TraverseRelation(childrenEntity, visitHistory, entityAction, manyToOneRelationAction, oneToManyRelationAction);
                        if (oneToManyRelationAction != null)
                        {
                            oneToManyRelationAction(oneToManyRelation.Source, oneToManyRelation.PropertyName, childrenEntity);
                        }
                    }
                }
            }

            if (processManyToOneRelations)
            {
                foreach (var manyToOneRelation in relations.OfType<ManyToOneRelation>())
                {
                    if (manyToOneRelation.Target != null)
                    {
                        TraverseRelation(manyToOneRelation.Target, visitHistory, entityAction, manyToOneRelationAction, oneToManyRelationAction);
                        if (manyToOneRelationAction != null)
                        {
                            manyToOneRelationAction(manyToOneRelation.Source, manyToOneRelation.PropertyName, manyToOneRelation.Target);
                        }
                    }
                }
            }
        }

        private void PopulateRelations(object item)
        {
            var entityType = item.GetType();

            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            List<Relation> relations = properties.Where(x => x.PropertyType.GetInterface(typeof(INotifyCollectionChanged).Name) != null)
                                                            .Select(x => new OneToManyRelation() { Source = item, PropertyName = x.Name, Target = x.GetValue(item, null) as ICollection }).Cast<Relation>().ToList();
            relations.AddRange(properties.Where(x => x.PropertyType.IsDerivativeOf(typeof(StorageEntity)) && !x.IsHaveAttribute(typeof(DoNotSerializeAttribute)))
                                                            .Select(x => new ManyToOneRelation() { Source = item, PropertyName = x.Name, Target = x.GetValue(item, null) }));

            lock (_lockObject)
            {
                foreach (var relation in relations)
                {
                    RegisterRelation(relation);
                }
            }
        }

        private void SubscribeObjectChanges(object item)
        {
            var oneToManyRelations = _relations.OfType<OneToManyRelation>().Where(x => x.Source == item);
            var manyToOneRelations = _relations.OfType<ManyToOneRelation>().Where(x => x.Source == item);

            //Subscribe to entity property changed
            var notifyPropertyChanged = item as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
            {
                PropertyChangedEventHandler propertyEventHandler = (sender, arg) =>
                {
                    var propertyInfo = sender.GetType().GetProperty(arg.PropertyName);
                    var propertyValue = propertyInfo.GetValue(sender, null);
                    var manyToOneRelation = _relations.OfType<ManyToOneRelation>().FirstOrDefault(x => x.Source == sender && x.PropertyName == arg.PropertyName);
                    //remove old relation if new value other that old value
                    if (manyToOneRelation != null)
                    {
                        string newValueKey = KeySelector(propertyValue) as string;
                        string oldValueKey = KeySelector(manyToOneRelation.Target) as string;
                        if (newValueKey != oldValueKey)
                        {
                            if (RemoveManyToOneRelationAction != null)
                            {
                                RemoveManyToOneRelationAction(manyToOneRelation.Source, manyToOneRelation.PropertyName, manyToOneRelation.Target);
                            }
                            manyToOneRelation.Target = propertyValue;

                            //Add target
                            if (!IsAttached(propertyValue))
                            {
                                Add(propertyValue);
                                //Set relation
                                if (SetManyToOneRelationAction != null)
                                {
                                    SetManyToOneRelationAction(manyToOneRelation.Source, manyToOneRelation.PropertyName, manyToOneRelation.Target);
                                }
                            }
                        }
                    }

                    if (IsAttached(sender) && TryChangeState(GetTrackingEntry(sender), EntryState.Modified))
                    {
                        //Call change property action
                        if (PropertyChangedAction != null)
                        {
                            PropertyChangedAction(item, arg.PropertyName, propertyValue);
                        }
                        if (UpdateAction != null)
                        {
                            UpdateAction(item);
                        }
                    }
                };

                notifyPropertyChanged.PropertyChanged += propertyEventHandler;
                _registeredPropertyChangedEventListener.Add(notifyPropertyChanged, propertyEventHandler);

            }

            //Subscribe one to many relations
            foreach (var relation in oneToManyRelations.ToList())
            {
                var oneToManyRelation = relation;
                NotifyCollectionChangedEventHandler collectionChangedHandler = (sender, args) =>
                {
                    if (args.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (var newItem in args.NewItems)
                        {
                            if (!IsAttached(newItem))
                            {
                                Add(newItem);
                                //Need add oneToMany relation for sender object
                                if (AddNewOneToManyRelationAction != null)
                                {
                                    AddNewOneToManyRelationAction(oneToManyRelation.Source, oneToManyRelation.PropertyName, newItem);
                                }
                            }
                        }
                    }
                    else if (args.Action == NotifyCollectionChangedAction.Remove)
                    {
                        foreach (var oldItem in args.OldItems)
                        {
							//// only remove old item with parent mark
							//if (ParentValidator.IsRemovable(oldItem.GetType()))
							//{
                             Remove(oldItem);
							//}

                            //Only remove relation
                            if (RemoveOneToManyRelationAction != null)
                            {
                                RemoveOneToManyRelationAction(oneToManyRelation.Source, oneToManyRelation.PropertyName, oldItem);
                            }
                        }
                    }
                };

                ((INotifyCollectionChanged)oneToManyRelation.Target).CollectionChanged += collectionChangedHandler;
                _registeredCollectionHangedEventListener.Add((INotifyCollectionChanged)oneToManyRelation.Target, collectionChangedHandler);
            }

        }

        private void UnsubscribeObjectChanges(object item)
        {
            var oneToManyRelations = _relations.OfType<OneToManyRelation>().Where(x => x.Source == item);
            var manyToOneRelations = _relations.OfType<ManyToOneRelation>().Where(x => x.Source == item);

            //Subscribe to entity property changed
            var notifyPropertyChanged = item as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
            {
                _registeredPropertyChangedEventListener.Remove(notifyPropertyChanged);
            }

            //Subscribe one to many relations
            foreach (var relation in oneToManyRelations.ToList())
            {
                var oneToManyRelation = relation;
                _registeredCollectionHangedEventListener.Remove((INotifyCollectionChanged)oneToManyRelation.Target);
            }

        }

        public bool IsAttached(object item)
        {
            var entry = GetTrackingEntry(item);
            return entry != null && entry.EntryState != EntryState.Detached;
        }

        private void RegisterRelation(Relation relation)
        {
            var existRealtion = _relations.FirstOrDefault(x => x.Source == relation.Source && x.PropertyName == relation.PropertyName);

            if (existRealtion == null)
            {
                _relations.Add(relation);
            }

        }


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //Debug.WriteLine(this.GetType().Name + " " + this.GetHashCode() + " Disposed");

            foreach (var pair in _registeredCollectionHangedEventListener)
            {
                pair.Key.CollectionChanged -= pair.Value;
            }
            foreach (var pair in _registeredPropertyChangedEventListener)
            {
                pair.Key.PropertyChanged -= pair.Value;
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
