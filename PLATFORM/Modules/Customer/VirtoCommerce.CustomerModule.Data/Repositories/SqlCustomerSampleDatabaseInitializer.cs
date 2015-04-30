using System;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Repositories
{
	public class SqlCustomerSampleDatabaseInitializer : SetupDatabaseInitializer<CustomerRepositoryImpl, VirtoCommerce.CustomerModule.Data.Migrations.Configuration>
	{
		private const string _customerInformationName = "Customer Information";
		private const string _casesInformationName = "Case Information";

		protected override void Seed(CustomerRepositoryImpl context)
		{
			base.Seed(context);
			//FillCustomerContent(context);
		}

		private void FillCustomerContent(CustomerRepositoryImpl client)
		{
			var contact = CreateContact("John", "Doe", DateTime.Now.AddYears(-24));
			contact.Id = "1"; //Mapped with admin account

			contact.Addresses.Add(new Address { Name = "Primary Address", FirstName = "John", LastName = "Doe", City = "Beverly Hills", CountryCode = "USA", StateProvince = "CA", CountryName = "United States", Line1 = "8237 Santa Monica Blvd", PostalCode = "90210", DaytimePhoneNumber = "none", Email = "john_doe_work@gmail.com", MemberId = contact.Id });
			contact.Emails.Add(new Email { Address = "john_doe@gmail.com", MemberId = contact.Id, Type = "Primary" });
			contact.Emails.Add(new Email { Address = "john_doe@outlook.com", MemberId = contact.Id, Type = "Primary" });
			contact.Phones.Add(new Phone { Number = "89520050242", MemberId = contact.Id });

			
			client.Add(contact);

	
			client.Add(CreateContactAttributeValue("LastVisit", false, contact.Id, DateTime.Now, 0, 0, null, null, coreModel.PropertyValueType.DateTime.GetHashCode(), 1));
			client.Add(CreateContactAttributeValue("LastOrder", false, contact.Id, DateTime.Today.AddDays(-10), 0, 0, null, null, coreModel.PropertyValueType.DateTime.GetHashCode(), 2));
			client.Add(CreateContactAttributeValue("Additional note", false, contact.Id, null, 0, 0, null, contact.FullName, coreModel.PropertyValueType.ShortText.GetHashCode(), 5));


			var contact2 = CreateContact("Bill", "Ballmer", new DateTime(1965, 5, 23));
			contact2.Id = "2";

		
			contact2.Addresses.Add(new Address { Name = "Primary Address", FirstName = "Bill", LastName = "Ballmer", City = "New York", CountryCode = "USA", StateProvince = "CA", CountryName = "United States", Line1 = "1556 Broadway", PostalCode = "90002", DaytimePhoneNumber = "none", Email = "bb_1965@outlook.com", MemberId = contact2.Id });
			contact2.Emails.Add(new Email { Address = "bb_1965@outlook.com", MemberId = contact2.Id, Type = "Primary" });
			contact2.Phones.Add(new Phone { Number = "8239-1234", MemberId = contact2.Id });

			client.Add(contact2);

			client.UnitOfWork.Commit();
		}

	

		private ContactPropertyValue CreateContactAttributeValue(string name, bool booleanValue, string contactId, DateTime? dateTimeValue, decimal decimalValue, int integerValue, string longTextValue, string shortTextValue, int valueType, int priority)
		{
			return new ContactPropertyValue
			{
				Name = name,
				BooleanValue = booleanValue,
				ContactId = contactId,
				DateTimeValue = dateTimeValue,
				DecimalValue = decimalValue,
				IntegerValue = integerValue,
				LongTextValue = longTextValue,
				ShortTextValue = shortTextValue,
				ValueType = valueType,
				Priority = priority
			};
		}

	
		private Contact CreateContact(string firstName, string lastName, DateTime birthDate)
		{
			return new Contact { FullName = firstName + " " + lastName, BirthDate = birthDate };
		}

	}
}
