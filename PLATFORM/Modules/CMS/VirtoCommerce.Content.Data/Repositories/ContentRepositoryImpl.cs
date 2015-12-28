﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Content.Data.Repositories
{
	public class ContentRepositoryImpl : EFRepositoryBase, IMenuRepository
	{
		public ContentRepositoryImpl()
		{
		}

		public ContentRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<Models.MenuLinkList>().HasKey(x => x.Id)
						.Property(x => x.Id);

			modelBuilder.Entity<Models.MenuLinkList>().ToTable("ContentMenuLinkList");

			modelBuilder.Entity<Models.MenuLink>().HasKey(x => x.Id)
						.Property(x => x.Id);

			modelBuilder.Entity<Models.MenuLink>().HasOptional(m => m.MenuLinkList)
						.WithMany(m => m.MenuLinks)
						.HasForeignKey(m => m.MenuLinkListId).WillCascadeOnDelete(true);

			modelBuilder.Entity<Models.MenuLink>().ToTable("ContentMenuLink");

		}

		public IQueryable<Models.MenuLinkList> MenuLinkLists
		{
			get { return GetAsQueryable<Models.MenuLinkList>(); }
		}

		public IQueryable<Models.MenuLink> MenuLinks
		{
			get { return GetAsQueryable<Models.MenuLink>(); }
		}

        public IEnumerable<MenuLinkList> GetAllLinkLists()
        {
            return MenuLinkLists.Include(s => s.MenuLinks).ToArray();
        }

        public IEnumerable<Models.MenuLinkList> GetListsByStoreId(string storeId)
		{
            return MenuLinkLists.Include(s => s.MenuLinks).Where(s => s.StoreId == storeId);
		}

		public Models.MenuLinkList GetListById(string listId)
		{
            return MenuLinkLists.Include(s => s.MenuLinks).FirstOrDefault(s => s.Id == listId);
		}

		public void UpdateList(Models.MenuLinkList list)
		{
			var existingList = GetListById(list.Id);
			if (existingList != null)
			{
				existingList.Attach(list);
			}
			else
			{
				existingList = list;
			}

			AddOrUpdate(existingList);
			UnitOfWork.Commit();

			var ids = new List<string>();

			foreach (var link in existingList.MenuLinks)
			{
				if (!list.MenuLinks.Any(l => l.Id == link.Id))
				{
					ids.Add(link.Id);
				}
			}

			foreach (var id in ids)
			{
				var link = MenuLinks.First(m => m.Id == id);
				Remove(link);
			}

			UnitOfWork.Commit();
		}

		public void DeleteList(string listId)
		{
			var existingList = GetListById(listId);

			Remove(existingList);
			UnitOfWork.Commit();
		}
	}
}
