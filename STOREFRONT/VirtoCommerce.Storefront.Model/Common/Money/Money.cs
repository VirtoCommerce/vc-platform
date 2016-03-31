/*
 * This Money class gives you the ability to work with money of multiple currencies
 * as if it were numbers.
 * It looks and behaves like a decimal.
 * Super light: Only a 64bit double and 16bit int are used to persist an instance.
 * Super fast: Access to the internal double value for fast calculations.
 * Currency codes are used to get everything from the MS Globalization classes.
 * All lookups happen from a singleton dictionary.
 * Formatting and significant digits are automatically handled.
 * An allocation function also allows even distribution of Money.
 * 
 * References:
 * Martin Fowler patterns
 * Making Money with C# : http://www.lindelauf.com/?p=17
 * http://www.codeproject.com/Articles/28244/A-Money-type-for-the-CLR?msg=3679755
 * A few other articles on the web around representing money types
 * http://en.wikipedia.org/wiki/ISO_4217
 * http://www.currency-iso.org/iso_index/iso_tables/iso_tables_a1.htm
 * 
 * NB!
 * Although the .Amount property wraps the class as Decimal, this Money class uses double to store the Money value internally.
 * Only 15 decimal digits of accuracy are guaranteed! (16 if the first digit is smaller than 9)
 * It should be fairly simple to replace the internal double with a decimal if this is not sufficient and performance is not an issue.
 * Decimal operations are MUCH slower than double (average of 15x)
 * http://stackoverflow.com/questions/366852/c-sharp-decimal-datatype-performance
 * Use the .InternalAmount property to get to the double member.
 * All the Money comparison operators use the Decimal wrapper with significant digits for the currency.
 * All the Money arithmatic (+-/*) operators use the internal double value.
 */

