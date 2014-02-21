#region
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Customers.Migrations;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.PowerShell.DatabaseSetup;
using Xunit;

#endregion

namespace FunctionalTests.Customers
{
    [Variant(RepositoryProvider.EntityFramework)]
    [Variant(RepositoryProvider.DataService)]
    public class CustomerCaseScenarios : FunctionalTestBase, IDisposable
    {
        #region Infrastructure/setup

        private readonly string _databaseName;
        private readonly object _previousDataDirectory;

        public CustomerCaseScenarios()
        {
            _previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
            AppDomain.CurrentDomain.SetData("DataDirectory", TempPath);
            _databaseName = "CustomersTest";
        }

        public void Dispose()
        {
            try
            {
                // Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
                // the temp location in which they are stored is later cleaned.
                using (var context = new EFCustomerRepository(_databaseName))
                {
                    context.Database.Delete();
                }
            }
            finally
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
            }
        }

        #endregion

        #region Tests

        [Fact]
        public void Can_create_customer_graph()
        {
            var client = GetRepository();

            var agent = CreateContact("Fox", "Mulder", DateTime.Now.AddYears(-50));
            agent.Addresses.Add(new Address
                {
                    City = "Los Angeles",
                    CountryCode = "test",
                    CountryName = "test",
                    Line1 = "test",
                    PostalCode = "test",
                    Name = "default",
                    DaytimePhoneNumber = "test",
                    Email = "test@test.com"
                });
            agent.Emails.Add(new Email {Address = "fm@fbi.com"});
            agent.Phones.Add(new Phone {Number = "911"});

            client.Add(agent);
            client.UnitOfWork.Commit();

            var contact = CreateContact("Bob", "Cat", DateTime.Now.AddYears(-23));
            contact.Addresses.Add(new Address
                {
                    City = "Los Angeles",
                    CountryCode = "test",
                    CountryName = "test",
                    Line1 = "test",
                    PostalCode = "test",
                    Name = "default",
                    DaytimePhoneNumber = "test",
                    Email = "test@test.com"
                });
            contact.Emails.Add(new Email {Address = "test@email.com"});
            contact.Phones.Add(new Phone {Number = "89114676315"});

            var case1 = CreateCase("3", 1, agent.MemberId, agent.FullName, "Title for case 1", "description for case 1",
                                   "1", "type for case 1");
            var case2 = CreateCase("10", 3, agent.MemberId, agent.FullName, "Title for case 2",
                                   "description for case 2", "2", "type for case 2");

            contact.Cases.Add(case1);
            contact.Cases.Add(case2);

            var label1 = CreateLabel("LABEL", string.Empty, "Description for label!");
            var label2 = CreateLabel("label", string.Empty, "Description for label 2");

            contact.Labels.Add(label1);
            contact.Labels.Add(label2);

            case1.Labels.Add(label2);
            case2.Labels.Add(label1);

            case1.CommunicationItems.Add(new PhoneCallItem
                {
                    AuthorName = "Customer Agent",
                    Body =
                        "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Direction = "Inbound",
                    PhoneNumber = "818213822",
                    Title = "Welcome to Virto Commerce"
                });
            case1.Notes.Add(new Note
                {
                    AuthorName = "Customer Agent",
                    Body = "Customer expressed desire to get more services once this case is resolved.",
                    Title = "Customer feedback"
                });

            case2.CommunicationItems.Add(new EmailItem
                {
                    AuthorName = "Customer Agent",
                    Body =
                        "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    From = "test@virtoway.com",
                    To = "customer@virtoway.com",
                    Title = "Welcome to Virto Commerce"
                });
            case2.Notes.Add(new Note
                {
                    AuthorName = "Customer Agent",
                    Body = "Customer expressed desire to get more services once this case is resolved.",
                    Title = "Customer feedback"
                });

            //client.Attach(contact);
            client.Add(contact);

            client.UnitOfWork.Commit();

            client = GetRepository();

            var caseAdded = client.Cases.First();

            //Test PublicReplyItem Email
            caseAdded.CommunicationItems.Add(new PublicReplyItem
                {
                    AuthorName = "Customer Agent",
                    Body =
                        "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Title = "Welcome to Virto Commerce"
                });
            client.UnitOfWork.Commit();

            //Contact contactFromDb = client.Contacts.Where(c => c.ContactId == contact.ContactId)
            //	.ExpandAll()
            //	.SingleOrDefault() as Contact;

            Contact contactFromDb = null;
            var a =
                client.Members.Where(m => (m as Contact).MemberId == contact.MemberId).DefaultIfEmpty(null);
            if (a != null)
            {
                //var b = a.ExpandAll();
                var c = a.OfType<Contact>();
                contactFromDb = c.SingleOrDefault();
            }
            Assert.NotNull(contactFromDb);

            Assert.NotNull(contactFromDb.Addresses);
            Assert.NotNull(contactFromDb.Cases);
            Assert.NotNull(contactFromDb.Emails);

            var caseFromDb = client.Cases.SingleOrDefault(c => c.CaseId == case2.CaseId);
            Assert.NotNull(caseFromDb);
        }

