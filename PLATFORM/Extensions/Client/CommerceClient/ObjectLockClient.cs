using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;

namespace VirtoCommerce.Client
{
    /// <summary>
    /// Class helper user to check if the objects are locked by a user.
    /// </summary>
    public class ObjectLockClient
    {
        private IAppConfigRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectLockClient"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public ObjectLockClient(IAppConfigRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Determines whether the object is locked by the userId passed.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>
        ///   <c>true</c> if [is locked by user] [the specified object id]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">userId</exception>
        public bool IsLockedByUser(string objectId, string objectType, string userId)
        {
            if(String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            string lockedUserId = String.Empty;

            if(IsObjectLocked(objectId, objectType, out lockedUserId))
            {
                return lockedUserId == userId;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the object is locked, if it is locked the userId of the user that has a lock is returned.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>
        ///   <c>true</c> if [is object locked] [the specified object id]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsObjectLocked(string objectId, string objectType, out string userId)
        {
            var currentLock = GetObjectLock(objectType, objectType);

            if (currentLock != null)
            {
                userId = currentLock.UserId;
                return true;
            }

            userId = null;

            return false;
        }

        /// <summary>
        /// Gets the object lock.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">objectId
        /// or
        /// objectType</exception>
        public ObjectLock GetObjectLock(string objectId, string objectType)
        {
            if (String.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException("objectId");
            }

            if (String.IsNullOrEmpty(objectType))
            {
                throw new ArgumentNullException("objectType");
            }

            var currentLock = _repository.ObjectLocks.Where(l => l.ObjectId == objectId && l.ObjectType == objectType).SingleOrDefault();
            return currentLock;
        }

        /// <summary>
        /// Locks the object.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="userId">The user id.</param>
        /// <exception cref="System.ArgumentNullException">
        /// objectId
        /// or
        /// objectType
        /// or
        /// userId
        /// </exception>
        /// <exception cref="System.OperationCanceledException">object already locked</exception>
        public void LockObject(string objectId, string objectType, string userId)
        {
            if (String.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException("objectId");
            }

            if (String.IsNullOrEmpty(objectType))
            {
                throw new ArgumentNullException("objectType");
            }

            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            var activeLock = GetObjectLock(objectId, objectType);

            if (activeLock == null)
            {
                CreateNewLock(objectId, objectType, userId);
            }
            else if (activeLock.UserId == userId)
            {
                RefreshLock(activeLock);
            }
            else
            {
                throw new OperationCanceledException("object already locked");
            }
        }

        /// <summary>
        /// Unlocks the object.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <exception cref="System.ArgumentNullException">
        /// objectId
        /// or
        /// objectType
        /// </exception>
        public void UnlockObject(string objectId, string objectType)
        {
            if (String.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException("objectId");
            }

            if (String.IsNullOrEmpty(objectType))
            {
                throw new ArgumentNullException("objectType");
            }

            var activeLock = GetObjectLock(objectId, objectType);
            if (activeLock != null)
            {
                UnlockObject(activeLock);
            }
        }

        /// <summary>
        /// Refreshes the object lock.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <exception cref="System.ArgumentNullException">
        /// objectId
        /// or
        /// objectType
        /// </exception>
        public void RefreshObjectLock(string objectId, string objectType)
        {
            if (String.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException("objectId");
            }

            if (String.IsNullOrEmpty(objectType))
            {
                throw new ArgumentNullException("objectType");
            }

            var activeLock = GetObjectLock(objectId, objectType);
            if (activeLock != null)
            {
                RefreshLock(activeLock);
            }
        }

        private void RefreshLock(ObjectLock activeLock)
        {
            activeLock.LastModified = DateTime.UtcNow;
            _repository.Update(activeLock);
            _repository.UnitOfWork.Commit();
        }

        private void CreateNewLock(string objectId, string objectType, string userId)
        {
            var newLock = new ObjectLock() { ObjectId = objectId, UserId = userId, ObjectType = objectType };
            _repository.Add(newLock);
            _repository.UnitOfWork.Commit();
        }

        private void UnlockObject(ObjectLock activeLock)
        {
            _repository.Remove(activeLock);
            _repository.UnitOfWork.Commit();
        }
    }
}
