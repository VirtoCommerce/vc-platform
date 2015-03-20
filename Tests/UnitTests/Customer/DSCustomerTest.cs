using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.CommerceUnitTest.Utils;

namespace CommerceFoundation.UnitTests.Customer
{


	[TestClass]
	public class CustomerServiceDataServiceTest
	{

		private static TestService service;
		private static List<Action> EndTestAction = new List<Action>();

		[ClassInitialize()]
		public static void ClassInitialize(TestContext testContext)
		{
			service = new TestService(typeof(TestCustomerDataService));
		}

		[ClassCleanup()]
		public static void ClassCleanup()
		{
			if (service != null)
			{
				service.Dispose();
				service = null;
			}
			foreach (Action a in EndTestAction)
			{
				try
				{
					a.Invoke();
				}
				catch { }
			}
			EndTestAction.Clear();
		}


		[TestCategory("CustomersTests"), TestMethod()]
		public void CreateFullGraphCustomerService()
		{
			ICustomerRepository client = GetRepository();

			var contact = CreateContact("FirstName", "Lastname", DateTime.Now.AddYears(-23), string.Empty);

			Case case1 = CreateCase("3", 1, "agentId11", "agentName11", "Title for case 1", "description for case 1", "1", "type for case 1");
			Case case2 = CreateCase("10", 3, "agentId22", "agentName22", "Title for case 2", "description for case 2", "2", "type for case 2");
			//case1.Contact = contact;
			//case2.Contact = contact;

			contact.Cases.Add(case1);
			contact.Cases.Add(case2);

			contact.Addresses.Add(new Address() { City = "kaliningrad", CountryCode = "teset", CountryName = "test", Line1 = "tets", PostalCode = "tet" });
            contact.Emails.Add(new Email() { Address = "test@live.ru" });
			contact.Phones.Add(new Phone() { Number = "89114676315" });

			Label label1 = CreateLabel("LABEL", string.Empty, "Description for label!");
			Label label2 = CreateLabel("label", string.Empty, "Description for label 2");

			contact.Labels.Add(label1);
			contact.Labels.Add(label2);

			case1.Labels.Add(label2);
			case2.Labels.Add(label1);

			//client.Attach(contact);
			client.Add(contact);


			client.UnitOfWork.Commit();

			client = GetRepository();

			//Contact contactFromDb = client.Contacts.Where(c => c.ContactId == contact.ContactId)
			//	.ExpandAll()
			//	.SingleOrDefault() as Contact;

			Contact contactFromDb = null;
            var a = client.Members.Where(m => (m as Contact).MemberId == contact.MemberId).DefaultIfEmpty(null);
			if (a != null)
			{
				//var b = a.ExpandAll();
				var c = a.OfType<Contact>();
				contactFromDb = c.SingleOrDefault();
			}
			Assert.IsNotNull(contactFromDb);

			Assert.IsNotNull(contactFromDb.Addresses);
			Assert.IsNotNull(contactFromDb.Cases);
			Assert.IsNotNull(contactFromDb.Emails);

			Case caseFromDb = client.Cases.Where(c => c.CaseId == case2.CaseId).SingleOrDefault();
			Assert.IsNotNull(caseFromDb);

		}

		#region Real Scenarios Tests

