using System;

namespace VirtoCommerce.Foundation.Frameworks.Tagging
{
    public partial class Tag : IConvertible
    {
        public object Value
        {
            get;set;
        }

        public Tag()
        {
        }

        public Tag(object val)
        {
            Value = val;
        }

        public override string ToString()
        {
            return Value != null ? Value.ToString() : string.Empty;
        }

        #region IConvertible

        public TypeCode GetTypeCode()
        {
            return Type.GetTypeCode(Value.GetType());
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(Value, provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(Value, provider);
        }

        [CLSCompliant(false)]
        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(Value, provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(Value, provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(Value, provider);
        }

        [CLSCompliant(false)]
        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToChar(Value, provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToChar(Value, provider);
        }

        [CLSCompliant(false)]
        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(Value, provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(Value, provider);
        }

        [CLSCompliant(false)]
        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(Value, provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(Value, provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(Value, provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(Value, provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(Value, provider);
        }

        public string ToString(IFormatProvider provider)
        {
            return Convert.ToString(Value, provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(Value, conversionType, provider);
        }

        #endregion
    }
}
