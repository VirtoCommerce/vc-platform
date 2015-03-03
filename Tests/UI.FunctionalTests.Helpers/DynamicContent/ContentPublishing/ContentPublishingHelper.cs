using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace UI.FunctionalTests.Helpers.DynaminContent.ContentPublishing
{
    public class TestContentPublishingBuilder
    {
        private readonly DynamicContentPublishingGroup _contentPublishingGroup;

        private TestContentPublishingBuilder(DynamicContentPublishingGroup contentPublishingGroup)
        {
            _contentPublishingGroup = contentPublishingGroup;
        }


        public static TestContentPublishingBuilder BuildDynamicContentPublishingGroup()
        {
            var contentPublishing = new DynamicContentPublishingGroup()
            {
                Name = "default",
                Description = "default_Description",
                Priority = 5,
                IsActive = true,
                StartDate = DateTime.Now.Date.AddDays(-5),
                EndDate = DateTime.Now.Date.AddDays(5)

            };

            return new TestContentPublishingBuilder(contentPublishing);
        }

        public TestContentPublishingBuilder WithContentItems(DynamicContentItem[] contentItems)
        {
	        foreach (var publishingGroupContentItem in contentItems.Select(dynamicContentItem => new PublishingGroupContentItem
		        {
			        DynamicContentPublishingGroupId = _contentPublishingGroup.DynamicContentPublishingGroupId,
			        DynamicContentItemId = dynamicContentItem.DynamicContentItemId
		        }))
	        {
		        _contentPublishingGroup.ContentItems.Add(publishingGroupContentItem);
	        }

	        return this;
        }

	    public TestContentPublishingBuilder WithContentPlaces(DynamicContentPlace[] contentPlaces)
	    {
		    foreach (var publishingGroupContentPlace in contentPlaces.Select(dynamicContentPlace => new PublishingGroupContentPlace
			    {
				    DynamicContentPublishingGroupId = _contentPublishingGroup.DynamicContentPublishingGroupId,
				    DynamicContentPlaceId = dynamicContentPlace.DynamicContentPlaceId
			    }))
		    {
			    _contentPublishingGroup.ContentPlaces.Add(publishingGroupContentPlace);
		    }

		    return this;
	    }

	    public DynamicContentPublishingGroup GetContentPublishingGroup()
        {
            return _contentPublishingGroup;
        }
    }

    public class TestContentItemsBuilder
    {
        private readonly List<DynamicContentItem> _contentItems;

        private TestContentItemsBuilder(List<DynamicContentItem> contentItems)
        {
            _contentItems = contentItems;
        }


        public static TestContentItemsBuilder BuildsContentItems()
        {
            var contentItem1 = new DynamicContentItem()
            {
                Name = "default_contentItem_1",
                Description = "description_1",
                IsMultilingual = true
            };

            var contentItem2 = new DynamicContentItem()
            {
                Name = "default_contentItem_2",
                Description = "description_2",
                IsMultilingual = false
            };

            var contentItem3 = new DynamicContentItem()
            {
                Name = "default_contentItem_2",
                Description = "description_2",
                IsMultilingual = false
            };

            return new TestContentItemsBuilder(new List<DynamicContentItem>() {contentItem1, contentItem2, contentItem3});
        }


        public List<DynamicContentItem> GetItems()
        {
            return _contentItems;
        }
    }

    public class TestContentPlacesBuilder
    {
        private readonly List<DynamicContentPlace> _contentPlaces;

        private TestContentPlacesBuilder(List<DynamicContentPlace> contentPlaces)
        {
            _contentPlaces = contentPlaces;
        }

		public static TestContentPlacesBuilder BuildContentPlaces(int quantity = 3)
        {
	        var retVal = new List<DynamicContentPlace>();
			for (var i = 0; i < quantity; i++)
	        {
				retVal.Add(new DynamicContentPlace() { Name = "default_contentPlace_" + i, Description = "description_" + i });
	        }

	        return new TestContentPlacesBuilder(retVal);
        }

        public List<DynamicContentPlace> GetPlaces()
        {
            return _contentPlaces;
        }
    }
}
