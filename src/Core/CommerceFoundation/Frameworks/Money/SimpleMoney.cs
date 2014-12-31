/*
 * This SimpleMoney class gives you the ability to work with money of a single currency.
 * It is a simpler version of the Money class in this Assembly - Supports all features except multiple currencies.
 * Money is not Derived from SimpleMoney, because of limited re-use due to the absence of virtual operators.
 * Polymorphism is not applicable as one should only use the one or the other in a project.
 * Performance was also a critical measure, so storage and functional redundancy had to be minimised.
 * 
 * NB!
 * This Money class uses double to store the Money value internally.
 * Only 15 decimal digits of accuracy are guaranteed! (16 if the first digit is smaller than 9)
 * It should be fairly simple to replace the internal double with a decimal if this is not sufficient and performance is not an issue.
 */

using System;
using System.Globalization;

namespace VirtoCommerce.Foundation.Money
{
	public class SimpleMoney : IComparable<SimpleMoney>, IEquatable<SimpleMoney>, IComparable
	{
		private double amount;
		private static readonly CurrencyCodes currencyCode = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), new RegionInfo(CultureInfo.CurrentCulture.LCID).ISOCurrencySymbol);

		#region Constructors

		public SimpleMoney() : this(0d) { }
		public SimpleMoney(long amount) : this((double)amount) { }
		public SimpleMoney(decimal amount) : this((double)amount) { }
		public SimpleMoney(double amount)
		{
			this.amount = amount;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Accesses the internal representation of the value of the Money
		/// </summary>
		/// <returns>A decimal with the internal amount stored for this Money.</returns>
		public double InternalAmount
		{
			get { return amount; }
			set { amount = value; }
		}

		/// <summary>
		/// Rounds the amount to the number of significant decimal digits
		/// of the associated currency using MidpointRounding.AwayFromZero.
		/// </summary>
		/// <returns>A decimal with the amount rounded to the significant number of decimal digits.</returns>
		public decimal Amount
		{
			get
			{
				return Decimal.Round((Decimal)amount, this.DecimalDigits, MidpointRounding.AwayFromZero);
			}
		}

		/// <summary>
		/// Truncates the amount to the number of significant decimal digits
		/// of the associated currency.
		/// </summary>
		/// <returns>A decimal with the amount truncated to the significant number of decimal digits.</returns>
		public decimal TruncatedAmount
		{
			get
			{
				return (decimal)((long)Math.Truncate(amount * this.DecimalDigits)) / this.DecimalDigits;
			}
		}

		public string CurrencyCode
		{
			get { return Currency.Get(currencyCode).Code; }
		}

		public string CurrencySymbol
		{
			get { return Currency.Get(currencyCode).Symbol; }
		}

		public string CurrencyName
		{
			get { return Currency.Get(currencyCode).EnglishName; }
		}

		/// <summary>
		/// Gets the number of decimal digits for the associated currency.
		/// </summary>
		/// <returns>An int containing the number of decimal digits.</returns>
		public int DecimalDigits
		{
			get { return Currency.Get(currencyCode).NumberFormat.CurrencyDecimalDigits; }
		}

		/// <summary>
		/// Gets the CurrentCulture from the CultureInfo object and creates a CurrencyCodes enum object.
		/// </summary>
		/// <returns>The CurrencyCodes enum of the current locale.</returns>
		public static CurrencyCodes LocalCurrencyCode
		{
			get
			{
				return currencyCode;
			}
		}

		#endregion

		#region Money Operators

		public override int GetHashCode()
		{
			return Amount.GetHashCode();
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is SimpleMoney))
			{
				throw new ArgumentException("Argument must be SimpleMoney");
			}
			return CompareTo((SimpleMoney)obj);
		}
		public int CompareTo(SimpleMoney other)
		{
			if (this < other)
			{
				return -1;
			}
			if (this > other)
			{
				return 1;
			}
			return 0;
		}

		public override bool Equals(object obj)
		{
			return (obj is SimpleMoney) && Equals((SimpleMoney)obj);
		}
		public bool Equals(SimpleMoney other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			return Amount == other.Amount;
		}

		public static bool operator ==(SimpleMoney first, SimpleMoney second)
		{
			if (object.ReferenceEquals(first, second)) return true;
			if (object.ReferenceEquals(first, null) || object.ReferenceEquals(second, null)) return false;
			return first.Amount == second.Amount;
		}
		public static bool operator !=(SimpleMoney first, SimpleMoney second)
		{
			return !first.Equals(second);
		}

		public static bool operator >(SimpleMoney first, SimpleMoney second)
		{
			return first.Amount > second.Amount;
		}

		public static bool operator >=(SimpleMoney first, SimpleMoney second)
		{
			return first.Amount >= second.Amount;
		}

		public static bool operator <=(SimpleMoney first, SimpleMoney second)
		{
			return first.Amount <= second.Amount;
		}

		public static bool operator <(SimpleMoney first, SimpleMoney second)
		{
			return first.Amount < second.Amount;
		}

		public static SimpleMoney operator +(SimpleMoney first, SimpleMoney second)
		{
			return new SimpleMoney(first.amount + second.amount);
		}

		public static SimpleMoney operator -(SimpleMoney first, SimpleMoney second)
		{
			return new SimpleMoney(first.amount - second.amount);
		}

		public static SimpleMoney operator *(SimpleMoney first, SimpleMoney second)
		{
			return new SimpleMoney(first.amount * second.amount);
		}

		public static SimpleMoney operator /(SimpleMoney first, SimpleMoney second)
		{
			return new SimpleMoney(first.amount / second.amount);
		}

		#endregion

		#region Cast Operators

		public static implicit operator SimpleMoney(long amount)
		{
			return new SimpleMoney(amount);
		}

		public static implicit operator SimpleMoney(decimal amount)
		{
			return new SimpleMoney(amount);
		}

		public static implicit operator SimpleMoney(double amount)
		{
			return new SimpleMoney(amount);
		}

		public static bool operator ==(SimpleMoney money, long value)
		{
			if (object.ReferenceEquals(money, null) || object.ReferenceEquals(value, null)) return false;
			return (money.Amount == (decimal)value);
		}
		public static bool operator !=(SimpleMoney money, long value)
		{
			return !(money == value);
		}

		public static bool operator ==(SimpleMoney money, decimal value)
		{
			if (object.ReferenceEquals(money, null) || object.ReferenceEquals(value, null)) return false;
			return (money.Amount == value);
		}
		public static bool operator !=(SimpleMoney money, decimal value)
		{
			return !(money == value);
		}

		public static bool operator ==(SimpleMoney money, double value)
		{
			if (object.ReferenceEquals(money, null) || object.ReferenceEquals(value, null)) return false;
			return (money.Amount == (decimal)value);
		}
		public static bool operator !=(SimpleMoney money, double value)
		{
			return !(money == value);
		}

		public static SimpleMoney operator +(SimpleMoney money, long value)
		{
			return money + (double)value;
		}
		public static SimpleMoney operator +(SimpleMoney money, decimal value)
		{
			return money + (double)value;
		}
		public static SimpleMoney operator +(SimpleMoney money, double value)
		{
			if (money == null) throw new ArgumentNullException("money");
			return new SimpleMoney(money.amount + value);
		}

		public static SimpleMoney operator -(SimpleMoney money, long value)
		{
			return money - (double)value;
		}
		public static SimpleMoney operator -(SimpleMoney money, decimal value)
		{
			return money - (double)value;
		}
		public static SimpleMoney operator -(SimpleMoney money, double value)
		{
			if (money == null) throw new ArgumentNullException("money");
			return new SimpleMoney(money.amount - value);
		}

		public static SimpleMoney operator *(SimpleMoney money, long value)
		{
			return money * (double)value;
		}
		public static SimpleMoney operator *(SimpleMoney money, decimal value)
		{
			return money * (double)value;
		}
		public static SimpleMoney operator *(SimpleMoney money, double value)
		{
			if (money == null) throw new ArgumentNullException("money");
			return new SimpleMoney(money.amount * value);
		}

		public static SimpleMoney operator /(SimpleMoney money, long value)
		{
			return money / (double)value;
		}
		public static SimpleMoney operator /(SimpleMoney money, decimal value)
		{
			return money / (double)value;
		}
		public static SimpleMoney operator /(SimpleMoney money, double value)
		{
			if (money == null) throw new ArgumentNullException("money");
			return new SimpleMoney(money.amount / value);
		}

		#endregion

		#region Functions

		public SimpleMoney Copy()
		{
			return new SimpleMoney(Amount);
		}

		public SimpleMoney Clone()
		{
			return new SimpleMoney();
		}

		public string ToString(string format = "C")
		{
			return Amount.ToString(format, Currency.Get(currencyCode).NumberFormat);
		}

		/// <summary>
		/// Evenly distributes the amount over n parts, resolving remainders that occur due to rounding 
		/// errors, thereby garuanteeing the postcondition: result->sum(r|r.amount) = this.amount and
		/// x elements in result are greater than at least one of the other elements, where x = amount mod n.
		/// </summary>
		/// <param name="n">Number of parts over which the amount is to be distibuted.</param>
		/// <returns>Array with distributed Money amounts.</returns>
		public SimpleMoney[] Allocate(int n)
		{
			double cents = Math.Pow(10, this.DecimalDigits);
			double lowResult = ((long)Math.Truncate((double)amount / n * cents)) / cents;
			double highResult = lowResult + 1.0d / cents;
			SimpleMoney[] results = new SimpleMoney[n];
			int remainder = (int)(((double)amount * cents) % n);
			for (int i = 0; i < remainder; i++)
				results[i] = new SimpleMoney((Decimal)highResult);
			for (int i = remainder; i < n; i++)
				results[i] = new SimpleMoney((Decimal)lowResult);
			return results;
		}

		#endregion
	}
}
