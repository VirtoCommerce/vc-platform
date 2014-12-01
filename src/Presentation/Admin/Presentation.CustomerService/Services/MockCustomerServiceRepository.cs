using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ManagementClient.Customers.Services
{
    public class MockCustomerServiceRepository : ICustomerRepository
    {
        private const string NameCustomerInformation = "Customer Information";
        private const string NameCasesInformation = "Case Information";

        private List<Contact> MockContactList;
        private List<Case> MockCaseList;
		private List<CaseRule> MockCaseRuleList; 
		private List<CaseAlert> MockCaseAlertList;
        private List<CasePropertySet> MockCasePropertySetList;
        private List<CasePropertyValue> MockCasePropertyValueList;
        private List<ContactPropertyValue> MockContactPropertyValueList;
        private List<CommunicationItem> MockCommunicationItemList;

        private List<Label> MockLabelList;
        private List<KnowledgeBaseGroup> MockKnowledgeBaseGroupList = new List<KnowledgeBaseGroup>();
        private List<KnowledgeBaseArticle> MockKnowledgeBaseArticleList = new List<KnowledgeBaseArticle>();

        public MockCustomerServiceRepository()
        {
            PopulateTestData();
            PopulateKnowledgeData();
        }


        private void PopulateTestData()
        {
            MockContactList = new List<Contact>();
            MockCaseList = new List<Case>();
            MockCaseRuleList = new List<CaseRule>();
			MockCaseAlertList = new List<CaseAlert>();
            MockCasePropertySetList = new List<CasePropertySet>();
            MockCasePropertyValueList = new List<CasePropertyValue>();
            MockContactPropertyValueList = new List<ContactPropertyValue>();
            MockCommunicationItemList = new List<CommunicationItem>();
            MockLabelList = new List<Label>();

            MockCaseRuleList.Add(new CaseRule { Name = "Rule01", Description = "rule 01 description", Priority = 2 });
            MockCaseRuleList.Add(new CaseRule { Name = "Rule02", Description = "rule 02 description", Priority = 1 });
            MockCaseRuleList.Add(new CaseRule { Name = "Rule03", Description = "rule 03 description", Priority = 4 });

            FillCasePropertySetsContent();

            MockLabelList.Add(new Label() { Name = "LabelName1", Description = "Description1" });
            MockLabelList.Add(new Label() { Name = "LabelName2", Description = "Description2" });

            Random r = new Random();

            for (int i = 0; i < 25; i++)
            {
                Contact contact = new Contact();
                contact.Addresses.Add(new Address() { City = string.Format("Kaliningrad {0}", i), Line1 = "Line1", Type = "Primary" });
                contact.Phones.Add(new Phone() { Number = "891112345678", Type = "Primary" });
                contact.Emails.Add(new Email() { Address = "test@live.ru", Type = "Primary" });
                contact.Notes.Add(new Note() { Title = "Customer Note 1", Body = "Customer Note 1 body" });
                contact.Notes.Add(new Note() { Title = "Customer Note 2", Body = "Customer Note 2 body" });

                contact.BirthDate = new DateTime(1989, 2, 3);
                contact.FullName = string.Format("Иван Иванов {0}", i);
                
                Case c = new Case();
                c.Contact = contact;
                c.Description = string.Format("Description {0}", i);
                c.Priority = r.Next(0, 3);
                c.Title = string.Format("Title {0}", i);
                c.Status = ((CaseStatus)r.Next(0, 3)).ToString();
                c.Channel = ((CaseChannel)r.Next(0, 4)).ToString();
                c.AgentName = "ETatar";
                c.AgentId = Guid.NewGuid().ToString();

                Attachment attachment = new Attachment() { CreatorName = "test creator", DisplayName = "New Attachment", FileType = "txt", FileUrl = "fileUrl" };
                EmailItem emailItem = new EmailItem() { To = "To", From = "From", Title = "Title email" };
                emailItem.Attachments.Add(attachment);


                c.CommunicationItems.Add(emailItem);
                c.CommunicationItems.Add(new PhoneCallItem { PhoneNumber = "79846134", Direction = "Inbound" });
                c.Notes.Add(new Note() { Body = "test Body", Title = "Test title" });

                MockCasePropertyValueList.Add(CreateCasePropertyValue("Agent",false, c.CaseId, null, 0, 0, null, c.AgentId, PropertyValueType.ShortString.GetHashCode(), r.Next(4)));
                MockContactPropertyValueList.Add(CreateContactPropertyValue("Additional note", false, contact.MemberId, null, 0, 0, null, contact.FullName, PropertyValueType.ShortString.GetHashCode(), r.Next(3)));

                contact.Cases.Add(c);
                MockContactList.Add(contact);
                MockCaseList.Add(c);
            }
        }

        private void FillCasePropertySetsContent()
        {
            var item = CreateCasePropertySet(1, NameCustomerInformation);
            item.CaseProperties.Add(CreateCaseProperty("Contact.FirstName", "First Name", 1));
            item.CaseProperties.Add(CreateCaseProperty("Contact.LastName", "Last Name", 2));
            item.CaseProperties.Add(CreateCaseProperty("Contact.BirthDate", "Birthday", 3));
            item.CaseProperties.Add(CreateCaseProperty("Cases.Count", "Total Cases", 4));
            MockCasePropertySetList.Add(item);

            item = CreateCasePropertySet(2, NameCasesInformation);
            item.CaseProperties.Add(CreateCaseProperty("Site", "Referral Site", 1));
            item.CaseProperties.Add(CreateCaseProperty("SLA", "SLA", 2));
            MockCasePropertySetList.Add(item);
        }

        private ContactPropertyValue CreateContactPropertyValue(string Name, bool BooleanValue, string ContactId, DateTime? DateTimeValue, decimal DecimalValue, int IntegerValue, string LongTextValue, string ShortTextValue, int valueType, int Priority)
        {
            return new ContactPropertyValue
            {
                Name = Name,
                BooleanValue = BooleanValue,
                ContactId = ContactId,
                DateTimeValue = DateTimeValue,
                DecimalValue = DecimalValue,
                IntegerValue = IntegerValue,
                LongTextValue = LongTextValue,
                ShortTextValue = ShortTextValue,
                ValueType = valueType,
                Priority = Priority
            };
        }

        private CasePropertyValue CreateCasePropertyValue(string Name, bool BooleanValue, string CaseId, DateTime? DateTimeValue, decimal DecimalValue, int IntegerValue, string LongTextValue, string ShortTextValue, int valueType, int Priority)
        {
            return new CasePropertyValue
            {
                Name = Name,
                BooleanValue = BooleanValue,
                CaseId = CaseId,
                DateTimeValue = DateTimeValue,
                DecimalValue = DecimalValue,
                IntegerValue = IntegerValue,
                LongTextValue = LongTextValue,
                ShortTextValue = ShortTextValue,
                ValueType = valueType,
                Priority = Priority
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

        #region Populate KnowledgeBase data

        private void PopulateKnowledgeData()
        {
            KnowledgeBaseGroup gr = new KnowledgeBaseGroup() { Title = "Группа 1", Name = "Название группы 1" };
            KnowledgeBaseGroup pgr1 = new KnowledgeBaseGroup() { Title = "Подгруппа 1:1", Name = "Название подгруппы 1:1", Parent = gr };
            KnowledgeBaseGroup pgr2 = new KnowledgeBaseGroup() { Title = "Подгруппа 1:2", Name = "Название подгруппы 1:2", Parent = gr };
            MockKnowledgeBaseGroupList.Add(gr);
            MockKnowledgeBaseGroupList.Add(pgr1);
            MockKnowledgeBaseGroupList.Add(pgr2);
            PopulateKnowledgeArticleData(gr, 3);
            PopulateKnowledgeArticleData(pgr1, 10);
            PopulateKnowledgeArticleData(pgr2, 15);

            gr = new KnowledgeBaseGroup() { Title = "Группа 2", Name = "Название группы 2" };
            pgr1 = new KnowledgeBaseGroup() { Title = "Подгруппа 2:1", Name = "Название подгруппы 2:1", Parent = gr };
            pgr2 = new KnowledgeBaseGroup() { Title = "Подгруппа 2:2", Name = "Название подгруппы 2:2", Parent = gr };
            MockKnowledgeBaseGroupList.Add(gr);
            MockKnowledgeBaseGroupList.Add(pgr1);
            MockKnowledgeBaseGroupList.Add(pgr2);
            PopulateKnowledgeArticleData(gr, 3);
            PopulateKnowledgeArticleData(pgr1, 10);
            PopulateKnowledgeArticleData(pgr2, 15);
        }

        private void PopulateKnowledgeArticleData(KnowledgeBaseGroup group, int count)
        {
            for (int i = 0; i < count; i++)
            {
                KnowledgeBaseArticle article = new KnowledgeBaseArticle()
                {
                    Group = group,
                    AuthorName = users[rnd.Next(users.Length)],
                    ModifierName = users[rnd.Next(users.Length)],
                    Created = dates[rnd.Next(dates.Length)],
                    LastModified = dates[rnd.Next(dates.Length)],
                    Title = knowledgeBaseArticleTitles[rnd.Next(knowledgeBaseArticleTitles.Length)],
                    Body = bodyes[rnd.Next(bodyes.Length)]
                };
                MockKnowledgeBaseArticleList.Add(article);
            }
        }

        static public Random rnd = new Random();

        static private string[] users = new string[] { "Коля", "Вася", "Аня", "Света" };
        static private DateTime[] dates = new DateTime[] { DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-2) };
        static private string[] knowledgeBaseArticleTitles = new string[] { "Apple 30 GB iPod with Video Playback Black (5th Generation)",
									   "Sony MDR-IF240RK Wireless Headphone System",
									   "Samsung DVD-HD841 Up-Converting DVD Player", 
									   "Apple QuickTake 200 - Digital camera - compact - 0.35 Mpix - supported memory: SM", 
									   "Samsung YP-T9JAB 4 GB Digital Multimedia Player (Black)", 
									   "EFC-1B1NBECXAR Carrying Case for 10.1", 
										"Sony SGPFLS1 Tablet S LCD Protection Sheet", 
										"Galaxy Tab 8.9 3G Android Honeycomb Tablet (16GB, 850/1900 3G)",
										"Samsung Galaxy Tab Gt-p7500 16gb, Wi-fi + 3g Unlocked"};

        static private string[] bodyes = new string[] { "Жопа - это событие. Полная жопа - это комплекс мероприятий.",
									   "Руководство компании МТС боится сказать Николаю Валуеву, что он больше не будет сниматься в их рекламе.",
									   "Кошки не поддаются дрессировке с использованием метода кнута и пряника. Потому что они не жрут пряники.", 
									   "Путину - 60! А я-то ломал голову, почему хотят увеличить возраст выхода на пенсию!", 
									   "Через три тысячи лет археологи откопают солярий и подумают, что мы жарили людей в наказание.", 
									   "Летом он хотел работать учителем, а зимой - агрономом.", 
										"Говорят, что употребление молока делает человека сильнее. Выпей пять стаканов молока и попробуй отодвинуть стену. У тебя ничего не получится. А теперь выпей пять стаканов водки и... вуаля! - стена движется самостоятельно!", 
										"Рассказал приятель, волей случая получивший краткосрочный контракт в американской деревеньке Силиконовке некоторое время назад. Попал он в небольшую компанию, где основной контингент разработчиков были выходцами с необъятных просторов бывшего СССР. Ну и американцев, естественно, хватало - кто-то же должен был общаться с местным населением и сбывать ему весь тот хлам, который производился бывшими соотечественниками. Как порой в таких случаях бывает, американские имена благополучно переиначивались на русский лад - без какой-либо подоплеки, только ради удобства общения. Так, весьма обаятельная и привлекательная обладательница 4-го номера Кристина была именована Крыской, а непонятно каким образом попавший в команду разработчиков американец Боб - естественно, Бобиком. Все было хорошо и замечательно до тех пор, пока в компанию не устроился еще один Боб. Возникла путаница. Некоторое время их пытались различать как Бобик-старый - Бобик-новый, потом как Бобик первый-второй. Но это было очень громоздко и неудобно. В конце-концов кто-то предложил переименовать второго Бобика в Шарика. Гармония снова воцарилась в отдельно взятой компании...",
										"Неделю назад женился... Сегодня возвращаюсь после работы домой - жена хвастается обновками. На вопрос о стоимости \"столь необходимого нового одеяния\" получаю достойный и аргументированный ответ: \"Какая разница, это ведь моя зарплата!\". Промолчал, вроде как и не поспоришь... Жена, немного подумав, говорит: \"Любимый, прости, я не должна была так говорить... Теперь всё - НАШЕ! И твоя зарплата - тоже моя!\""};
        #endregion

        #region ICustomerServiceRepository

        public IQueryable<Contact> Contacts
        {
            get { return MockContactList.AsQueryable(); }
        }

        public IQueryable<Case> Cases
        {
            get { return MockCaseList.AsQueryable(); }
        }

        public IQueryable<CaseTemplate> CaseTemplates
        {
            get { return null; }
        }

        public IQueryable<CaseRule> CaseRules
        {
            get { return MockCaseRuleList.AsQueryable(); }
        }

		public IQueryable<CaseAlert> CaseAlerts
		{
			get { return MockCaseAlertList.AsQueryable(); }
		}

        public IQueryable<CasePropertySet> CasePropertySets
        {
            get { return MockCasePropertySetList.AsQueryable(); }
        }

        public IQueryable<CasePropertyValue> CasePropertyValues
        {
            get { return MockCasePropertyValueList.AsQueryable(); }
        }

        public IQueryable<ContactPropertyValue> ContactPropertyValues
        {
            get { return MockContactPropertyValueList.AsQueryable(); }
        }

        public IQueryable<Address> Addresses
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<EmailItem> EmailItems
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<PhoneCallItem> PhoneCallItems
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<Organization> Organizations
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<Email> Emails
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<Label> Labels
        {
            get { return MockLabelList.AsQueryable(); }
        }

        public IQueryable<Note> Notes
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<Phone> Phones
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<KnowledgeBaseArticle> KnowledgeBaseArticles
        {
            get { return MockKnowledgeBaseArticleList.AsQueryable(); }
        }

        public IQueryable<KnowledgeBaseGroup> KnowledgeBaseGroups
        {
            get { return MockKnowledgeBaseGroupList.AsQueryable(); }
        }

        MockUnitOfWork _MockUnitOfWorkItem = new MockUnitOfWork();
        public IUnitOfWork UnitOfWork
        {
            get { return _MockUnitOfWorkItem; }
        }

        public void Attach<T>(T item) where T : class
        {
            //throw new NotImplementedException();
        }

        public bool IsAttachedTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }


        public void Add<T>(T item) where T : class
        {
            MockCaseRuleList.Add(item as CaseRule);
        }

        public void Update<T>(T item) where T : class
        {

        }

        public bool IsAttachTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(T item) where T : class
        {

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


        #region IDisposableMembers

        public void Dispose()
        {

        }

        #endregion


        public IQueryable<Member> Members
        {
            get { return MockContactList.AsQueryable(); }
        }

        public IQueryable<CommunicationItem> CommunicationItems
        {
            get { return MockCommunicationItemList.AsQueryable(); }
        }


        public IQueryable<Attachment> Attachments
        {
            get { return null; }
        }


        public IQueryable<MemberRelation> MemberRelations
        {
            get { throw new NotImplementedException(); }
        }


        public IQueryable<CaseCC> CaseCcs
        {
            get { throw new NotImplementedException(); }
        }
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