        #endregion

        #region Real Scenarios Tests

		[Fact]
	    public void Can_create_labels_to_case_and_contact()
		{

			var client = GetRepository();

			var contactToDb = new Contact();
			contactToDb.FullName = "test";

			var caseToDb = new Case();

			contactToDb.Cases.Add(caseToDb);

			var labelForContact = new Label(){Name = "testName", Description = "testDescription"};

			var labelForCase = new Label() { Name = "testName", Description = "testDescription"};

			caseToDb.Labels.Add(labelForCase);
			
			contactToDb.Labels.Add(labelForContact);

			client.Add(contactToDb);
			client.UnitOfWork.Commit();


			client = GetRepository();

			var contactFromDB =
				client.Members.OfType<Contact>()
					.Where(c => c.MemberId == contactToDb.MemberId)
					.Expand(c => c.Cases)
					.Expand(c => c.Labels).SingleOrDefault();

			Assert.NotNull(contactFromDB);
			Assert.NotNull(contactFromDB.Cases);
			Assert.True(contactFromDB.Labels.Count==1);



			var caseFromDb =
				client.Cases.Where(c => c.CaseId == caseToDb.CaseId).Expand(c => c.Labels).SingleOrDefault();

			Assert.NotNull(caseFromDb);
			Assert.True(caseFromDb.Labels.Count == 1);

		}

		[Fact]
        public void Can_change_case_status()
        {
            //get from db Case with status==Open
            var client = GetRepository();

	        var caseToDb = new Case {Title = "Case_change_Status_test", Status = CaseStatus.Open.ToString()};

			client.Add(caseToDb);
	        client.UnitOfWork.Commit();
	        client = GetRepository();

	        var caseWithOpenedStatusFromDb = client.Cases.Where(c => c.CaseId == caseToDb.CaseId).SingleOrDefault();

			Assert.NotNull(caseWithOpenedStatusFromDb);
			Assert.True(caseWithOpenedStatusFromDb.Status==CaseStatus.Open.ToString());

	        caseWithOpenedStatusFromDb.Status = CaseStatus.Pending.ToString();
			//после этого вызова объект Case удалится из коллекции client.Cases, хотя я этого не делаю
			//Adomas: No Magic here, just Parent validator was removing item even if FK property was not modified
	        client.UnitOfWork.Commit();


	        client = GetRepository();

	        var caseWithPendingStatusFromDb =
		        client.Cases.Where(c => c.CaseId == caseWithOpenedStatusFromDb.CaseId).SingleOrDefault();

			Assert.NotNull(caseWithPendingStatusFromDb);
	        Assert.True(caseWithPendingStatusFromDb.Status == CaseStatus.Pending.ToString());
        }

	    [Fact]
	    public void Can_create_Email_in_Contact()
	    {
		    var client = GetRepository();

		    var contactWithNewEmail = new Contact();
		    contactWithNewEmail.FullName = "test";

		    var emailToAdd = new Email();
		    emailToAdd.Address = "test@test.ru";
		    emailToAdd.Type = "Primary";

		    contactWithNewEmail.Emails.Add(emailToAdd);

		    client.Add(contactWithNewEmail);
		    client.UnitOfWork.Commit();


		    client = GetRepository();

		    var contactFromDb =
			    client.Members.OfType<Contact>()
				    .Where(c => c.MemberId == contactWithNewEmail.MemberId)
				    .Expand(c => c.Emails)
				    .SingleOrDefault();

		    Assert.NotNull(contactFromDb);
		    Assert.True(contactFromDb.Emails.Count == 1);
	    }

