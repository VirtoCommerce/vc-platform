using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VirtoCommerce.Scheduling.LogicalCall
{
    /// <summary>
    /// Parser for "Connection string" alike formated strings.
    /// Very usefull to make configuration more compact and flexible.  
    /// </summary>
    public static class PairsParser
    {
        public static Dictionary<string, string> Parse(string configurationString)
        {
            var @value = new Dictionary<string, string>();
            ParseInternal(@value, configurationString, true, null, false, false);
            return @value;
        }

        public static string Encode(Dictionary<string, string> dictionary )
        {
            var @value = "";
            var e = dictionary.GetEnumerator();
            bool hasFirst = e.MoveNext();
            if (hasFirst)
            {
                var tb = new StringBuilder();
// ReSharper disable PossibleNullReferenceException
                tb.Append(e.Current.Key).Append("=").Append(e.Current.Value);
// ReSharper restore PossibleNullReferenceException
                while (e.MoveNext())
                {
// ReSharper disable PossibleNullReferenceException
                    tb.Append("; ").Append(e.Current.Key).Append("=").Append(e.Current.Value);
// ReSharper restore PossibleNullReferenceException
                }
                @value = tb.ToString();
            }
            return @value;
        }

        private static string GetKeyName(StringBuilder buffer, bool convertKeysToLowerCase)
        {
            int length = buffer.Length;
            while ((0 < length) && char.IsWhiteSpace(buffer[length - 1]))
            {
                length--;
            }
            string s = buffer.ToString(0, length);
            if (convertKeysToLowerCase)
                s = s.ToLower(CultureInfo.InvariantCulture);
            return s;
        }

        private enum ParserState
        {
            BraceQuoteValue = 10,
            BraceQuoteValueQuote = 11,
            DoubleQuoteValue = 6,
            DoubleQuoteValueQuote = 7,
            Key = 2,
            KeyEnd = 4,
            KeyEqual = 3,
            NothingYet = 1,
            NullTermination = 13,
            QuotedValueEnd = 12,
            SingleQuoteValue = 8,
            SingleQuoteValueQuote = 9,
            UnquotedValue = 5
        }

        private static string GetKeyValue(StringBuilder buffer, bool trimWhitespace)
        {
            int length = buffer.Length;
            int startIndex = 0;
            if (trimWhitespace)
            {
                while ((startIndex < length) && char.IsWhiteSpace(buffer[startIndex]))
                {
                    startIndex++;
                }
                while ((0 < length) && char.IsWhiteSpace(buffer[length - 1]))
                {
                    length--;
                }
            }
            return buffer.ToString(startIndex, length - startIndex);
        }

        private static int GetKeyValuePair(string connectionString, int currentPosition, StringBuilder buffer,
                                           bool useOdbcRules, out string keyname, out string keyvalue,
                                           bool convertKeysToLowerCase)
        {
            int index = currentPosition;
            buffer.Length = 0;
            keyname = null;
            keyvalue = null;
            char c = '\0';
            var nothingYet = ParserState.NothingYet;
            int length = connectionString.Length;
            while (currentPosition < length)
            {
                c = connectionString[currentPosition];
                switch (nothingYet)
                {
                    case ParserState.NothingYet:
                        if ((';' != c) && !char.IsWhiteSpace(c))
                        {
                            if (c != '\0')
                            {
                                break;
                            }
                            nothingYet = ParserState.NullTermination;
                        }
                        goto nextPosition;

                    case ParserState.Key:
                        if ('=' != c)
                        {
                            goto checkWhiteControl;
                        }
                        nothingYet = ParserState.KeyEqual;
                        goto nextPosition;

                    case ParserState.KeyEqual:
                        if (useOdbcRules || ('=' != c))
                        {
                            goto checkIsKeyExists;
                        }
                        nothingYet = ParserState.Key;
                        goto appendChar;

                    case ParserState.KeyEnd:
                        goto complexCheck;

                    case ParserState.UnquotedValue:
                        if (char.IsWhiteSpace(c) || (!char.IsControl(c) && (';' != c)))
                        {
                            goto appendChar;
                        }
                        goto eofData;

                    case ParserState.DoubleQuoteValue:
                        if ('"' != c)
                        {
                            goto checkForZero;
                        }
                        nothingYet = ParserState.DoubleQuoteValueQuote;
                        goto nextPosition;

                    case ParserState.DoubleQuoteValueQuote:
                        if ('"' != c)
                        {
                            goto getKeyValue;
                        }
                        nothingYet = ParserState.DoubleQuoteValue;
                        goto appendChar;

                    case ParserState.SingleQuoteValue:
                        if ('\'' != c)
                        {
                            goto checkForZero;
                        }
                        nothingYet = ParserState.SingleQuoteValueQuote;
                        goto nextPosition;

                    case ParserState.SingleQuoteValueQuote:
                        if ('\'' != c)
                        {
                            goto getKeyValue;
                        }
                        nothingYet = ParserState.SingleQuoteValue;
                        goto appendChar;

                    case ParserState.BraceQuoteValue:
                        if ('}' != c)
                        {
                            goto checkForZero;
                        }
                        nothingYet = ParserState.BraceQuoteValueQuote;
                        goto appendChar;

                    case ParserState.BraceQuoteValueQuote:
                        if ('}' != c)
                        {
                            goto quotedValueEnd;
                        }
                        nothingYet = ParserState.BraceQuoteValue;
                        goto appendChar;

                    case ParserState.QuotedValueEnd:
                        goto checkForWhiteZeroSemi;

                    case ParserState.NullTermination:
                        if ((c != '\0') && !char.IsWhiteSpace(c))
                        {
                            throw new ApplicationException("Syntax error in char " + currentPosition + " poistion");
                        }
                        goto nextPosition;

                    default:
                        throw new ApplicationException("Internal parser error, system error detected");
                }
                if (char.IsControl(c))
                {
                    throw new ApplicationException("Syntax error in char " + index + " poistion");
                }
                index = currentPosition;
                if ('=' != c)
                {
                    nothingYet = ParserState.Key;
                    goto appendChar;
                }
                nothingYet = ParserState.KeyEqual;
                goto nextPosition;

                checkWhiteControl:
                if (char.IsWhiteSpace(c) || !char.IsControl(c))
                {
                    goto appendChar;
                }
                throw new ApplicationException("Syntax error in char " + index + " poistion");

                checkIsKeyExists:
                keyname = GetKeyName(buffer, convertKeysToLowerCase);
                if (string.IsNullOrEmpty(keyname))
                {
                    throw new ApplicationException("Syntax error in char " + index + " poistion");
                }
                buffer.Length = 0;
                nothingYet = ParserState.KeyEnd;

                complexCheck:
                if (char.IsWhiteSpace(c))
                {
                    goto nextPosition;
                }
                if (useOdbcRules)
                {
                    if ('{' != c)
                    {
                        goto checkForSemiZeroControl;
                    }
                    nothingYet = ParserState.BraceQuoteValue;
                    goto appendChar;
                }
                if ('\'' == c)
                {
                    nothingYet = ParserState.SingleQuoteValue;
                    goto nextPosition;
                }
                if ('"' == c)
                {
                    nothingYet = ParserState.DoubleQuoteValue;
                    goto nextPosition;
                }

                checkForSemiZeroControl:
                if ((';' == c) || (c == '\0'))
                {
                    break;
                }
                if (char.IsControl(c))
                {
                    throw new ApplicationException("Syntax error in char " + index + " poistion");
                }
                nothingYet = ParserState.UnquotedValue;
                goto appendChar;


                getKeyValue:
                keyvalue = GetKeyValue(buffer, false);
                nothingYet = ParserState.QuotedValueEnd;
                goto checkForWhiteZeroSemi;

                checkForZero:
                if (c != '\0')
                {
                    goto appendChar;
                }
                throw new ApplicationException("Syntax error in char " + index + " poistion");

                quotedValueEnd:
                keyvalue = GetKeyValue(buffer, false);
                nothingYet = ParserState.QuotedValueEnd;
                checkForWhiteZeroSemi:
                if (char.IsWhiteSpace(c))
                {
                    goto nextPosition;
                }
                if (';' == c)
                {
                    break;
                }
                if (c == '\0')
                {
                    nothingYet = ParserState.NullTermination;
                    goto nextPosition;
                }
                throw new ApplicationException("Syntax error in char " + index + " poistion");
                appendChar:
                buffer.Append(c);
                nextPosition:
                currentPosition++;
            }
            eofData:
            switch (nothingYet)
            {
                case ParserState.NothingYet:
                case ParserState.KeyEnd:
                case ParserState.NullTermination:
                    break;

                case ParserState.Key:
                case ParserState.DoubleQuoteValue:
                case ParserState.SingleQuoteValue:
                case ParserState.BraceQuoteValue:
                    throw new ApplicationException("Syntax error in char " + index + " poistion");

                case ParserState.KeyEqual:
                    keyname = GetKeyName(buffer, convertKeysToLowerCase);
                    if (string.IsNullOrEmpty(keyname))
                    {
                        throw new ApplicationException("Syntax error in char " + index + " poistion");
                    }
                    break;

                case ParserState.UnquotedValue:
                    {
                        keyvalue = GetKeyValue(buffer, true);
                        char ch2 = keyvalue[keyvalue.Length - 1];
                        if (!useOdbcRules && (('\'' == ch2) || ('"' == ch2)))
                        {
                            throw new ApplicationException("Syntax error in char " + index + " poistion");
                        }
                        break;
                    }
                case ParserState.DoubleQuoteValueQuote:
                case ParserState.SingleQuoteValueQuote:
                case ParserState.BraceQuoteValueQuote:
                case ParserState.QuotedValueEnd:
                    keyvalue = GetKeyValue(buffer, false);
                    break;

                default:
                    throw new ApplicationException("Internal error.");
            }
            if ((';' == c) && (currentPosition < connectionString.Length))
            {
                currentPosition++;
            }
            return currentPosition;
        }

        public class Pair
        {
            private readonly int length;
            private readonly string name;
            private Pair next;
            private readonly string value;

            internal Pair(string name, string value, int length)
            {
                this.name = name;
                this.value = value;
                this.length = length;
            }

            public string Name
            {
                get { return this.name; }
            }

            internal string Value
            {
                get { return this.value; }
            }

            public Pair Next
            {
                get { return this.next; }
                set
                {
                    if ((this.next != null) || (value == null))
                    {
                        throw new ApplicationException("Can't parse");
                    }
                    this.next = value;
                }
            }

            public int Length
            {
                get { return this.length; }
            }

        }

        private static void ParseInternal(Dictionary<string, string> parsetable, string connectionString,
                                          bool buildChain, Hashtable synonyms, bool firstKey,
                                          bool convertKeysToLowerCase)
        {
            var buffer = new StringBuilder();
            Pair pair = null;
            int num = 0;
            int length = connectionString.Length;
            while (num < length)
            {
                string proposedKeyName, proposedValue;
                int currentPosition = num;
                num = GetKeyValuePair(connectionString, currentPosition, buffer, firstKey, out proposedKeyName,
                                      out proposedValue, convertKeysToLowerCase);
                if (string.IsNullOrEmpty(proposedKeyName))
                {
                    return;
                }
                string keyname = (synonyms != null) ? ((string) synonyms[proposedKeyName]) : proposedKeyName;
                if (!IsKeyNameValid(keyname))
                {
                    throw new ApplicationException("Key word not supported: " + proposedKeyName);
                }
                if (!firstKey || !parsetable.ContainsKey(keyname))
                {
                    parsetable[keyname] = proposedValue;
                }
                if (pair != null)
                {
                    pair = pair.Next = new Pair(keyname, proposedValue, num - currentPosition);
                }
                else if (buildChain)
                {
                    pair = new Pair(keyname, proposedValue, num - currentPosition);
                }
            }
        }

        private static bool IsKeyNameValid(string keyname)
        {
            if (keyname == null)
            {
                return false;
            }
            return ((((0 < keyname.Length) && (';' != keyname[0])) && !char.IsWhiteSpace(keyname[0])) &&
                    (-1 == keyname.IndexOf('\0')));
        }
    }
}