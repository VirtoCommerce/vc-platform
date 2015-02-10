using System;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.CustomerModule.Web.Controllers.Api;
using VirtoCommerce.CustomerModule.Web.Model;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using webModel = VirtoCommerce.CustomerModule.Web.Model;

namespace VirtoCommerce.CustomerModule.Test
{
	[TestClass]
	public class ContactControllerTest
	{
		[TestMethod]
		public void SearchContactsTest()
		{
			var controller = GetContactController();
			var result = controller.Search(new SearchCriteria()) as OkNegotiatedContentResult<SearchResult>;
			Assert.IsNotNull(result.Content);
		}
		
		[TestMethod]
		public void CreateNewContact()
		{
			var controller = GetContactController();
			var contact = new webModel.Contact
			{
				 Id = "testContact",
				 FullName = "Vasa",
				 BirthDate = DateTime.UtcNow,
				 Addresses = new webModel.Address[]
				{
					new webModel.Address {	
					Name = "some name",	 
					AddressType = coreModel.AddressType.Shipping, 
					City = "london",
					Phone = "+68787687",
					PostalCode = "22222",
					CountryCode = "ENG",
					CountryName = "England",
					Email = "user@mail.com",
					FirstName = "first name",
					LastName = "last name",
					Line1 = "line 1",
					Organization = "org1"
					}
				}.ToList(),
				 Notes = new webModel.Note[] { new webModel.Note { Title = "1111", Body = "dfsdfs sdf sdf sdf sd" } },
				 Emails = new string[] {  "uuu@mail.ru", "ssss@mail.ru" },
				 Phones = new string[] {  "2322232", "32323232" },
				 Properties = new webModel.Property[] { new webModel.Property { Name = "testProp", Value = "sss", ValueType = coreModel.PropertyValueType.ShortText } }.ToList(),
				 DefaultLanguage = "ru"
			};
			var result = controller.Create(contact) as OkNegotiatedContentResult<webModel.Contact>;
			Assert.IsNotNull(result.Content);
		}

		[TestMethod]
		public void UpdateContact()
		{
			var controller = GetContactController();
			var result = controller.GetContactById("testContact") as OkNegotiatedContentResult<webModel.Contact>;
			var contact = result.Content;

			contact.FullName = "diff name";
			contact.Emails.Remove(contact.Emails.FirstOrDefault());
			contact.Properties.Add(new webModel.Property { Name = "setting2", Value = "1223", ValueType = coreModel.PropertyValueType.Integer });

			controller.Update(contact);

			result = controller.GetContactById("testContact") as OkNegotiatedContentResult<webModel.Contact>;

			contact = result.Content;
		}

		[TestMethod]
		public void PartialUpdateContact()
		{
			var controller = GetContactController();
			var contact = new webModel.Contact
			{
				Id = "testContact",
				FullName = "ET"
			};
			
			controller.Update(contact);

			var result = controller.GetContactById("testContact") as OkNegotiatedContentResult<webModel.Contact>;

			contact = result.Content;
		}



		[TestMethod]
		public void DeleteStore()
		{
			var controller = GetContactController();
			controller.Delete(new string[] { "testContact" });
			var result = controller.GetContactById("testStore") as OkNegotiatedContentResult<webModel.Contact>;

			Assert.IsNull(result);

		}
		private static ContactController GetContactController()
		{
			Func<IFoundationCustomerRepository> customerRepositoryFactory = () =>
			{
				return new FoundationCustomerRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			};
			var service = new ContactServiceImpl(customerRepositoryFactory);
			return new ContactController(service, service);
		}
	
	}
}