		[TestMethod]
		public void CreateNewContactWithNewCaseTest()
		{
			//ICustomerRepository client = new EFCustomersRepository(new CustomerEntityFactory());
			var client = GetRepository();


			Contact contactToDb = new Contact();
			contactToDb.BirthDate = new DateTime(1970, 8, 15);
			contactToDb.FullName = "dataServiceFullnameTest";

			Case caseToDb = new Case();
			caseToDb.Description = "dataServiceDescriptionTest";
			caseToDb.Number = "qweqweas";
			caseToDb.Priority = 1;
			caseToDb.Status = CaseStatus.Open.ToString();
			caseToDb.Title = "dataServiceTitleTest";



			contactToDb.Addresses.Add(CreateAddress("Верхнеозерная", "25/8", "Калининград", "RUS", "Russianfederation", string.Empty, "238210", "Primary"));
			contactToDb.Addresses.Add(CreateAddress("пл. Победы", "1", "Калининград", "RUS", "Russianfederation", string.Empty, "666259", "Shipping"));

            contactToDb.Emails.Add(new Email { Address = "sg@xvideos.com", Type = "Primary" });
            contactToDb.Emails.Add(new Email { Address = "sashagrey@facebook.com", Type = "Secondary" });

			contactToDb.Phones.Add(new Phone { Number = "12346578912", Type = "Primary" });


			//add labels

			var labels = client.Labels.ToList();

			foreach (var lb in labels)
			{
				client.Attach(lb);
			}

			var label1 = labels[0];
			var label2 = labels[1];

			contactToDb.Labels.Add(label1);

			caseToDb.Labels.Add(label1);
			caseToDb.Labels.Add(label2);


			//save new contact in db
			caseToDb.ContactId = contactToDb.MemberId;
			client.Add(caseToDb);
			client.Add(contactToDb);



			client.UnitOfWork.Commit();

			string contactId = contactToDb.MemberId;

			client = new DSCustomerClient(service.ServiceUri, new CustomerEntityFactory(), null);

			Contact contactFromDb = client.Members.Where(m => (m as Contact).MemberId == contactId).OfType<Contact>()
				.Expand(c => c.Addresses).Expand(c => c.Cases).Expand(c => c.Emails)
				.Expand(c => c.Labels).Expand(c => c.Notes).Expand(c => c.Phones).SingleOrDefault();


			//contact is saved correctly
			Assert.IsNotNull(contactFromDb);
			//cases count>0
			Assert.IsTrue(contactFromDb.Cases.Count > 0);
			//addresses count>0
			Assert.IsTrue(contactFromDb.Addresses.Count > 0);
			//phones count > 0
			Assert.IsTrue(contactFromDb.Phones.Count > 0);
			//emails count > 0
			Assert.IsTrue(contactFromDb.Emails.Count > 0);

			Assert.IsTrue(contactToDb.Labels.Count == 1);

		}


		[TestMethod]
		public void CreateNewInboundCallCaseWithExistingContactTest()
		{
			var client = GetRepository();

			Contact contactFromDb = client.Members.OfType<Contact>().Expand(c => c.Addresses)
				.Expand(c => c.Cases).Expand(c => c.Emails).Expand(c => c.Labels).Expand(c => c.Notes)
				.Expand(c => c.Phones).FirstOrDefault();

			client.Attach(contactFromDb);

			Assert.IsNotNull(contactFromDb);

			Case inboundCall = CreateCase("2", 0, string.Empty, string.Empty, "Very important", "Where is my drugs?!?!", CaseStatus.Open.ToString(), CaseChannel.Email.ToString());
			inboundCall.CommunicationItems.Add(new PhoneCallItem { Title = "PhoneTitle", Body = "body", Direction = "Inbound" });


			contactFromDb.Cases.Add(inboundCall);

			var labels = client.Labels.ToList();
			Label label1 = labels[0];
			Label label2 = labels[1];
			client.Attach(label1);
			client.Attach(label2);

			inboundCall.Labels.Add(label1);
			contactFromDb.Labels.Add(label2);

			client.Update(label1);
			client.Update(label2);


			contactFromDb.Cases.Add(inboundCall);
			client.Update(contactFromDb);

			client.UnitOfWork.Commit();

			client = GetRepository();

			Contact contactForCheck = client.Members.Where(m => m.MemberId == contactFromDb.MemberId).OfType<Contact>()
				.Expand(c => c.Addresses).Expand(c => c.Cases).Expand(c => c.Emails)
				.Expand(c => c.Labels).Expand(c => c.Notes).Expand(c => c.Phones)
				.SingleOrDefault();

			Case caseForCheck = contactForCheck.Cases.Where(c => c.CaseId == inboundCall.CaseId)
				.SingleOrDefault();

			caseForCheck = client.Cases.Where(c => c.CaseId == caseForCheck.CaseId)
				.Expand(c => c.Labels).Expand(c => c.Notes).SingleOrDefault();

			Assert.IsNotNull(contactForCheck);
			Assert.IsNotNull(caseForCheck);
			Assert.IsTrue(contactForCheck.Cases.Contains(caseForCheck));
			Assert.IsFalse(contactForCheck.Labels.Contains(label1));

			Assert.IsNotNull(caseForCheck.Labels);



		}


