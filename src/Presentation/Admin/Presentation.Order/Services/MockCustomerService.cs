using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VirtoCommerce.ManagementClient.Order.Services
{
	public class MockCustomerRepository// : ICustomerRepository
	{
		//private List<User> MockUserList;

		//private void PopulateTestUsers()
		//{
		//	if (MockUserList == null)
		//	{
		//		MockUserList = new List<User>();
		//		MockUserList.Add(new User() { MemberId = "1", FirstName = "Bob", LastName = "Marley", FullName = "Bob Marley", Email = "bo@mail.com" });
		//		MockUserList.Add(new User() { MemberId = "2", FirstName = "Curt", LastName = "Cobeyn", FullName = "Curt Cobeyn", Email = "curt@gmail.com" });
		//		MockUserList.Add(new User() { MemberId = "3", FirstName = "Mark", LastName = "Knofler", FullName = "Mark Knofler", Email = "matk@mail.ru" });
		//		MockUserList.Add(new User() { MemberId = "4", FirstName = "Jim", LastName = "Morrison", FullName = "Jim Morrison", Email = "jim@testdomain.com" });
		//		MockUserList.Add(new User() { MemberId = "5", FirstName = "Mark", LastName = "Hammet", FullName = "Mark Hammet", Email = "markhammet@somemail.com" });
		//		MockUserList.Add(new User() { MemberId = "6", FirstName = "Jeason", LastName = "Hefler", FullName = "Jeason Hefler", Email = "jeason@media.com" });
		//		MockUserList.Add(new User() { MemberId = "7", FirstName = "Leila", LastName = "Bella", FullName = "Leila Bella", Email = "leila@fusin.com" });
		//		MockUserList.Add(new User() { MemberId = "8", FirstName = "Carlie", LastName = "Caplin", FullName = "Carlie Caplin", Email = "cc@fusin.com" });
		//		MockUserList.Add(new User() { MemberId = "9", FirstName = "Don", LastName = "Jonson", FullName = "Don Jonson", Email = "don@fusin.com" });
		//		MockUserList.Add(new User() { MemberId = "10", FirstName = "Agent", LastName = "Smith", FullName = "Agent Smith", Email = "Smith@fusin.com" });

		//		MockUserList.Add(new User() { MemberId = "11", FirstName = "Bob 2", LastName = "Marley", FullName = "Bob Marley", Email = "bo@mail.com" });
		//		MockUserList.Add(new User() { MemberId = "12", FirstName = "Curt 2", LastName = "Cobeyn", FullName = "Curt Cobeyn", Email = "curt@gmail.com" });
		//		MockUserList.Add(new User() { MemberId = "13", FirstName = "Mark 2", LastName = "Knofler", FullName = "Mark Knofler", Email = "matk@mail.ru" });
		//		MockUserList.Add(new User() { MemberId = "14", FirstName = "Jim 2", LastName = "Morrison", FullName = "Jim Morrison", Email = "jim@testdomain.com" });
		//		MockUserList.Add(new User() { MemberId = "15", FirstName = "Mark 2", LastName = "Hammet", FullName = "Mark Hammet", Email = "markhammet@somemail.com" });
		//		MockUserList.Add(new User() { MemberId = "16", FirstName = "Jeason 2", LastName = "Hefler", FullName = "Jeason Hefler", Email = "jeason@media.com" });
		//		MockUserList.Add(new User() { MemberId = "17", FirstName = "Leila 1", LastName = "Bella", FullName = "Leila Bella", Email = "leila@fusin.com" });
		//		MockUserList.Add(new User() { MemberId = "18", FirstName = "Carlie 2", LastName = "Caplin", FullName = "Carlie Caplin 2", Email = "cc2@fusin.com" });
		//		MockUserList.Add(new User() { MemberId = "19", FirstName = "Don 2", LastName = "Jonson", FullName = "Don Jonson 2", Email = "don2@fusin.com" });
		//		MockUserList.Add(new User() { MemberId = "20", FirstName = "Agent 2", LastName = "Smith", FullName = "Agent Smith 2", Email = "Smith2@fusin.com" });
		//	}

		//	foreach (var user in MockUserList)
		//	{
		//		user.Addresses.Add(new Address() { City = "Los Angeles", CountryName = "USA", FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, Line1 = "st. 1231", Line2 = "bd. 12", DaytimePhoneNumber = "(495) 906-2131-245" });
		//	}

		//}

		public MockCustomerRepository()
		{
			// PopulateTestUsers();
		}


		#region ICustomerRepository Members

		//public IQueryable<Member> Members
		//{
		//	get { return MockUserList.AsQueryable<Member>(); }
		//}

		//public IQueryable<Address> Addresses
		//{
		//	get { throw new NotImplementedException(); }
		//}

		//public IQueryable<CreditCard> CreditCards
		//{
		//	get { throw new NotImplementedException(); }
		//}

		//public IQueryable<Note> Notes
		//{
		//	get { throw new NotImplementedException(); }
		//}

		#endregion

		#region IRepository Members

		public VirtoCommerce.Foundation.Frameworks.IUnitOfWork UnitOfWork
		{
			get { throw new NotImplementedException(); }
		}

		public void Attach<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

		public void Add<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

		public void Update<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

		public void Remove<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> GetAsQueryable<T>() where T : class
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{

		}

		#endregion
	}
}
