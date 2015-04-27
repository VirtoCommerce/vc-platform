using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Content.Data
{
	public class SqlPagesDatabaseInitializer : SetupDatabaseInitializer<DatabasePagesRepositoryImpl, VirtoCommerce.Content.Pages.Data.Migrations.Configuration>
	{
		protected override void Seed(DatabasePagesRepositoryImpl repository)
		{
			base.Seed(repository);

			CreateDefaultPages(repository, "AppleStore");
			CreateDefaultPages(repository, "SampleStore");
			CreateDefaultPages(repository, "SonyStore");
		}

		public void CreateDefaultPages(DatabasePagesRepositoryImpl repository, string storeId)
		{
			var builder = new StringBuilder();
			builder.AppendLine("<p>A great About Us page helps builds trust between you and your customers. The more content you provide about you and your business, the more confident people will be when purchasing from your store.</p>");
			builder.AppendLine("<p>Your About Us page might include:</p>");
			builder.AppendLine("<ul>");
			builder.AppendLine("<li>Who you are</li>");
			builder.AppendLine("<li>Why you sell the items you sell</li>");
			builder.AppendLine("<li>Where you are located</li>");
			builder.AppendLine("<li>How long you have been in business</li>");
			builder.AppendLine("<li>How long you have been running your online shop</li>");
			builder.AppendLine("<li>Who are the people on your team</li>");
			builder.AppendLine("<li>Contact information</li>");
			builder.AppendLine("<li>Social links (Twitter, Facebook)</li>");
			builder.AppendLine("</ul>");
			builder.AppendLine("<p>To edit the content on this page, go to the <a href=\"http://virtocommerce.com/\">Pages</a> section of your Shopify admin.</p>");

			repository.Add(new Page
			{
				Id = string.Format("{0}/en-US/about_us.liquid", storeId),
				Path = string.Format("{0}/en-US/about_us.liquid", storeId),
				Name = "about_us",
				Language = "en-US",
				Content = builder.ToString(),
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			builder = new StringBuilder();
			builder.AppendLine("<p>Write a few sentences to tell people about your store (the kind of products you sell, your mission, etc). You can also add images and videos to help tell your story and generate more interest in your shop.</p>");
			builder.AppendLine("<p>To edit the content on this page, go to the <a href=\"http://virtocommerce.com/\">Pages</a> section of your Shopify admin.</p>");

			repository.Add(new Page
			{
				Id = string.Format("{0}/en-US/default.liquid", storeId),
				Path = string.Format("{0}/en-US/default.liquid", storeId),
				Name = "default",
				Language = "en-US",
				Content = builder.ToString(),
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			repository.UnitOfWork.Commit();
		}
	}
}