		[TestMethod]
		public void CreateNewOutboundCallCaseWithExistingContactTest()
		{
			//get from db Case with status==Open
			var client = GetRepository();

			Contact qqqContact = client.Members.Where(m => (m as Contact).FullName == "qqq").OfType<Contact>()
				.Expand(c => c.Addresses).Expand(c => c.Cases).Expand(c => c.Emails)
				.Expand(c => c.Labels).Expand(c => c.Notes).Expand(c => c.Phones)
				.SingleOrDefault();

			client.Attach(qqqContact);

			Assert.IsNotNull(qqqContact);

			//create new case
			Case newCase = new Case();

			newCase.Description = "testDescroption";
			newCase.Number = "adasdqw";
			newCase.Priority = 1;
			newCase.Status = CaseStatus.Open.ToString();
			newCase.Title = "cooltitle";


			//clear existing contact's labels
			qqqContact.Labels.Clear();


			//get all labels
			var labels = client.Labels.ToList();
			var label1 = labels[0];
			client.Attach(label1);


			qqqContact.Labels.Add(label1);
			newCase.Labels.Add(label1);

			newCase.ContactId = qqqContact.MemberId;

			client.Add(newCase);
			client.Update(qqqContact);

			client.UnitOfWork.Commit();


			client = GetRepository();

			Contact qqqFromDbContact = client.Members.Where(m => (m as Contact).FullName == "qqq").OfType<Contact>()
				.Expand(c => c.Addresses).Expand(c => c.Cases).Expand(c => c.Emails)
				.Expand(c => c.Labels).Expand(c => c.Notes).Expand(c => c.Phones)
				.SingleOrDefault();

			Assert.IsNotNull(qqqFromDbContact);
			Assert.IsNotNull(qqqFromDbContact.Cases);
			Assert.IsTrue(qqqFromDbContact.Labels.Count == 1);


			Case caseFromDb = client.Cases.Where(c => c.CaseId == newCase.CaseId)
				.Expand(c => c.CommunicationItems).Expand(c => c.Labels).Expand(c => c.Notes)
				.SingleOrDefault();

			Assert.IsNotNull(caseFromDb);
			Assert.IsTrue(caseFromDb.Labels.Count == 1);

		}

		[TestMethod]
		public void ChangeCaseStatusTest()
		{
			//get from db Case with status==Open
			var client = GetRepository();

			string openStatus = CaseStatus.Open.ToString();
			Case openCaseFromDb = client.Cases.Where(c => c.Status == openStatus).FirstOrDefault();
			client.Attach(openCaseFromDb);

			string id = openCaseFromDb.CaseId;
			Assert.IsNotNull(openCaseFromDb);
			//cahnge status to Pending
			openCaseFromDb.Status = CaseStatus.Pending.ToString();

			client.UnitOfWork.Commit();


			client = GetRepository();

			string pendingStatus = CaseStatus.Pending.ToString();
			Case pendingCaseForCheck = client.Cases.Where(c => c.CaseId == id).FirstOrDefault();
			client.Attach(pendingCaseForCheck);
			//status must be pending
			Assert.IsTrue(pendingCaseForCheck.Status == pendingStatus);
			//change status to Resolved
			pendingCaseForCheck.Status = CaseStatus.Resolved.ToString();

			client.UnitOfWork.Commit();


			client = GetRepository();

			string resolvedStatus = CaseStatus.Resolved.ToString();
			Case resolvedCaseForCheck = client.Cases.Where(c => c.CaseId == id).FirstOrDefault();
			//status must be 
			Assert.IsTrue(pendingCaseForCheck.Status == resolvedStatus);



		}

		[TestMethod]
		public void AddEmailToExitingCase()
		{
			var client = GetRepository();

			var caseFromDb = client.Cases.Expand(c => c.CommunicationItems).Expand(c => c.Labels).Expand(c => c.Notes).Expand(c => c.Contact)
				.FirstOrDefault();

			Assert.IsNotNull(caseFromDb);
			Assert.IsNotNull(caseFromDb.Contact);


			caseFromDb.Contact = client.Members.Where(m => m.MemberId == caseFromDb.Contact.MemberId)
				.OfType<Contact>().Expand(c => c.Addresses).Expand(c => c.Emails).Expand(c => c.Labels)
				.Expand(c => c.Notes).Expand(c => c.Phones).SingleOrDefault();

			client.Attach(caseFromDb);

			Email emailToAdd = new Email();
            emailToAdd.Address = "test@test.ru";
			emailToAdd.Type = "Primary";

			caseFromDb.Contact.Emails.Add(emailToAdd);

			client.UnitOfWork.Commit();

			client = GetRepository();

			caseFromDb = client.Cases.Expand(c => c.CommunicationItems).Expand(c => c.Labels).Expand(c => c.Notes).Expand(c => c.Contact)
				.FirstOrDefault();

			Assert.IsNotNull(caseFromDb);
			Assert.IsNotNull(caseFromDb.Contact);


			caseFromDb.Contact = client.Members.Where(m => m.MemberId == caseFromDb.Contact.MemberId)
				.OfType<Contact>().Expand(c => c.Addresses).Expand(c => c.Emails).Expand(c => c.Labels)
				.Expand(c => c.Notes).Expand(c => c.Phones).SingleOrDefault();

			var emailFromDb = caseFromDb.Contact.Emails.Where(e => e.EmailId == emailToAdd.EmailId).SingleOrDefault();

			Assert.IsNotNull(emailFromDb);


		}

