using System;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;

namespace VirtoCommerce.Foundation.Data.Customers
{
	public class SqlCustomerSampleDatabaseInitializer : SqlCustomerDatabaseInitializer
	{
		private const string _customerInformationName = "Customer Information";
		private const string _casesInformationName = "Case Information";

		protected override void Seed(EFCustomerRepository context)
		{
			base.Seed(context);
			FillCustomerContent(context);
			FillCaseSubjectContent(context);
			FillCaseRulesContent(context);
			FillCasePropertyContent(context);
			FillCustomersScripts(context);
		}

		private void FillCustomerContent(EFCustomerRepository client)
		{
			var contact = CreateContact("John", "Doe", DateTime.Now.AddYears(-24));
			contact.MemberId = "1"; //Mapped with admin account

			Case case1 = CreateCase("CS234s23", CasePriority.Low, "Welcome to Virto Commerce", "", CaseStatus.Open, CaseChannel.CommerceManager);
			Case case2 = CreateCase("CS3dfs34", CasePriority.Medium, "Welcome to Virto Commerce", "", CaseStatus.Pending, CaseChannel.CommerceManager);

			contact.Cases.Add(case1);
			contact.Cases.Add(case2);

			contact.Addresses.Add(new Address { Name = "Primary Address", FirstName = "John", LastName = "Doe", City = "Beverly Hills", CountryCode = "USA", StateProvince = "CA", CountryName = "United States", Line1 = "8237 Santa Monica Blvd", PostalCode = "90210", DaytimePhoneNumber = "none", Email = "john_doe_work@gmail.com", MemberId = contact.MemberId });
			contact.Emails.Add(new Email { Address = "john_doe@gmail.com", MemberId = contact.MemberId, Type = EmailType.Primary.ToString() });
			contact.Emails.Add(new Email { Address = "john_doe@outlook.com", MemberId = contact.MemberId, Type = EmailType.Secondary.ToString() });
			contact.Phones.Add(new Phone { Number = "89520050242", MemberId = contact.MemberId });

			case1.CommunicationItems.Add(new PhoneCallItem { AuthorName = "Admin", Body = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", Direction = "Inbound", PhoneNumber = "818213822", Title = "Welcome to Virto Commerce" });
			case1.Notes.Add(new Note { AuthorName = "Admin", Body = "Customer expressed desire to get more services once this case is resolved.", Title = "Customer feedback" });

			case2.CommunicationItems.Add(new EmailItem { AuthorName = "Admin", Body = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", From = "test@virtoway.com", To = "customer@virtoway.com", Title = "Welcome to Virto Commerce" });
			case2.Notes.Add(new Note { AuthorName = "Admin", Body = "Customer expressed desire to get more services once this case is resolved.", Title = "Customer feedback" });

			client.Add(contact);

			client.Add(CreateCaseAttributeValue("Agent", false, case1.CaseId, null, 0, 0, null, case1.AgentId, PropertyValueType.ShortString.GetHashCode(), 1));

			client.Add(CreateContactAttributeValue(ContactPropertyValueName.LastVisit, false, contact.MemberId, DateTime.Now, 0, 0, null, null, PropertyValueType.DateTime.GetHashCode(), 1));
			client.Add(CreateContactAttributeValue(ContactPropertyValueName.LastOrder, false, contact.MemberId, DateTime.Today.AddDays(-10), 0, 0, null, null, PropertyValueType.DateTime.GetHashCode(), 2));
			client.Add(CreateContactAttributeValue("Additional note", false, contact.MemberId, null, 0, 0, null, contact.FullName, PropertyValueType.ShortString.GetHashCode(), 5));


			var contact2 = CreateContact("Bill", "Ballmer", new DateTime(1965, 5, 23));
			contact2.MemberId = "2";

			Case caseForContact2 = CreateCase("CS43sd53", CasePriority.High, "Very Important Case", "",
											  CaseStatus.Resolved, CaseChannel.CommerceManager);

			contact2.Cases.Add(caseForContact2);


			contact2.Addresses.Add(new Address { Name = "Primary Address", FirstName = "Bill", LastName = "Ballmer", City = "New York", CountryCode = "USA", StateProvince = "CA", CountryName = "United States", Line1 = "1556 Broadway", PostalCode = "90002", DaytimePhoneNumber = "none", Email = "bb_1965@outlook.com", MemberId = contact2.MemberId });
			contact2.Emails.Add(new Email { Address = "bb_1965@outlook.com", MemberId = contact2.MemberId, Type = EmailType.Primary.ToString() });
			contact2.Phones.Add(new Phone { Number = "8239-1234", MemberId = contact2.MemberId });

			client.Add(contact2);

			client.UnitOfWork.Commit();
		}

		private void FillCaseSubjectContent(ICustomerRepository client)
		{
			//            string.Format("Request on Order {0}", orderId)
			//, string.Format("Return to Order {0}", orderId)
			//, string.Format("Exchange on Order {0}", orderId)
			//, string.Format("Changes to Order {0}", orderId)
			//, "Ordering"
			//, "Client registration"
			//, "Product inquiry"
			//, "Shipping inquiry"
			//, "Billing inquiry"

			var item = CreateCaseSubject("Request on Order", "Request on Order (any)", true);
			item.CaseTemplateProperties.Add(CreateCaseSubjectProperty("Order ID", PropertyValueType.ShortString));
			client.Add(item);

			item = CreateCaseSubject("Return to Order", "Return to Order", true);
			item.CaseTemplateProperties.Add(CreateCaseSubjectProperty("Order ID", PropertyValueType.ShortString));
			client.Add(item);

			item = CreateCaseSubject("Exchange on Order", "Exchange on Order", true);
			item.CaseTemplateProperties.Add(CreateCaseSubjectProperty("Order ID", PropertyValueType.ShortString));
			client.Add(item);

			item = CreateCaseSubject("Client registration", "Client registration", true);
			client.Add(item);

			item = CreateCaseSubject("Product inquiry", "", true);
			client.Add(item);

			item = CreateCaseSubject("Shipping inquiry", "", true);
			client.Add(item);

			item = CreateCaseSubject("Billing inquiry", "", true);
			client.Add(item);

			client.UnitOfWork.Commit();
		}

		private void FillCaseRulesContent(ICustomerRepository client)
		{
			var item = CreateCaseRule("Displays latest order by a customer", "Latest Order", 0, 2);
			client.Add(item);
			item = CreateCaseRule("Displays last communication with a customer and a status of it", "Last Contact", 1, 1);
			client.Add(item);

			client.UnitOfWork.Commit();
		}

		private void FillCasePropertyContent(ICustomerRepository client)
		{
			var item = CreateCasePropertySet(1, _customerInformationName);
			item.CaseProperties.Add(CreateCaseProperty("Contact.BirthDate", "Birthday", 1));
			item.CaseProperties.Add(CreateCaseProperty("Contact.FirstName", "FirstName", 2));
			item.CaseProperties.Add(CreateCaseProperty("Contact.LastName", "LastName", 3));
			//item.CaseProperties.Add(CreateCaseProperty("Cases.Count", "Total Cases", 2));
			client.Add(item);

			item = CreateCasePropertySet(2, _casesInformationName);
			item.CaseProperties.Add(CreateCaseProperty("Site", "Referral Site", 1));
			item.CaseProperties.Add(CreateCaseProperty("SLA", "SLA", 2));
			client.Add(item);

			client.UnitOfWork.Commit();
		}

		private void FillCustomersScripts(EFCustomerRepository client)
		{
			ExecuteSqlScriptFile(client, "FillRequiredCaseProperty.sql", "Customers");
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

		private CasePropertyValue CreateCaseAttributeValue(string name, bool booleanValue, string caseId, DateTime? dateTimeValue, decimal decimalValue, int integerValue, string longTextValue, string shortTextValue, int valueType, int priority)
		{
			return new CasePropertyValue
			{
				Name = name,
				BooleanValue = booleanValue,
				CaseId = caseId,
				DateTimeValue = dateTimeValue,
				DecimalValue = decimalValue,
				IntegerValue = integerValue,
				LongTextValue = longTextValue,
				ShortTextValue = shortTextValue,
				ValueType = valueType,
				Priority = priority
			};
		}

		private CasePropertySet CreateCasePropertySet(int priority, string name)
		{
			return new CasePropertySet
			{
				Priority = priority,
				Name = name
			};
		}

		private CaseProperty CreateCaseProperty(string fieldName, string name, int priority)
		{
			return new CaseProperty
			{
				FieldName = fieldName,
				Priority = priority,
				Name = name
			};
		}

		private CaseRule CreateCaseRule(string description, string name, int status, int priority)
		{
			var item = new CaseRule
			{
				Description = description,
				Name = name,
				Status = status,
				Priority = priority
			};
			return item;
		}

		private CaseTemplate CreateCaseSubject(string name, string description, bool isActive)
		{
			var item = new CaseTemplate
			{
				Name = name,
				Description = description,
				IsActive = isActive
			};
			return item;
		}

		private CaseTemplateProperty CreateCaseSubjectProperty(string name, PropertyValueType valueType)
		{
			return new CaseTemplateProperty
			{
				Name = name,
				ValueType = (int)valueType
			};
		}

		private Case CreateCase(string number, CasePriority priority, string title, string description, CaseStatus status, CaseChannel channel)
		{
			return new Case
				{
					Number = number,
					Priority = (int)priority,
					Title = title,
					Description = description,
					Status = status.ToString(),
					Channel = channel.ToString()
				};
		}

		private Contact CreateContact(string firstName, string lastName, DateTime birthDate)
		{
			return new Contact { FullName = firstName + " " + lastName, BirthDate = birthDate };
		}

		/*
				private Label CreateLabel(string name, string imgUrl, string description)
				{
					return new Label {Name = name, ImgUrl = imgUrl, Description = description};
				}
		*/

		/*
				private Address CreateAddress(string line1, string line2, string city, string countryCode, string countryName, string region, string postalCode, string type)
				{
				   return new Address
					{
						Line1 = line1,
						Line2 = line2,
						City = city,
						CountryCode = countryCode,
						CountryName = countryName,
						RegionName = region,
						PostalCode = postalCode,
						Type = type
					};
				}
		*/

		/*
				private PhoneCallItem CreatePhoneCall(string author, string title, string body, string direction, string phone)
				{
					return new PhoneCallItem
						{
							AuthorName = author,
							Body = body,
							Direction = direction,
							PhoneNumber = phone,
							Title = title
						};
				}
		*/
	}
}
