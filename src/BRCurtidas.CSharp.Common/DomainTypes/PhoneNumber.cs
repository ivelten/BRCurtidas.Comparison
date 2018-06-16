using System;
using System.Text.RegularExpressions;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class PhoneNumber
    {
        public PhoneNumber(string number)
        {
            if (number == null)
                throw new ArgumentNullException(nameof(number));

            if (!IsValidFormat(number))
                throw new ArgumentException("Invalid phone number format.", nameof(number));

            Value = number;
        }

        public const string RegexPattern = @"^(?:(?:\+|00)?(55)\s?)?(?:\(?([1-9][0-9])\)?\s?)?(?:((?:9\d|[2-9])\d{3})\-?(\d{4}))$";

        private static bool IsValidFormat(string number)
        {
            return new Regex(RegexPattern).IsMatch(number);
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator ==(PhoneNumber x, PhoneNumber y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(PhoneNumber x, PhoneNumber y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<PhoneNumber, T> Apply<T>(PhoneNumber x, Func<string, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as PhoneNumber;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }
}