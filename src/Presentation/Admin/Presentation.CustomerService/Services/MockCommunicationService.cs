using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications.Model;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers;

namespace VirtoCommerce.ManagementClient.Customers.Services
{
    /// <example>
    /// var communicationItem = MockCommunicationItemBuilder.Build()
    ///               .WithState(ItemState.New)
    ///               .WithTitle("Заголовок")
    ///               .GetMock();
    /// </example>
    public class MockCommunicationItemBuilder
    {
        static public Random rnd = new Random();

        protected CommunicationItemViewModel _mock;

        public CommunicationItemViewModel GetMock()
        {
            return _mock;
        }

        public MockCommunicationItemBuilder(CommunicationItemViewModel communocationItem)
        {
            _mock = communocationItem;
            var index = rnd.Next(titles.Length);
            _mock.Title = titles[index];
            _mock.Body = bodyes[index];

            index = rnd.Next(users.Length);
            _mock.AuthorName = users[index];
            _mock.AuthorId = users[index];

            index = rnd.Next(dates.Length);
            _mock.Created = dates[index];
            _mock.LastModified = dates[index];
            
            for (int i = 0; i < 5; i++)
            {
                index = rnd.Next(attachments.Length * 2);
                if (index < attachments.Length)
                {
                    _mock.Attachments.Add(new CommunicationAttachment() { FileUrl = attachments[index], State = CommunicationItemState.NotModified, Url=index.ToString() });
                }
            }
        }

        public MockCommunicationItemBuilder WithState(CommunicationItemState state)
        {
            _mock.State = state;
            return this;
        }

        public MockCommunicationItemBuilder WithTitle(string title)
        {
            _mock.Title = title;
            return this;
        }

        public MockCommunicationItemBuilder WithBody(string body)
        {
            _mock.Body = body;
            return this;
        }
        
        public MockCommunicationItemBuilder WithAuthor(string AuthorId, string AuthorName)
        {
            _mock.AuthorName = AuthorName;
            _mock.AuthorId = AuthorId;
            return this;
        }

        public MockCommunicationItemBuilder WithLastModify(DateTime date)
        {
            _mock.LastModified = date;
            return this;
        }

        #region Randon data
        static private string[] titles = new string[] { "Apple 30 GB iPod with Video Playback Black (5th Generation)",
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
        static private string[] users = new string[] { "Коля", "Вася", "Аня", "Света" };
        static private DateTime[] dates = new DateTime[] { DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-2) };
        static private string[] labels = new string[] { "Заметка", "Памятка", "Напоминание", "Будильник" };
        static private string[] attachments = new string[] { "tshort.png", "Рубашка.jpg", "Рваные носки.txt", "Не исправный космолет.pdf" };
        
        #endregion
    }

    #region Channels CommunicationItem Builder

    public class MockCommunicationItemEmailBuilder : MockCommunicationItemBuilder
    {

        public MockCommunicationItemEmailBuilder(CommunicationItemViewModel communocationItem)
            : base(communocationItem)
        {
        }

        

        #region Randon data
        
        static private string[] emails = new string[] { "11111@22222.com", "333333@22222.com", "4444444444@22222.com", "55555555555@22222.com", "6666666666666@22222.com" };
        
        #endregion
    }

    public class MockCommunicationItemNoteBuilder : MockCommunicationItemBuilder
    {

        public MockCommunicationItemNoteBuilder(CommunicationItemViewModel communocationItem)
            : base(communocationItem)
        {
        }

        public static MockCommunicationItemNoteBuilder Build()
        {
            CommunicationItemNoteViewModel communocationItem = new CommunicationItemNoteViewModel();
            return new MockCommunicationItemNoteBuilder(communocationItem);
        }
    }

    public class MockCommunicationItemInboundCallBuilder : MockCommunicationItemBuilder
    {

        public MockCommunicationItemInboundCallBuilder(CommunicationItemViewModel communocationItem)
            : base(communocationItem)
        {
        }

       
        #region Randon data

        static private string[] phoneNumbers = new string[] { "11-11-11", "33-33-33", "4-444-444-44-42", "5555-555-55-55-", "6-666-66-66-666" };
        static private string[] durations = new string[] { "1 мин", "пол часа", "5 сек", "вечность", "мало" };

        #endregion
    }

    #endregion

    /// <example>
    /// var communicationItem = MockCommunicationItemsBuilder.Build()
    ///               .WithEmails(count, state)
    ///               .WithNotes(count, state)
    ///               .WithInboundCall(count, state)
    ///               .GetMock();
    /// </example>
    public class MockCommunicationItemsBuilder
    {
        private List<CommunicationItemViewModel> _mock;

        public List<CommunicationItemViewModel> GetMock()
        {
            return _mock;
        }

        private MockCommunicationItemsBuilder(List<CommunicationItemViewModel> communocationItem)
        {
            _mock = communocationItem;
        }

        public static MockCommunicationItemsBuilder Build()
        {
            return new MockCommunicationItemsBuilder(new List<CommunicationItemViewModel>());
        }
        
       

        public MockCommunicationItemsBuilder WithNotes(int count, CommunicationItemState state)
        {
            for (int i = 0; i < count; i++)
            {
                _mock.Add(MockCommunicationItemNoteBuilder.Build().WithState(state).GetMock());
            }
            return this;
        }

        
    }

}