	    [Fact]
        public void Can_edit_Address_in_Contact()
        {
            var client = GetRepository();

			var contactWithNewAddress = new Contact();
			contactWithNewAddress.FullName = "test";


            var address1 = new Address();
	        address1.Name = "testName1";
            address1.City = "testCity1";
            address1.CountryCode = "testCountryCode1";
            address1.CountryName = "testCountryName1";
            address1.Line1 = "testLine1_1";
            address1.Line2 = "testLine2_1";
            address1.PostalCode = "testPostalCode1";
            address1.RegionName = "testResgion1";
            address1.StateProvince = "testState1";
            address1.Type = "Primary";

            var address2 = new Address();
			address2.Name = "testName2";
            address2.City = "testCity2";
            address2.CountryCode = "testCountryCode2";
            address2.CountryName = "testCountryName2";
            address2.Line1 = "testLine1_2";
            address2.Line2 = "testLine2_2";
            address2.PostalCode = "testPostalCode2";
            address2.RegionName = "testResgion2";
            address2.StateProvince = "testState2";
            address2.Type = "Primary";

            //add 2 addresses to contact
			contactWithNewAddress.Addresses.Add(address1);
			contactWithNewAddress.Addresses.Add(address2);

			Assert.True(contactWithNewAddress.Addresses.Count != 0);

            //save contact
			client.Add(contactWithNewAddress);
            client.UnitOfWork.Commit();


            client = GetRepository();

			var contactForCheck = client.Members.Where(m => m.MemberId == contactWithNewAddress.MemberId).OfType<Contact>().
                                         Expand(c => c.Addresses).SingleOrDefault();

            client.Attach(contactForCheck);

            Assert.NotNull(contactForCheck);
            Assert.NotNull(contactForCheck.Addresses);
            Assert.True(contactForCheck.Addresses.Count == 2);

            //edit address 1
            var address1ForCheck =
                contactForCheck.Addresses.Where(a => a.AddressId == address1.AddressId).SingleOrDefault();
            address1ForCheck.Line1 = "updated line 1_1";
            address1ForCheck.Line2 = "updated line 2_1";
            //edit address 2
            var address2ForCheck =
                contactForCheck.Addresses.Where(a => a.AddressId == address2.AddressId).SingleOrDefault();
            address2ForCheck.Line1 = "updated line 1_2";
            address2ForCheck.Line2 = "updated line 2_2";

			
            client.UnitOfWork.Commit();


            client = GetRepository();

            var contactForDelete =
				client.Members.Where(m => m.MemberId == contactWithNewAddress.MemberId).OfType<Contact>().
                       Expand(c => c.Addresses).SingleOrDefault();

            client.Attach(contactForDelete);

            Assert.NotNull(contactForDelete);
            Assert.NotNull(contactForDelete.Addresses);
            Assert.True(contactForDelete.Addresses.Count >= 2);

            var updatedAddress1 =
                contactForDelete.Addresses.Where(a => a.AddressId == address1ForCheck.AddressId).SingleOrDefault();

            Assert.NotNull(updatedAddress1);
            Assert.True(updatedAddress1.Line1 == "updated line 1_1");
            Assert.True(updatedAddress1.Line2 == "updated line 2_1");

            var updatedAddress2 =
                contactForDelete.Addresses.Where(a => a.AddressId == address2ForCheck.AddressId).SingleOrDefault();

            Assert.NotNull(updatedAddress2);
            Assert.True(updatedAddress2.Line1 == "updated line 1_2");
            Assert.True(updatedAddress2.Line2 == "updated line 2_2");


            contactForDelete.Addresses.Clear();
            client.Remove(updatedAddress1);
            client.Remove(updatedAddress2);

            client.UnitOfWork.Commit();


            client = GetRepository();

            var contactWithRemovedAddresses =
				client.Members.Where(m => m.MemberId == contactWithNewAddress.MemberId).OfType<Contact>().
                       Expand(c => c.Addresses).SingleOrDefault();

            Assert.NotNull(contactWithRemovedAddresses);
            Assert.NotNull(contactWithRemovedAddresses.Addresses);
            Assert.True(contactWithRemovedAddresses.Addresses.Count == 0);
        }