using System;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class Money : IComparable<Money>, IEquatable<Money>, IComparable, IConvertible<Money>
    {
        private Currency _currency;
        private decimal _amount;

        #region Constructors
        public Money()
        {
        }

        public Money(Currency currency)
            : this(0m, currency)
        {
        }

        public Money(double amount, Currency currency)
            : this((decimal)amount, currency)
        {
        }

        public Money(decimal amount, Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }
            _currency = currency;
            _amount = amount;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Accesses the internal representation of the value of the Money
        /// </summary>
        /// <returns>A decimal with the internal amount stored for this Money.</returns>
        public decimal InternalAmount
        {
            get { return _amount; }
            set { _amount = value; }
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
                return decimal.Round(_amount, DecimalDigits, MidpointRounding.AwayFromZero);
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
                return (decimal)((long)Math.Truncate(_amount * DecimalDigits)) / DecimalDigits;
            }
        }
        public string FormatedAmount
        {
            get
            {
                return ToString();
            }
        }

        public Currency Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }


        /// <summary>
        /// Gets the number of decimal digits for the associated currency.
        /// </summary>
        /// <returns>An int containing the number of decimal digits.</returns>
        public int DecimalDigits
        {
            get { return _currency.NumberFormat.CurrencyDecimalDigits; }
        }
        #endregion

        #region Money Operators

        public override int GetHashCode()
        {
            return Amount.GetHashCode() ^ _currency.Code.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is Money) && Equals((Money)obj);
        }

        public bool Equals(Money other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return ((Currency.Equals(other.Currency)) && (Amount == other.Amount));
        }

        public static bool operator ==(Money first, Money second)
        {
            if (ReferenceEquals(first, second)) return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null)) return false;
            return first.Equals(second);
        }

        public static bool operator !=(Money first, Money second)
        {
            return !(first == second);
        }

        public static bool operator >(Money first, Money second)
        {
            return first.Amount > second.ConvertTo(first.Currency).Amount
              && second.Amount < first.ConvertTo(second.Currency).Amount;
        }

        public static bool operator >=(Money first, Money second)
        {
            return first.Amount >= second.ConvertTo(first.Currency).Amount
              && second.Amount <= first.ConvertTo(second.Currency).Amount;
        }

        public static bool operator <=(Money first, Money second)
        {
            return first.Amount <= second.ConvertTo(first.Currency).Amount
              && second.Amount >= first.ConvertTo(second.Currency).Amount;
        }

        public static bool operator <(Money first, Money second)
        {
            return first.Amount < second.ConvertTo(first.Currency).Amount
              && second.Amount > first.ConvertTo(second.Currency).Amount;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is Money))
                throw new ArgumentException("Argument must be Money");
            return CompareTo((Money)obj);
        }

        public int CompareTo(Money other)
        {
            if (this < other)
                return -1;
            if (this > other)
                return 1;
            return 0;
        }

        public static Money operator +(Money first, Money second)
        {
            return new Money(first.Amount + second.ConvertTo(first.Currency).Amount, first.Currency);
        }

        public static Money operator -(Money first, Money second)
        {
            return new Money(first.Amount - second.ConvertTo(first.Currency).Amount, first.Currency);
        }

        public static Money operator *(Money first, Money second)
        {
            return new Money(first.Amount * second.ConvertTo(first.Currency).Amount, first.Currency);
        }

        public static Money operator /(Money first, Money second)
        {
            return new Money(first.Amount / second.ConvertTo(first.Currency).Amount, first.Currency);
        }

        #endregion

        #region Cast Operators

        public static bool operator ==(Money money, long value)
        {
            if (ReferenceEquals(money, null))
                return false;
            return (money.Amount == value);
        }
        public static bool operator !=(Money money, long value)
        {
            return !(money == value);
        }

        public static bool operator ==(Money money, decimal value)
        {
            if (ReferenceEquals(money, null))
                return false;
            return (money.Amount == value);
        }
        public static bool operator !=(Money money, decimal value)
        {
            return !(money == value);
        }

        public static bool operator ==(Money money, double value)
        {
            if (ReferenceEquals(money, null))
                return false;
            return (money.Amount == (decimal)value);
        }
        public static bool operator !=(Money money, double value)
        {
            return !(money == value);
        }

        public static Money operator +(Money money, long value)
        {
            return money + (decimal)value;
        }
        public static Money operator +(Money money, double value)
        {
            return money + (decimal)value;
        }
        public static Money operator +(Money money, decimal value)
        {
            if (money == null)
                throw new ArgumentNullException("money");
            return new Money(money.Amount + value, money.Currency);
        }

        public static Money operator -(Money money, long value)
        {
            return money - (decimal)value;
        }
        public static Money operator -(Money money, double value)
        {
            return money - (decimal)value;
        }
        public static Money operator -(Money money, decimal value)
        {
            if (money == null)
                throw new ArgumentNullException("money");
            return new Money(money.Amount - value, money.Currency);
        }

        public static Money operator *(Money money, long value)
        {
            return money * (decimal)value;
        }
        public static Money operator *(Money money, double value)
        {
            return money * (decimal)value;
        }
        public static Money operator *(Money money, decimal value)
        {
            if (money == null)
                throw new ArgumentNullException("money");
            return new Money(money.Amount * value, money.Currency);
        }

        public static Money operator /(Money money, long value)
        {
            return money / (decimal)value;
        }
        public static Money operator /(Money money, double value)
        {
            return money / (decimal)value;
        }

        public static Money operator /(Money money, decimal value)
        {
            if (money == null)
                throw new ArgumentNullException("money");
            return new Money(money.Amount / value, money.Currency);
        }

        #endregion

        #region Functions

        public override string ToString()
        {
            var retVal = Amount.ToString();
            if (Currency != null)
            {
                if (!string.IsNullOrEmpty(Currency.CustomFormatting))
                {
                    retVal = Amount.ToString(Currency.CustomFormatting);
                }
                else if (Currency.NumberFormat != null)
                {
                    retVal = Amount.ToString("C", Currency.NumberFormat);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Evenly distributes the amount over n parts, resolving remainders that occur due to rounding 
        /// errors, thereby garuanteeing the postcondition: result->sum(r|r.amount) = this.amount and
        /// x elements in result are greater than at least one of the other elements, where x = amount mod n.
        /// </summary>
        /// <param name="n">Number of parts over which the amount is to be distibuted.</param>
        /// <returns>Array with distributed Money amounts.</returns>
        public Money[] Allocate(int n)
        {
            var cents = Math.Pow(10, DecimalDigits);
            var lowResult = ((long)Math.Truncate((double)_amount / n * cents)) / cents;
            var highResult = lowResult + 1.0d / cents;
            var results = new Money[n];
            var remainder = (int)(((double)_amount * cents) % n);
            for (var i = 0; i < remainder; i++)
                results[i] = new Money((decimal)highResult, _currency);
            for (var i = remainder; i < n; i++)
                results[i] = new Money((decimal)lowResult, _currency);
            return results;
        }

        #endregion

        #region IConvertible<Money> Members
        public Money ConvertTo(Currency toCurrency)
        {
            if (Currency == toCurrency)
                return this;
            return new Money((_amount * Currency.ExchangeRate) / toCurrency.ExchangeRate, toCurrency);
        }
        #endregion
    }
}