		[TestMethod]
		public void AddLabelsToExistingCaseAdContact()
		{
			var client = GetRepository();

			//get case from service
			var caseFromDb = client.Cases.Where(c => c.CaseId == "b24fe32e-50d8-4b5c-876d-7de1b0ce2fe1")
				.Expand(c => c.CommunicationItems).Expand(c => c.Labels).Expand(c => c.Notes)
				.Expand(c => c.Contact)
				.SingleOrDefault();

			Assert.IsNotNull(caseFromDb);
			Assert.IsNotNull(caseFromDb.Contact);

			//get contact from service
			caseFromDb.Contact = client.Members.Where(m => m.MemberId == caseFromDb.Contact.MemberId)
				.OfType<Contact>().Expand(c => c.Addresses).Expand(c => c.Emails).Expand(c => c.Labels)
				.Expand(c => c.Notes).Expand(c => c.Phones).Expand(c => c.Cases).SingleOrDefault();

			client.Attach(caseFromDb);

			caseFromDb.Title = "title";
			caseFromDb.Contact.FullName = "Fullname";
			//caseFromDb.Contact.LastName = "Lastname";


			//get all labels from service
			var labels = client.Labels.ToList();

			var label1 = labels[0];
			var label2 = labels[1];


			caseFromDb.Labels.Add(label1);

			//clear contact's labels
			foreach (var label in caseFromDb.Contact.Labels)
			{
				client.Remove(label);
			}
			caseFromDb.Contact.Labels.Clear();

			//add label to contact
			caseFromDb.Contact.Labels.Add(label2);



			//client.Update(label1);
			//client.Update(label2);

			client.Update(caseFromDb.Contact);
			client.Update(caseFromDb);

			client.UnitOfWork.Commit();

			client = GetRepository();

			var caseForCheck = client.Cases.Where(c => c.CaseId == caseFromDb.CaseId).Expand(c => c.CommunicationItems).Expand(c => c.Labels).Expand(c => c.Notes).Expand(c => c.Contact)
				.SingleOrDefault();

			Assert.IsNotNull(caseForCheck);
			Assert.IsNotNull(caseForCheck.Contact);


			caseForCheck.Contact = client.Members.Where(m => m.MemberId == caseForCheck.Contact.MemberId)
				.OfType<Contact>().Expand(c => c.Addresses).Expand(c => c.Emails).Expand(c => c.Labels)
				.Expand(c => c.Notes).Expand(c => c.Phones).SingleOrDefault();

			Assert.IsTrue(caseForCheck.Labels.Count > 0);
			Assert.IsTrue(caseForCheck.Contact.Labels.Count > 0);

		}

