using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Data.Caching
{
	/// <summary>
	/// Thread safe class to manipulate cache locks.
	/// </summary>
	internal class CacheEntries
	{
		private static readonly Dictionary<string, CacheEntry> _Entries;
		private static readonly ReaderWriterLockSlim _Lock;

		/// <summary>
		/// Initializes the <see cref="CacheEntries"/> class.
		/// </summary>
		static CacheEntries()
		{
			_Lock = new ReaderWriterLockSlim();
			_Entries = new Dictionary<string, CacheEntry>();
		}

		/// <summary>
		/// Determines whether the specified key contains key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		/// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
		/// </returns>
		public static bool ContainsKey(string key)
		{
			return _Entries.ContainsKey(key);
		}

		/// <summary>
		/// Gets the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static CacheEntry Get(string key)
		{
			_Lock.EnterReadLock();
			try
			{
				if (ContainsKey(key))
				{
					return _Entries[key];
				}
			}
			finally
			{
				_Lock.ExitReadLock();
			}

			return null;
		}

		/// <summary>
		/// Gets the lock.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static object GetLock(string key)
		{
			CacheEntry entry = null;
			_Lock.EnterUpgradeableReadLock();
			try
			{
				if (ContainsKey(key))
				{
					entry = _Entries[key];
				}
				else
				{
					entry = new CacheEntry();

					_Lock.EnterWriteLock();
					try
					{
						_Entries.Add(key, entry);
					}
					finally
					{
						_Lock.ExitWriteLock();
					}
				}
			}
			finally
			{
				_Lock.ExitUpgradeableReadLock();
			}

			return entry.Lock;
		}

		/// <summary>
		/// Removes the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		public static void Remove(string key)
		{
			_Lock.EnterUpgradeableReadLock();
			try
			{
				if (ContainsKey(key))
				{
					_Lock.EnterWriteLock();
					try
					{
						_Entries.Remove(key);
					}
					finally
					{
						_Lock.ExitWriteLock();
					}
				}
			}
			finally
			{
				_Lock.ExitUpgradeableReadLock();
			}
		}
	}
}
