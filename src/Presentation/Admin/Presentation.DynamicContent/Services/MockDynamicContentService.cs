using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Model;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.ManagementClient.DynamicContent.Services
{
	public class MockDynamicContentService: IDynamicContentRepository 
	{
		private IList[] MockLists;

		private List<DynamicContentPlace> ContentPlaceList = new List<DynamicContentPlace>();
		private List<DynamicContentItem> ContentItemList = new List<DynamicContentItem>();
		private List<DynamicContentPublishingGroup> ContentPublishingGroupList = new List<DynamicContentPublishingGroup>();


		public MockDynamicContentService()
		{
			ContentPlaceList.Add(new DynamicContentPlace { Name = "Main CP", Description = "main content place description"});
			ContentPlaceList.Add(new DynamicContentPlace { Name = "Header CP", Description = "header content place description"});

			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "1", Description = "first dynamic content item", ContentTypeId = DynamicContentType.ImageNonClickable.ToString(), IsMultilingual = false, Name = "first", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "2", Description = "sec dynamic content item", ContentTypeId = DynamicContentType.Html.ToString(), IsMultilingual = false, Name = "first1", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "3", Description = "third dynamic content item", ContentTypeId = DynamicContentType.ImageClickable.ToString(), IsMultilingual = false, Name = "first2", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "4", Description = "fourth dynamic content item", ContentTypeId = DynamicContentType.Flash.ToString(), IsMultilingual = false, Name = "first3", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "5", Description = "fifth dynamic content item", ContentTypeId = DynamicContentType.CategoryWithImage.ToString(), IsMultilingual = false, Name = "first4", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "6", Description = "six dynamic content item", ContentTypeId = DynamicContentType.ProductWithImageAndPrice.ToString(), IsMultilingual = false, Name = "first5", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "7", Description = "dynamic content item", ContentTypeId = DynamicContentType.ImageNonClickable.ToString(), IsMultilingual = false, Name = "first6", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "8", Description = "dynamic content item", ContentTypeId = DynamicContentType.ImageNonClickable.ToString(), IsMultilingual = false, Name = "first7", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "9", Description = "dynamic content item", ContentTypeId = DynamicContentType.Html.ToString(), IsMultilingual = false, Name = "first8", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "10", Description = "dynamic content item", ContentTypeId = DynamicContentType.ImageNonClickable.ToString(), IsMultilingual = false, Name = "first9", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "11", Description = "dynamic content item", ContentTypeId = DynamicContentType.Flash.ToString(), IsMultilingual = false, Name = "first10", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "12", Description = "dynamic content item", ContentTypeId = DynamicContentType.ImageNonClickable.ToString(), IsMultilingual = false, Name = "first11", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "13", Description = "dynamic content item", ContentTypeId = DynamicContentType.CategoryWithImage.ToString(), IsMultilingual = false, Name = "first12", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "14", Description = "dynamic content item", ContentTypeId = DynamicContentType.ImageNonClickable.ToString(), IsMultilingual = false, Name = "first13", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "15", Description = "dynamic content item", ContentTypeId = DynamicContentType.ProductWithImageAndPrice.ToString(), IsMultilingual = false, Name = "first14", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "16", Description = "dynamic content item", ContentTypeId = DynamicContentType.ImageNonClickable.ToString(), IsMultilingual = false, Name = "first15", Created = DateTime.Now });
			ContentItemList.Add(new DynamicContentItem { DynamicContentItemId = "17", Description = "second dynamic content item", ContentTypeId = DynamicContentType.Html.ToString(), IsMultilingual = false, Name = "second", Created = DateTime.Now });
			var props = new ObservableCollection<DynamicContentItemProperty>();
			props.Add(new DynamicContentItemProperty{ Name="Path to Image file", ShortTextValue="http://testservice.org/imgs/img1.jpg", ValueType=2, Created=DateTime.Today });
			props.Add(new DynamicContentItemProperty { Name = "Alternative text", LongTextValue = "If you see this, please call 8-800-12345", ValueType=0, Created=DateTime.Today });
			ContentItemList[0].PropertyValues.Add(props);
			ContentItemList[6].PropertyValues.Add(props);
			ContentItemList[7].PropertyValues.Add(props);
			ContentItemList[9].PropertyValues.Add(props);
			ContentItemList[11].PropertyValues.Add(props);
			ContentItemList[13].PropertyValues.Add(props);
			ContentItemList[15].PropertyValues.Add(props);

			ContentPublishingGroupList.Add(new DynamicContentPublishingGroup { Name = "Content publishing group", Description = "First Content publishing group", StartDate=DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(5), IsActive = true, Priority= 100 });
			//var contentExpression = 
			//ContentPublishingGroupList[0].Items.Add(ContentItemList[0]);
			//ContentPublishingGroupList[0].Places.Add(ContentPlaceList[0]);

			MockLists = new IList[] { ContentPlaceList, ContentItemList, ContentPublishingGroupList };
		}

		#region IDynamicContentRepository

		public IQueryable<DynamicContentPlace> Places
		{
			get { return ContentPlaceList.AsQueryable(); }
		}
		
		public IQueryable<DynamicContentItem> Items 
		{
			get { return ContentItemList.AsQueryable(); }
		}

		public IQueryable<DynamicContentPublishingGroup> PublishingGroups 
		{ 
			get {return ContentPublishingGroupList.AsQueryable();}
		}

		public IQueryable<PublishingGroupContentItem> PublishingGroupContentItems
		{
			get { return GetAsQueryable<PublishingGroupContentItem>(); }
		}

		public IQueryable<PublishingGroupContentPlace> PublishingGroupContentPlaces
		{
			get { return GetAsQueryable<PublishingGroupContentPlace>(); }
		}

		#endregion

		#region IRepository Members

		MockUnitOfWork MockUnitOfWorkItem = new MockUnitOfWork();
		public IUnitOfWork UnitOfWork
		{
			get { return MockUnitOfWorkItem; }
		}

		public void Attach<T>(T item) where T : class
		{
		}

        public bool IsAttachedTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }


		public void Add<T>(T item) where T : class
		{
			var list = MockLists.OfType<List<T>>().First();

			if (!list.Contains(item))
				list.Add(item);
		}

		public void Update<T>(T item) where T : class
		{
		}

		public void Remove<T>(T item) where T : class
		{
			var list = MockLists.OfType<List<T>>().First();
			list.Remove(item);
		}

		public IQueryable<T> GetAsQueryable<T>() where T : class
		{
			throw new NotImplementedException();
		}

	    public void Refresh(IEnumerable collection)
	    {
	        throw new NotImplementedException();
	    }

	    #endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion

	}

	public class MockUnitOfWork : IUnitOfWork
	{
		public int Commit()
		{
			return 0;
		}

		public void CommitAndRefreshChanges()
		{
		}

		public void RollbackChanges()
		{
		}
	}

}