		[TestMethod]
		public void AddEditAndRemoveAddressFromContactTest()
		{
			var client = GetRepository();

			Contact contactFromDb = client.Members.OfType<Contact>().
				Expand(c => c.Addresses).FirstOrDefault();

			client.Attach(contactFromDb);

			Assert.IsNotNull(contactFromDb);


			Address address1 = new Address();
			address1.City = "testCity1";
			address1.CountryCode = "testCountryCode1";
			address1.CountryName = "testCountryName1";
			address1.Line1 = "testLine1_1";
			address1.Line2 = "testLine2_1";
			address1.PostalCode = "testPostalCode1";
			address1.RegionName = "testResgion1";
			address1.StateProvince = "testState1";
			address1.Type = "Primary";

			Address address2 = new Address();
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
			contactFromDb.Addresses.Add(address1);
			contactFromDb.Addresses.Add(address2);

			Assert.IsTrue(contactFromDb.Addresses.Count != 0);

			//save contact
			client.UnitOfWork.Commit();


			client = GetRepository();

			Contact contactForCheck = client.Members.Where(m => m.MemberId == contactFromDb.MemberId).OfType<Contact>().
				Expand(c => c.Addresses).SingleOrDefault();

			client.Attach(contactForCheck);

			Assert.IsNotNull(contactForCheck);
			Assert.IsNotNull(contactForCheck.Addresses);
			Assert.IsTrue(contactForCheck.Addresses.Count >= 2);

			//edit address 1
			Address address1ForCheck = contactForCheck.Addresses.Where(a => a.AddressId == address1.AddressId).SingleOrDefault();
			address1ForCheck.Line1 = "updated line 1_1";
			address1ForCheck.Line2 = "updated line 2_1";
			//edit address 2
			Address address2ForCheck = contactForCheck.Addresses.Where(a => a.AddressId == address2.AddressId).SingleOrDefault();
			address2ForCheck.Line1 = "updated line 1_2";
			address2ForCheck.Line2 = "updated line 2_2";

			client.UnitOfWork.Commit();


			client = GetRepository();

			Contact contactForDelete = client.Members.Where(m => m.MemberId == contactFromDb.MemberId).OfType<Contact>().
				Expand(c => c.Addresses).SingleOrDefault();

			client.Attach(contactForDelete);

			Assert.IsNotNull(contactForDelete);
			Assert.IsNotNull(contactForDelete.Addresses);
			Assert.IsTrue(contactForDelete.Addresses.Count >= 2);

			Address updatedAddress1 = contactForDelete.Addresses.Where(a => a.AddressId == address1ForCheck.AddressId).SingleOrDefault();

			Assert.IsNotNull(updatedAddress1);
			Assert.IsTrue(updatedAddress1.Line1 == "updated line 1_1");
			Assert.IsTrue(updatedAddress1.Line2 == "updated line 2_1");

			Address updatedAddress2 = contactForDelete.Addresses.Where(a => a.AddressId == address2ForCheck.AddressId).SingleOrDefault();

			Assert.IsNotNull(updatedAddress2);
			Assert.IsTrue(updatedAddress2.Line1 == "updated line 1_2");
			Assert.IsTrue(updatedAddress2.Line2 == "updated line 2_2");


			contactForDelete.Addresses.Clear();
			client.Remove(updatedAddress1);
			client.Remove(updatedAddress2);

			client.UnitOfWork.Commit();


			client = GetRepository();

			Contact contactWithRemovedAddresses = client.Members.Where(m => m.MemberId == contactFromDb.MemberId).OfType<Contact>().
				Expand(c => c.Addresses).SingleOrDefault();

			Assert.IsNotNull(contactWithRemovedAddresses);
			Assert.IsNotNull(contactWithRemovedAddresses.Addresses);
			Assert.IsTrue(contactWithRemovedAddresses.Addresses.Count == 1);

		}

		[TestMethod]
		public void AddEditAndRemovePhoneFromContactTest()
		{
			var client = GetRepository();

			Contact contactFromDb = client.Members.OfType<Contact>().
				Expand(c => c.Phones).FirstOrDefault();

			client.Attach(contactFromDb);

			Assert.IsNotNull(contactFromDb);

			//create new phone
			Phone phoneToAdd = new Phone();
			phoneToAdd.Number = "228810";

			contactFromDb.Phones.Add(phoneToAdd);

			client.UnitOfWork.Commit();



			client = GetRepository();

			Contact contactForCheck = client.Members.OfType<Contact>().
				Expand(c => c.Phones).FirstOrDefault();

			client.Attach(contactForCheck);

			Assert.IsNotNull(contactForCheck);

			//get phone for update
			var phoneForCheck = contactForCheck.Phones.Where(p => p.PhoneId == phoneToAdd.PhoneId).SingleOrDefault();
			Assert.IsNotNull(phoneForCheck);

			phoneForCheck.Number = "666";

			client.UnitOfWork.Commit();



			client = GetRepository();

			contactForCheck = client.Members.OfType<Contact>().
				Expand(c => c.Phones).FirstOrDefault();
			Assert.IsNotNull(contactFromDb);

			phoneForCheck = contactForCheck.Phones.Where(p => p.PhoneId == phoneToAdd.PhoneId).SingleOrDefault();
			Assert.IsNotNull(phoneForCheck);
			Assert.IsTrue(phoneForCheck.Number == "666");

		}


		[TestMethod]
		public void AddNotesToCaseAndContactTest()
		{
			var client = GetRepository();


			var contactFromDb = client.Members.OfType<Contact>()
				.Expand(c => c.Notes).Expand(c => c.Cases).FirstOrDefault();

			client.Attach(contactFromDb);

			Note noteForContact = new Note();
			noteForContact.Body = "noteForCase";


			contactFromDb.Notes.Add(noteForContact);
			//noteForContact.ContactId = contactFromDb.MemberId;

			client.UnitOfWork.Commit();

			client = GetRepository();


			var contactForCheck = client.Members.Where(m => m.MemberId == contactFromDb.MemberId).OfType<Contact>()
				.Expand(c => c.Notes).Expand(c => c.Cases).FirstOrDefault();

			Assert.IsNotNull(contactForCheck);
			Assert.IsTrue(contactForCheck.Notes.Count >= 1);
		}

