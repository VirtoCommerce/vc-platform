using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// Helper class used for resolving model object primary keys when it persisted in persistent infrastructure
    /// Used in model to db model converters
    /// </summary>
    public class PrimaryKeyResolvingMap
    {
        private readonly Dictionary<IEntity, IEntity> _resolvingMap = new Dictionary<IEntity, IEntity>();
        private readonly List<(IEntity Model, AuditableEntity SavedEntity)> _savedPairs = new List<(IEntity Model, AuditableEntity SavedEntity)>();

        public void AddPair(IEntity transientEntity, IEntity persistentEntity)
        {
            _resolvingMap[transientEntity] = persistentEntity;
        }

        /// <summary>
        /// Registers an updated model with the entity that was saved for it (the tracked one, not the FromModel copy),
        /// so that ResolvePrimaryKeys syncs the values the save generated to the model.
        /// </summary>
        public void AddSavedPair(IEntity model, AuditableEntity savedEntity)
        {
            _savedPairs.Add((model, savedEntity));
        }

        public void ResolvePrimaryKeys()
        {
            foreach (var pair in _resolvingMap)
            {
                if (string.IsNullOrEmpty(pair.Key.Id) && !string.IsNullOrEmpty(pair.Value.Id))
                {
                    pair.Key.Id = pair.Value.Id;

                    if (pair.Value is AuditableEntity persistentEntity)
                    {
                        persistentEntity.SyncGeneratedValues(pair.Key);
                    }
                }
            }

            foreach (var (model, savedEntity) in _savedPairs)
            {
                savedEntity.SyncGeneratedValues(model);
            }
        }
    }
}
