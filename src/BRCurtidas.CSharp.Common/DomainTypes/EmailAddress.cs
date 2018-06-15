using System;
using System.Text.RegularExpressions;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class EmailAddress
    {
        public EmailAddress(string address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            if (new Regex(RegexPattern).IsMatch(address) == false)
                throw new ArgumentException("Invalid e-mail address.", nameof(address));

            Value = Canonicalize(address);
        }

        private static string Canonicalize(string address)
        {
            if (address == null)
                return null;

            return address.ToLowerInvariant();
        }

        public const string RegexPattern = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(EmailAddress x, EmailAddress y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(EmailAddress x, EmailAddress y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<EmailAddress, T> Apply<T>(EmailAddress x, Func<string, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as EmailAddress;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }
}