		[TestMethod]
		public void AddCommunicationItemsWithAttachmentsToExistingCase()
		{
			var client = GetRepository();

			Case caseFromDb = client.Cases
				.Expand(c => c.Labels).Expand(c => c.Notes).Expand(c => c.CommunicationItems)
				.FirstOrDefault();

			Assert.IsNotNull(caseFromDb);
			client.Attach(caseFromDb);

			var emailItem = new EmailItem();
			emailItem.AuthorName = "Unknown";
			emailItem.Body = "body of emailItem";
			emailItem.From = "sashagrey@xvideos.com";
			emailItem.Subject = "Subject";
			emailItem.Title = "Hello!!";
			emailItem.To = "et@virtoway.com";


			var phoneItem = new PhoneCallItem();
			phoneItem.AuthorName = "bob marley";
			phoneItem.Body = "body of phoneitem";
			phoneItem.Direction = "Inbound";
			phoneItem.PhoneNumber = "789465123";
			phoneItem.Title = "Hey you!";


			var note = new Note();
			note.AuthorName = "Me";
			note.Body = "noteBody";
			note.Title = "note fo case";

			Attachment attach1 = new Attachment();
			attach1.DisplayName = "Attachment1";

			emailItem.Attachments.Add(attach1);

			caseFromDb.CommunicationItems.Add(emailItem);
			caseFromDb.CommunicationItems.Add(phoneItem);
			caseFromDb.Notes.Add(note);

			client.UnitOfWork.Commit();


			client = GetRepository();

			Case caseForCheck = client.Cases.Where(c => c.CaseId == caseFromDb.CaseId)
				.Expand(c => c.Labels).Expand(c => c.Notes).Expand(c => c.CommunicationItems)
				.SingleOrDefault();

			var phoneItemFromDb = caseForCheck.CommunicationItems.Where(ci => ci.CommunicationItemId == phoneItem.CommunicationItemId)
				.OfType<PhoneCallItem>().SingleOrDefault();

			var emailItemFromDb = caseForCheck.CommunicationItems.Where(ci => ci.CommunicationItemId == emailItem.CommunicationItemId)
				.OfType<EmailItem>().SingleOrDefault();

			var noteFromDb = caseForCheck.Notes.Where(n => n.NoteId == note.NoteId).SingleOrDefault();

			Assert.IsNotNull(caseForCheck);
			Assert.IsNotNull(phoneItemFromDb);
			Assert.IsNotNull(emailItemFromDb);
			Assert.IsNotNull(noteFromDb);

		}

		[TestMethod]
		public void AddAttachmentToCommunicationItemTest()
		{
			var client = GetRepository();


			var caseFromDb = client.Cases.Expand(c => c.CommunicationItems).FirstOrDefault();

			Assert.IsNotNull(caseFromDb);

			client.Attach(caseFromDb);


			var emailItemToAdd = new EmailItem();
			emailItemToAdd.Body = "newEmailItem";

			var phoneItemToAdd = new PhoneCallItem();
			phoneItemToAdd.Body = "newPhoneBody";

			Attachment attachToemail = new Attachment();
			attachToemail.DisplayName = "emailAttach";

			Attachment attachToPhone = new Attachment();
			attachToPhone.DisplayName = "phoneAttach";

			emailItemToAdd.Attachments.Add(attachToemail);
			phoneItemToAdd.Attachments.Add(attachToPhone);

			caseFromDb.CommunicationItems.Add(emailItemToAdd);
			caseFromDb.CommunicationItems.Add(phoneItemToAdd);

			client.UnitOfWork.Commit();


			client = GetRepository();

			var caseForCheck = client.Cases.Where(c => c.CaseId == caseFromDb.CaseId).Expand(c => c.CommunicationItems).SingleOrDefault();

			Assert.IsNotNull(caseForCheck);
			Assert.IsTrue(caseForCheck.CommunicationItems.Count >= 2);

			var emailForCheck = caseFromDb.CommunicationItems.Where(ci => ci.CommunicationItemId == emailItemToAdd.CommunicationItemId).OfType<EmailItem>()
				.SingleOrDefault();

			Assert.IsNotNull(emailForCheck);

			var emailAttachemntForCheck = emailForCheck.Attachments.Where(at => at.AttachmentId == attachToemail.AttachmentId).SingleOrDefault();

			Assert.IsNotNull(emailAttachemntForCheck);


			var phoneForCheck = caseFromDb.CommunicationItems.Where(ci => ci.CommunicationItemId == phoneItemToAdd.CommunicationItemId)
				.OfType<PhoneCallItem>().SingleOrDefault();

			Assert.IsNotNull(phoneForCheck);

			var phoneAttachmentForCheck = phoneForCheck.Attachments.Where(at => at.AttachmentId == attachToPhone.AttachmentId)
				.SingleOrDefault();

			Assert.IsNotNull(phoneAttachmentForCheck);

		}

