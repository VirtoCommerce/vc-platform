using System.Collections.Generic;
using System.Linq;

namespace DiBs.Managers
{
    public class Iso4217Lookup
    {
        // http://en.wikipedia.org/wiki/ISO_4217
        private static readonly List<Iso4217Definition> DefinitionCollection = new List<Iso4217Definition> {
        new Iso4217Definition("AED", 784, 2),
        new Iso4217Definition("AFN", 971, 2),
        new Iso4217Definition("ALL", 8, 2),
        new Iso4217Definition("AMD", 51, 2),
        new Iso4217Definition("ANG", 532, 2),
        new Iso4217Definition("AOA", 973, 2),
        new Iso4217Definition("ARS", 32, 2),
        new Iso4217Definition("AUD", 36, 2),
        new Iso4217Definition("AWG", 533, 2),
        new Iso4217Definition("AZN", 944, 2),
        new Iso4217Definition("BAM", 977, 2),
        new Iso4217Definition("BBD", 52, 2),
        new Iso4217Definition("BDT", 50, 2),
        new Iso4217Definition("BGN", 975, 2),
        new Iso4217Definition("BHD", 48, 3),
        new Iso4217Definition("BIF", 108, 0),
        new Iso4217Definition("BMD", 60, 2),
        new Iso4217Definition("BND", 96, 2),
        new Iso4217Definition("BOB", 68, 2),
        new Iso4217Definition("BOV", 984, 2),
        new Iso4217Definition("BRL", 986, 2),
        new Iso4217Definition("BSD", 44, 2),
        new Iso4217Definition("BTN", 64, 2),
        new Iso4217Definition("BWP", 72, 2),
        new Iso4217Definition("BYR", 974, 0),
        new Iso4217Definition("BZD", 84, 2),
        new Iso4217Definition("CAD", 124, 2),
        new Iso4217Definition("CDF", 976, 2),
        new Iso4217Definition("CHE", 947, 2),
        new Iso4217Definition("CHF", 756, 2),
        new Iso4217Definition("CHW", 948, 2),
        new Iso4217Definition("CLF", 990, 0),
        new Iso4217Definition("CLP", 152, 0),
        new Iso4217Definition("CNY", 156, 2),
        new Iso4217Definition("COP", 170, 2),
        new Iso4217Definition("COU", 970, 2),
        new Iso4217Definition("CRC", 188, 2),
        new Iso4217Definition("CUC", 931, 2),
        new Iso4217Definition("CUP", 192, 2),
        new Iso4217Definition("CVE", 132, 0),
        new Iso4217Definition("CZK", 203, 2),
        new Iso4217Definition("DJF", 262, 0),
        new Iso4217Definition("DKK", 208, 2),
        new Iso4217Definition("DOP", 214, 2),
        new Iso4217Definition("DZD", 12, 2),
        new Iso4217Definition("EGP", 818, 2),
        new Iso4217Definition("ERN", 232, 2),
        new Iso4217Definition("ETB", 230, 2),
        new Iso4217Definition("EUR", 978, 2),
        new Iso4217Definition("FJD", 242, 2),
        new Iso4217Definition("FKP", 238, 2),
        new Iso4217Definition("GBP", 826, 2),
        new Iso4217Definition("GEL", 981, 2),
        new Iso4217Definition("GHS", 936, 2),
        new Iso4217Definition("GIP", 292, 2),
        new Iso4217Definition("GMD", 270, 2),
        new Iso4217Definition("GNF", 324, 0),
        new Iso4217Definition("GTQ", 320, 2),
        new Iso4217Definition("GYD", 328, 2),
        new Iso4217Definition("HKD", 344, 2),
        new Iso4217Definition("HNL", 340, 2),
        new Iso4217Definition("HRK", 191, 2),
        new Iso4217Definition("HTG", 332, 2),
        new Iso4217Definition("HUF", 348, 2),
        new Iso4217Definition("IDR", 360, 2),
        new Iso4217Definition("ILS", 376, 2),
        new Iso4217Definition("INR", 356, 2),
        new Iso4217Definition("IQD", 368, 3),
        new Iso4217Definition("IRR", 364, 0),
        new Iso4217Definition("ISK", 352, 0),
        new Iso4217Definition("JMD", 388, 2),
        new Iso4217Definition("JOD", 400, 3),
        new Iso4217Definition("JPY", 392, 0),
        new Iso4217Definition("KES", 404, 2),
        new Iso4217Definition("KGS", 417, 2),
        new Iso4217Definition("KHR", 116, 2),
        new Iso4217Definition("KMF", 174, 0),
        new Iso4217Definition("KPW", 408, 0),
        new Iso4217Definition("KRW", 410, 0),
        new Iso4217Definition("KWD", 414, 3),
        new Iso4217Definition("KYD", 136, 2),
        new Iso4217Definition("KZT", 398, 2),
        new Iso4217Definition("LAK", 418, 0),
        new Iso4217Definition("LBP", 422, 0),
        new Iso4217Definition("LKR", 144, 2),
        new Iso4217Definition("LRD", 430, 2),
        new Iso4217Definition("LSL", 426, 2),
        new Iso4217Definition("LTL", 440, 2),
        new Iso4217Definition("LVL", 428, 2),
        new Iso4217Definition("LYD", 434, 3),
        new Iso4217Definition("MAD", 504, 2),
        new Iso4217Definition("MDL", 498, 2),
        new Iso4217Definition("MGA", 969, 2),
        new Iso4217Definition("MKD", 807, 0),
        new Iso4217Definition("MMK", 104, 0),
        new Iso4217Definition("MNT", 496, 2),
        new Iso4217Definition("MOP", 446, 2),
        new Iso4217Definition("MRO", 478, 2),
        new Iso4217Definition("MUR", 480, 2),
        new Iso4217Definition("MVR", 462, 2),
        new Iso4217Definition("MWK", 454, 2),
        new Iso4217Definition("MXN", 484, 2),
        new Iso4217Definition("MXV", 979, 2),
        new Iso4217Definition("MYR", 458, 2),
        new Iso4217Definition("MZN", 943, 2),
        new Iso4217Definition("NAD", 516, 2),
        new Iso4217Definition("NGN", 566, 2),
        new Iso4217Definition("NIO", 558, 2),
        new Iso4217Definition("NOK", 578, 2),
        new Iso4217Definition("NPR", 524, 2),
        new Iso4217Definition("NZD", 554, 2),
        new Iso4217Definition("OMR", 512, 3),
        new Iso4217Definition("PAB", 590, 2),
        new Iso4217Definition("PEN", 604, 2),
        new Iso4217Definition("PGK", 598, 2),
        new Iso4217Definition("PHP", 608, 2),
        new Iso4217Definition("PKR", 586, 2),
        new Iso4217Definition("PLN", 985, 2),
        new Iso4217Definition("PYG", 600, 0),
        new Iso4217Definition("QAR", 634, 2),
        new Iso4217Definition("RON", 946, 2),
        new Iso4217Definition("RSD", 941, 2),
        new Iso4217Definition("RUB", 643, 2),
        new Iso4217Definition("RWF", 646, 0),
        new Iso4217Definition("SAR", 682, 2),
        new Iso4217Definition("SBD", 90, 2),
        new Iso4217Definition("SCR", 690, 2),
        new Iso4217Definition("SDG", 938, 2),
        new Iso4217Definition("SEK", 752, 2),
        new Iso4217Definition("SGD", 702, 2),
        new Iso4217Definition("SHP", 654, 2),
        new Iso4217Definition("SLL", 694, 0),
        new Iso4217Definition("SOS", 706, 2),
        new Iso4217Definition("SRD", 968, 2),
        new Iso4217Definition("SSP", 728, 2),
        new Iso4217Definition("STD", 678, 0),
        new Iso4217Definition("SYP", 760, 2),
        new Iso4217Definition("SZL", 748, 2),
        new Iso4217Definition("THB", 764, 2),
        new Iso4217Definition("TJS", 972, 2),
        new Iso4217Definition("TMT", 934, 2),
        new Iso4217Definition("TND", 788, 3),
        new Iso4217Definition("TOP", 776, 2),
        new Iso4217Definition("TRY", 949, 2),
        new Iso4217Definition("TTD", 780, 2),
        new Iso4217Definition("TWD", 901, 2),
        new Iso4217Definition("TZS", 834, 2),
        new Iso4217Definition("UAH", 980, 2),
        new Iso4217Definition("UGX", 800, 2),
        new Iso4217Definition("USD", 840, 2),
        new Iso4217Definition("USN", 997, 2),
        new Iso4217Definition("USS", 998, 2),
        new Iso4217Definition("UYI", 940, 0),
        new Iso4217Definition("UYU", 858, 2),
        new Iso4217Definition("UZS", 860, 2),
        new Iso4217Definition("VEF", 937, 2),
        new Iso4217Definition("VND", 704, 0),
        new Iso4217Definition("VUV", 548, 0),
        new Iso4217Definition("WST", 882, 2),
        new Iso4217Definition("XAF", 950, 0),
        new Iso4217Definition("XCD", 951, 2),
        new Iso4217Definition("XOF", 952, 0),
        new Iso4217Definition("XPF", 953, 0),
        new Iso4217Definition("YER", 886, 2),
        new Iso4217Definition("ZAR", 710, 2),
        new Iso4217Definition("ZMW", 967, 2)

    };

        public static Iso4217Definition LookupByCode(string code)
        {
            return DefinitionCollection.SingleOrDefault(d => d.Code == code.ToUpper()) ?? Iso4217Definition.NotFound();
        }

        public static Iso4217Definition LookupByNumber(int number)
        {
            return DefinitionCollection.SingleOrDefault(d => d.Number == number) ?? Iso4217Definition.NotFound();
        }

        public class Iso4217Definition
        {
            private readonly string _code;
            private readonly int _number;
            private readonly int _exponent;
            public bool Found { get; set; }

            public string Code
            {
                get { return _code; }
            }

            public int Number
            {
                get { return _number; }
            }

            public int Exponent
            {
                get { return _exponent; }
            }

            private Iso4217Definition() { }

            public Iso4217Definition(string code, int number, int exponent)
            {
                _code = code;
                _number = number;
                _exponent = exponent;
                Found = true;
            }

            public static Iso4217Definition NotFound()
            {
                return new Iso4217Definition { Found = false };
            }
        }
    }
}