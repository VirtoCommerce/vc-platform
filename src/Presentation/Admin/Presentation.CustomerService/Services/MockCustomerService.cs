using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.ManagementClient.Customers.Services
{
	public class MockCustomerService
	{
		static public Random rnd = new Random();

		//public CustomerInfoDto[] GenerateCustomers(int count)
		//{
		//	List<CustomerInfoDto> retVal = new List<CustomerInfoDto>();

		//	//for (int i = 0; i < count; i++)
		//	//{
		//	//	CustomerInfoDto custInfo = new CustomerInfoDto();
		//	//	custInfo.CustomerId = i;
		//	//	custInfo.Contact = new ContactDto()
		//	//	{
		//	//		FirstName = firstName[rnd.Next(firstName.Length)],
		//	//		LastName = lastName[rnd.Next(lastName.Length)],
		//	//		Title = string.Format("Title {0}", i)
		//	//	};

		//	//	List<AddressDto> addresses = new List<AddressDto>();
		//	//	for (int j=0; j<rnd.Next(3);j++)
		//	//	{
		//	//		addresses.Add(GenerateAddressDto());
		//	//	}
		//	//	custInfo.Contact.Addresses = addresses.ToArray();

		//	//	retVal.Add(custInfo);
		//	//}

		//	return retVal.ToArray();
		//}

		//private AddressDto GenerateAddressDto()
		//{
		//	//return new AddressDto()
		//	//{
		//	//	CountryName = countryName[rnd.Next(countryName.Length)],
		//	//	City = cityName[rnd.Next(cityName.Length)]
		//	//};
		//}

		#region Random data
		private string[] firstName = new string[] { "Иван", "Данила", "Славик", "Артур", "Игорь", "Петр" };
		private string[] lastName = new string[] { "Иванов", "Данилеев", "Славиков", "Артуров", "Игорян", "Петридзе" };
		private string[] countryName = new string[] { "Перу", "Кампучия", "Ангола", "Гандурас", "Сомали", "Новая Зеландия" };
		private string[] cityName = new string[] { "Токио", "Рио-де-Жанейро", "Париж", "Осло", "Нью-Васюки", "Мехико" };
		#endregion
		//public CustomerInfoDto GetCustomerInfo(long customerId = 0)
		//{
		//	CustomerInfoDto retval = new CustomerInfoDto();

		//	//retval.Contact = new ContactDto()
		//	//{
		//	//	FirstName = "Иван",
		//	//	LastName = "Иванов",
		//	//	Title="CEO",
		//	//	Addresses = new AddressDto[]
		//	//	{
		//	//		new AddressDto()
		//	//		{
		//	//			City="Гвардейск",
		//	//			CountryName="Russian Federation",
		//	//			PostalCode="238210",
		//	//			Region="39",
		//	//			Line1="ул. Ленина 20"
		//	//		}
		//	//	},
		//	//	Emails = new EmailDto[]
		//	//	{
		//	//		new EmailDto()
		//	//		{
		//	//			Email="pg@virtoway.com"
		//	//		},
		//	//		new EmailDto()
		//	//		{
		//	//			Email="billy@microsoft.com"
		//	//		}
		//	//	},
		//	//	PhoneNumbers = new PhoneNumberDto[]
		//	//	{
		//	//		new PhoneNumberDto()
		//	//		{
		//	//			Number="89110050242"
		//	//		},
		//	//		new PhoneNumberDto()
		//	//		{
		//	//			Number="89524568542"
		//	//		}
		//	//	}


		//	//};

		//	//retval.Items = new LabelDto[]
		//	//{
		//	//	new LabelDto()
		//	//	{
		//	//		Content="Заметка"
		//	//	},
		//	//	new LabelDto()
		//	//	{
		//	//		Content="Напоминание"
		//	//	}
		//	//};

		//	return retval;
		//}


	}
}
