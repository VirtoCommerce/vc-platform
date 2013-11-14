using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.Model
{
	public class PaymentModel
	{
		public PaymentModel()
		{
			NewPayment = new CreditCardPayment();
		}

		public CreditCardPayment NewPayment { get; private set; }

		public ListModelInt[] Months { get; set; }
		public ListModelInt[] Years { get; set; }
		public ListModel[] CardTypes { get; set; }

		public bool Validate()
		{
			var result = NewPayment.Validate();
			//var result = !string.IsNullOrEmpty(NewPayment.CustomerName)
			//			 && !string.IsNullOrEmpty(CardType)
			//			 && !string.IsNullOrEmpty(CardNumber)
			//			 && ExpirationMonth > 0 && ExpirationMonth <= 12
			//			 && ExpirationYear >= DateTime.Today.Year
			//			 && !string.IsNullOrEmpty(CardVerificationNumber);
			return result;
		}
	}

	public class ListModel
	{
		public ListModel(string name, string val)
		{
			Name = name;
			Value = val;
		}

		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class ListModelInt
	{
		public ListModelInt(string name, int val)
		{
			Name = name;
			Value = val;
		}

		public string Name { get; set; }
		public int Value { get; set; }
	}
}
