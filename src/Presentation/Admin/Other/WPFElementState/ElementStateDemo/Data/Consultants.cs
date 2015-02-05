using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Virtoway.WPF.State.Demo.Data
{
	class Consultants : ObservableCollection<Consultant>
	{
		public Consultants()
		{
			Add(new Consultant("Tomer", "Shamam", "http://blogs.microsoft.co.il/blogs/Virtowayhamam"));
			Add(new Consultant("Alon", "Flies", "http://blogs.microsoft.co.il/blogs/alon"));
			Add(new Consultant("Emanuel", "Cohen-Yashar", "http://blogs.microsoft.co.il/blogs/applisec"));
			Add(new Consultant("Noam", "King", "http://blogs.microsoft.co.il/blogs/noam"));
		}
	}

	class Consultant
	{
		private string _firstName;
		public string FirstName
		{
			get { return _firstName; }
		}

		private string _lastName;
		public string LastName
		{
			get { return _lastName; }
		}

		private string _blog;
		public string Blog
		{
			get { return _blog; }
		}

		public Consultant(string firstName, string lastName, string blog)
		{
			this._firstName = firstName;
			this._lastName = lastName;
			this._blog = blog;
		}
	}
}