        [Fact]
        public void Can_edit_Phone_in_Contact()
        {
            var client = GetRepository();

			var contactWithNewPhone = new Contact();
			contactWithNewPhone.FullName = "test";

            //create new phone
            var phoneToAdd = new Phone();
            phoneToAdd.Number = "228810";

			contactWithNewPhone.Phones.Add(phoneToAdd);

			client.Add(contactWithNewPhone);
            client.UnitOfWork.Commit();


            client = GetRepository();

            var contactForCheck = client.Members.OfType<Contact>().
                                         Expand(c => c.Phones).FirstOrDefault();

            client.Attach(contactForCheck);

            Assert.NotNull(contactForCheck);

            //get phone for update
            var phoneForCheck = contactForCheck.Phones.Where(p => p.PhoneId == phoneToAdd.PhoneId).SingleOrDefault();
            Assert.NotNull(phoneForCheck);

            phoneForCheck.Number = "666";

            client.UnitOfWork.Commit();


            client = GetRepository();

            contactForCheck = client.Members.OfType<Contact>().
                                     Expand(c => c.Phones).FirstOrDefault();
			Assert.NotNull(contactForCheck);

            phoneForCheck = contactForCheck.Phones.Where(p => p.PhoneId == phoneToAdd.PhoneId).SingleOrDefault();
            Assert.NotNull(phoneForCheck);
            Assert.True(phoneForCheck.Number == "666");
        }

        [Fact]
        public void Can_create_Notes_in_Contact()
        {
            var client = GetRepository();


	        var contactWithNewNote = new Contact();
	        contactWithNewNote.FullName = "test";

            var noteForContact = new Note();
            noteForContact.Body = "noteForCase";


			contactWithNewNote.Notes.Add(noteForContact);
			client.Add(contactWithNewNote);

            client.UnitOfWork.Commit();

            client = GetRepository();


			var contactForCheck = client.Members.Where(m => m.MemberId == contactWithNewNote.MemberId).OfType<Contact>()
                                        .Expand(c => c.Notes).SingleOrDefault();

            Assert.NotNull(contactForCheck);
            Assert.True(contactForCheck.Notes.Count >= 1);
        }

        [Fact]
        public void Can_create_case()
        {
            var client = GetRepository();

            var caseToAdd = new Case();

            client.Add(caseToAdd);
            client.UnitOfWork.Commit();

            client = GetRepository();

            var caseFromDb = client.Cases.Where(cs => cs.CaseId == caseToAdd.CaseId).SingleOrDefault();

            Assert.NotNull(caseFromDb);
        }

        [Fact]
        public void Can_create_contact()
        {
            var client = GetRepository();

            var contactToDb = new Contact();
	        contactToDb.FullName = "name_for_test_contact";

            client.Add(contactToDb);
            client.UnitOfWork.Commit();

            client = GetRepository();

            var contactFromDb =
                client.Members.Where(m => m.MemberId == contactToDb.MemberId).OfType<Contact>().SingleOrDefault();

            Assert.NotNull(contactFromDb);
        }

        #endregion

        #region Helper Methods

        private ICustomerRepository GetRepository()
        {
			EnsureDatabaseInitialized(() => new EFCustomerRepository(_databaseName), () => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFCustomerRepository, Configuration>()));
            var retVal = new EFCustomerRepository(_databaseName);
            return retVal;
        }

        private Contact CreateContact(string firstName, string lastName, DateTime birthDate)
        {
            var cont = new Contact();

            cont.FullName = firstName + " " + lastName;
            cont.BirthDate = birthDate;

            return cont;
        }

        private Case CreateCase(string number, int priority, string agentId, string agentName, string title,
                                string description, string status, string type)
        {
            var c = new Case();

            c.Number = number;
            c.Priority = priority;
            c.AgentId = agentId;
            c.AgentName = agentName;
            c.Title = title;
            c.Description = description;
            c.Status = status;
            c.Channel = type;

            return c;
        }

        private Label CreateLabel(string name, string imgUrl, string description)
        {
            var label = new Label();

            label.Name = name;
            label.ImgUrl = imgUrl;
            label.Description = description;

            return label;
        }

        private Address CreateAddress(string line1, string name, string city, string countryCode, string countryName,
                                      string postalCode)
        {
            var retVal = new Address
                {
                    Line1 = line1,
                    Name = name,
                    City = city,
                    CountryCode = countryCode,
                    CountryName = countryName,
                    PostalCode = postalCode,
                };

            return retVal;
        }

        #endregion
    }
}