		[TestMethod]
		public void CreateEmptyKnowledgeBaseGroup()
		{
			var client = GetRepository();

			var baseToDb = new KnowledgeBaseGroup();
			baseToDb.Title = "testGroup";
			baseToDb.Name = "testGroup";

			var parentBaseToDb = new KnowledgeBaseGroup();
			parentBaseToDb.Title = "parentGroup";
			parentBaseToDb.Title = "parentGroup";

			baseToDb.ParentId = parentBaseToDb.KnowledgeBaseGroupId;

			client.Add(parentBaseToDb);
			client.Add(baseToDb);

			client.UnitOfWork.Commit();


		}


		[TestMethod]
		public void AddKnowledgeBaseArticleToGroupTest()
		{
			var client = GetRepository();

			var groupFromDb = client.KnowledgeBaseGroups.Where(g => g.KnowledgeBaseGroupId == "ccb57007-dac9-4d8d-8bf4-47f2c3df3c3a")
				.Expand(g => g.Parent).FirstOrDefault();

			Assert.IsNotNull(groupFromDb);
			Assert.IsNotNull(groupFromDb.Parent);

			client.Attach(groupFromDb);


			var articleToParent = new KnowledgeBaseArticle();
			articleToParent.Title = "articleToParent";
			articleToParent.GroupId = groupFromDb.Parent.KnowledgeBaseGroupId;

			var articleToGroup = new KnowledgeBaseArticle();
			articleToGroup.Title = "artickeToGroup";
			articleToGroup.GroupId = groupFromDb.KnowledgeBaseGroupId;

			client.Add(articleToParent);
			client.Add(articleToGroup);

			client.UnitOfWork.Commit();

		}

		[TestMethod]
		public void CreateCaseTest()
		{
			var client = GetRepository();

			Case caseToAdd = new Case();

			client.Add(caseToAdd);
			client.UnitOfWork.Commit();

			client = GetRepository();

			var caseFromDb = client.Cases.Where(cs => cs.CaseId == caseToAdd.CaseId).SingleOrDefault();

			Assert.IsNotNull(caseFromDb);
		}

		[TestMethod]
		public void CreateContactTest()
		{
			var client = GetRepository();

			Member contactToDb = new Contact();
			//client.Attach(contactToDb);
			client.Add(contactToDb);
			client.UnitOfWork.Commit();

			client = GetRepository();

			var contactFromDb = client.Members.Where(m => m.MemberId == contactToDb.MemberId).OfType<Contact>().SingleOrDefault();

			Assert.IsNotNull(contactFromDb);
		}

		#endregion

		#region KnowledgeBase

		[TestMethod]
		public void KnowledgeBaseGroup_DbOperationTest()
		{
			KnowledgeBaseGroup a = new KnowledgeBaseGroup() { Name = "a", Title = "a" };
			string a_Id = a.KnowledgeBaseGroupId;

			ICustomerRepository repository = GetRepository();
			repository.Attach(a);

			EndTestAction.Add((() =>
			{
				repository.Remove(a);
				repository.UnitOfWork.Commit();
			}));

			// add created group to DB
			repository.Add(a);
			repository.UnitOfWork.Commit();

			//KnowledgeBaseGroup is added to DB correctly?
			KnowledgeBaseGroup a_check = repository.KnowledgeBaseGroups.Where(x => x.KnowledgeBaseGroupId == a_Id).SingleOrDefault();
			Assert.IsTrue(a_check != null && a_check.KnowledgeBaseGroupId == a_Id);

			a.Name = "aa";
			repository.UnitOfWork.Commit();
			a_check = repository.KnowledgeBaseGroups.Where(x => x.KnowledgeBaseGroupId == a_Id).SingleOrDefault();
			Assert.IsTrue(a_check != null && a_check.KnowledgeBaseGroupId == a_Id && a_check.Name == "aa");

			//KnowledgeBaseGroup delete
			repository.Remove(a);
			repository.UnitOfWork.Commit();


			//KnowledgeBaseGroup is delete correctly
			a_check = repository.KnowledgeBaseGroups.Where(x => x.KnowledgeBaseGroupId == a_Id).SingleOrDefault();
			Assert.IsNull(a_check);
		}

