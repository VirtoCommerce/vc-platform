using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Content.Menu.Data.Repositories
{
	public class DatabaseMenuRepositoryImpl : EFRepositoryBase, IMenuRepository
	{
		#region

		public DatabaseMenuRepositoryImpl()
		{
			Database.SetInitializer<DatabaseMenuRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		public DatabaseMenuRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, null, interceptors)
		{
			Database.SetInitializer<DatabaseMenuRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			#region MenuLinkList

			modelBuilder.Entity<Models.MenuLinkList>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<Models.MenuLinkList>().ToTable("ContentMenuLinkList");
			
			#endregion

			#region MenuLink

			modelBuilder.Entity<Models.MenuLink>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<Models.MenuLink>().HasOptional(m => m.MenuLinkList)
				.WithMany(m => m.MenuLinks)
				.HasForeignKey(m => m.MenuLinkListId);

			modelBuilder.Entity<Models.MenuLink>().ToTable("ContentMenuLink");

			#endregion
		}

		#endregion

		public IQueryable<Models.MenuLinkList> MenuLinkLists
		{
			get { return GetAsQueryable<Models.MenuLinkList>(); }
		}

		public IQueryable<Models.MenuLink> MenuLinks
		{
			get { return GetAsQueryable<Models.MenuLink>(); }
		}

		public IEnumerable<Models.MenuLinkList> GetListsByStoreId(string storeId)
		{
			return MenuLinkLists.Where(s => s.StoreId == storeId);
		}

		public Models.MenuLinkList GetListById(string listId)
		{
			return MenuLinkLists.Include(s => s.MenuLinks).FirstOrDefault(s => s.Id == listId);
		}

		public void UpdateList(Models.MenuLinkList list)
		{
			AddOrUpdate(list);
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
