using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.ManagementClient.Customers.Services
{
    public class MockKnowlageBaseService
    {

        #region IKnowlageBaseService Members

		//public KnowledgeBaseSearchResultDto SearchKnowledgeItemByCriteria(string criteria)
		//{
		//	KnowledgeBaseSearchResultDto retVal = new KnowledgeBaseSearchResultDto();

		//	if (String.IsNullOrEmpty(criteria))
		//	{
		//		retVal.KnowledgeItems = new KnowledgeItemDto[] { };
		//		retVal.TotalResults = 0;
		//	}
		//	else
		//	{
		//		var rnd = new Random();
		//		int count = rnd.Next(50);
		//		retVal.KnowledgeItems = MockKnowledgeBuilder.GetRandom(count).ToArray();
		//		retVal.TotalResults = retVal.KnowledgeItems.Length;
		//	}
		//	System.Threading.Thread.Sleep(2000);
		//	return retVal;
		//}

        #endregion

    }

    /// <example>
    /// var knowledgeItemDto = MockKnowledgeBuilder.Build()
    ///               .WithBody("Some text")
    ///               .WithTitle("Заголовок")
    ///               .GetMock();
    ///               
    ///  List<KnowledgeItemDto> knoledges =  MockKnowledgeBuilder.GetRandom(count) ;           
    /// </example>
    public class MockKnowledgeBuilder
    {
		//static public Random rnd = new Random();

		//protected KnowledgeItemDto _mock;

		//public KnowledgeItemDto GetMock()
		//{
		//	return _mock;
		//}

		//public MockKnowledgeBuilder(KnowledgeItemDto knowledgeItemDto)
		//{
		//	_mock = InitRandomMock(knowledgeItemDto);
		//}

		//static private KnowledgeItemDto InitRandomMock(KnowledgeItemDto mock)
		//{
		//	var index = rnd.Next(titles.Length);
		//	mock.Title = titles[index];
		//	mock.Body = bodyes[index];
		//	return mock;
		//}

		//public static MockKnowledgeBuilder Build()
		//{
		//	KnowledgeItemDto knowledgeItemDto = new KnowledgeItemDto();
		//	return new MockKnowledgeBuilder(knowledgeItemDto);
		//}

		//public static List<KnowledgeItemDto> GetRandom(int count)
		//{
		//	List<KnowledgeItemDto> items = new List<KnowledgeItemDto>();
		//	for(int i = 0; i<count;i++)
		//	{
		//	items.Add(InitRandomMock(new KnowledgeItemDto()));
		//	}
		//	return items;
		//}

		//#region BuildWith
        
		//public MockKnowledgeBuilder WithState(string title)
		//{
		//	_mock.Title = title;
		//	return this;
		//}

		//public MockKnowledgeBuilder WithTitle(string body)
		//{
		//	_mock.Body = body;
		//	return this;
		//}

		//#endregion

		//#region Randon data
		//static private string[] titles = new string[] { "Apple 30 GB iPod with Video Playback Black (5th Generation)",
		//							   "Sony MDR-IF240RK Wireless Headphone System",
		//							   "Samsung DVD-HD841 Up-Converting DVD Player", 
		//							   "Apple QuickTake 200 - Digital camera - compact - 0.35 Mpix - supported memory: SM", 
		//							   "Samsung YP-T9JAB 4 GB Digital Multimedia Player (Black)", 
		//							   "EFC-1B1NBECXAR Carrying Case for 10.1", 
		//								"Sony SGPFLS1 Tablet S LCD Protection Sheet", 
		//								"Galaxy Tab 8.9 3G Android Honeycomb Tablet (16GB, 850/1900 3G)",
		//								"Samsung Galaxy Tab Gt-p7500 16gb, Wi-fi + 3g Unlocked"};
		//static private string[] bodyes = new string[] { "Жопа - это событие. Полная жопа - это комплекс мероприятий.",
		//							   "Руководство компании МТС боится сказать Николаю Валуеву, что он больше не будет сниматься в их рекламе.",
		//							   "Кошки не поддаются дрессировке с использованием метода кнута и пряника. Потому что они не жрут пряники.", 
		//							   "Путину - 60! А я-то ломал голову, почему хотят увеличить возраст выхода на пенсию!", 
		//							   "Через три тысячи лет археологи откопают солярий и подумают, что мы жарили людей в наказание.", 
		//							   "Летом он хотел работать учителем, а зимой - агрономом.", 
		//								"Говорят, что употребление молока делает человека сильнее. Выпей пять стаканов молока и попробуй отодвинуть стену. У тебя ничего не получится. А теперь выпей пять стаканов водки и... вуаля! - стена движется самостоятельно!", 
		//								"Рассказал приятель, волей случая получивший краткосрочный контракт в американской деревеньке Силиконовке некоторое время назад. Попал он в небольшую компанию, где основной контингент разработчиков были выходцами с необъятных просторов бывшего СССР. Ну и американцев, естественно, хватало - кто-то же должен был общаться с местным населением и сбывать ему весь тот хлам, который производился бывшими соотечественниками. Как порой в таких случаях бывает, американские имена благополучно переиначивались на русский лад - без какой-либо подоплеки, только ради удобства общения. Так, весьма обаятельная и привлекательная обладательница 4-го номера Кристина была именована Крыской, а непонятно каким образом попавший в команду разработчиков американец Боб - естественно, Бобиком. Все было хорошо и замечательно до тех пор, пока в компанию не устроился еще один Боб. Возникла путаница. Некоторое время их пытались различать как Бобик-старый - Бобик-новый, потом как Бобик первый-второй. Но это было очень громоздко и неудобно. В конце-концов кто-то предложил переименовать второго Бобика в Шарика. Гармония снова воцарилась в отдельно взятой компании...",
		//								"Неделю назад женился... Сегодня возвращаюсь после работы домой - жена хвастается обновками. На вопрос о стоимости \"столь необходимого нового одеяния\" получаю достойный и аргументированный ответ: \"Какая разница, это ведь моя зарплата!\". Промолчал, вроде как и не поспоришь... Жена, немного подумав, говорит: \"Любимый, прости, я не должна была так говорить... Теперь всё - НАШЕ! И твоя зарплата - тоже моя!\""};
		//static private string[] users = new string[] { "Коля", "Вася", "Аня", "Света" };
		//static private DateTime[] dates = new DateTime[] { DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-2) };
		//static private string[] labels = new string[] { "Заметка", "Памятка", "Напоминание", "Будильник" };
		//static private string[] attachments = new string[] { "tshort.png", "Рубашка.jpg", "Рваные носки.txt", "Не исправный космолет.pdf" };
        
		//#endregion
    }

}
