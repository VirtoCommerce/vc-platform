using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.ConfigurationUtility.Main.Models;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ConfigurationUtility.Main.Infrastructure
{
	public class ProjectsRepository : IProjectRepository, IUnitOfWork
	{
		private readonly string _filePath = string.Empty;
		private readonly List<Project> _CachedItems = new List<Project>();
		private bool _isLoaded;

		public ProjectsRepository()
		{
			var tempPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			tempPath = tempPath + @"\VirtoCommerce";
			if (!Directory.Exists(tempPath))
			{
				Directory.CreateDirectory(tempPath);
			}

			_filePath = string.Format(@"{0}\{1}", tempPath, "Projects.xml");
		}

		private void EnsureItems()
		{
			if (_isLoaded)
				return;

			if (File.Exists(_filePath))
			{
				var xml = File.ReadAllText(_filePath);
				var loadedItems = XmlSerializationService<Projects>.Deserialize(xml);

				if (loadedItems != null)
				{
					_CachedItems.AddRange(loadedItems);
				}
			}

			_isLoaded = true;
		}

		private void Add(Project item)
		{
			var existingItem = _CachedItems.SingleOrDefault(x => x.Id == item.Id);
			if (existingItem != null)
			{
				_CachedItems.Remove(existingItem);
			}

			_CachedItems.Add(item);
		}

		private void Remove(Project item)
		{
			var existingItem = _CachedItems.SingleOrDefault(x => x.Id == item.Id);
			if (existingItem != null)
			{
				_CachedItems.Remove(existingItem);
			}
		}

		private void Update(Project item)
		{
			var existingItem = _CachedItems.SingleOrDefault(x => x.Id == item.Id);
			if (existingItem != null)
			{
				_CachedItems.Remove(existingItem);
				_CachedItems.Add(item);
			}
			else
			{
				throw new KeyNotFoundException("item with " + item.Name + " was not found");
			}
		}


		public void Add<T>(T item) where T : class
		{
			Add(item as Project);
		}

		public void Attach<T>(T item) where T : class
		{
		}

		public IQueryable<T> GetAsQueryable<T>() where T : class
		{
			throw new NotImplementedException();
		}

		public void Refresh(IEnumerable collection)
		{
			throw new NotImplementedException();
		}

		public bool IsAttachedTo<T>(T item) where T : class
		{
			return true;
		}

		public void Remove<T>(T item) where T : class
		{
			EnsureItems();
			Remove(item as Project);
		}

		public IUnitOfWork UnitOfWork
		{
			get { return this; }
		}

		public void Update<T>(T item) where T : class
		{
			EnsureItems();
			Update(item as Project);
		}

		public void Dispose()
		{
		}

		public int Commit()
		{
			EnsureItems();

			var saveCollection = new Projects();
			saveCollection.AddRange(_CachedItems);
			var xml = XmlSerializationService<Projects>.Serialize(saveCollection);
			File.WriteAllText(_filePath, xml);

			return 0;
		}

		public void CommitAndRefreshChanges()
		{
			throw new NotImplementedException();
		}

		public void RollbackChanges()
		{
			throw new NotImplementedException();
		}

		public IQueryable<Project> Projects
		{
			get
			{
				EnsureItems();
				return _CachedItems.AsQueryable();
			}
		}
	}
}