		[TestMethod]
		public void KnowledgeBaseGroup_AddChild()
		{
			// create two KnowledgeBaseGroups
			var a = new KnowledgeBaseGroup() { Name = "a", Title = "a" };
			var b = new KnowledgeBaseGroup() { Name = "b", Title = "b" };
			string a_Id = a.KnowledgeBaseGroupId;
			string b_Id = b.KnowledgeBaseGroupId;

			var repository = GetRepository();

			EndTestAction.Add((() =>
			{
				repository.Remove(b);
				repository.Remove(a);
				repository.UnitOfWork.Commit();
			}));


			// add created groups to DB
			repository.Attach(a);
			repository.Add(a);
			repository.UnitOfWork.Commit();


			b.Parent = a;
			b.ParentId = a.KnowledgeBaseGroupId;
			repository.Attach(b);
			repository.Add(b);
			repository.UnitOfWork.Commit();

			//KnowledgeBaseGroup is saved correctly?
			var a_check = repository.KnowledgeBaseGroups.Where(x => x.KnowledgeBaseGroupId == a_Id).SingleOrDefault();
			var b_check = repository.KnowledgeBaseGroups.Where(x => x.KnowledgeBaseGroupId == b_Id).SingleOrDefault();

			Assert.IsTrue(a_check != null && a_check.KnowledgeBaseGroupId == a_Id);
			Assert.IsTrue(b_check != null && b_check.KnowledgeBaseGroupId == b_Id && b_check.ParentId == a_check.KnowledgeBaseGroupId);
		}

		[TestMethod]
		public void KnowledgeBaseGroup_AddArticle()
		{
			// create group and arcicle KnowledgeBaseGroups
			var g = new KnowledgeBaseGroup() { Name = "g", Title = "g" };
			var a = new KnowledgeBaseArticle() { Title = "a", Body = "a" };
			string g_Id = g.KnowledgeBaseGroupId;
			string a_Id = a.KnowledgeBaseArticleId;

			var repository = GetRepository();

			EndTestAction.Add((() =>
			{
				repository.Remove(a);
				repository.Remove(g);
				repository.UnitOfWork.Commit();
			}));


			// add created items to DB
			repository.Attach(g);
			repository.Add(g);
			repository.UnitOfWork.Commit();


			a.GroupId = g.KnowledgeBaseGroupId;
			repository.Attach(a);
			repository.Add(a);
			repository.UnitOfWork.Commit();

			//KnowledgeBaseGroup is saved correctly?
			var a_check = repository.KnowledgeBaseArticles.Where(x => x.KnowledgeBaseArticleId == a_Id).SingleOrDefault();
			var g_check = repository.KnowledgeBaseGroups.Where(x => x.KnowledgeBaseGroupId == g_Id).SingleOrDefault();

			Assert.IsTrue(g_check != null && g_check.KnowledgeBaseGroupId == g_Id);
			Assert.IsTrue(a_check != null && a_check.KnowledgeBaseArticleId == a_Id && a_check.GroupId == g_check.KnowledgeBaseGroupId);
		}

		[TestMethod]
		public void KnowledgeBaseArticle_AddAttachment()
		{
			var client = GetRepository();





			Assert.IsTrue(false);
		}

		public void KnowledgeBaseArticle_DelAttachment()
		{
			Assert.IsTrue(false);
		}

		#endregion


		private ICustomerRepository GetRepository()
		{
			//var retVal = new DSCustomerClient(service.ServiceUri, new CustomerEntityFactory());
            var retVal = new EFCustomerRepository("VirtoCommerce", new CustomerEntityFactory());
			return retVal;
		}

		private Contact CreateContact(string firstName, string lastName, DateTime birthDate, string photoUrl)
		{
			Contact cont = new Contact();


			cont.FullName = firstName+" "+lastName;
			cont.BirthDate = birthDate;

			return cont;

		}


		private Case CreateCase(string number, int priority, string agentId, string agentName, string title, string description, string status, string type)
		{
			Case c = new Case();


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
			Label label = new Label();

			label.Name = name;
			label.ImgUrl = imgUrl;
			label.Description = description;

			return label;
		}

		private Address CreateAddress(string line1, string line2, string city, string countryCode, string countryName, string region, string postalCode, string type)
		{
			Address retVal = new Address
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

			return retVal;
		}
	}
}
