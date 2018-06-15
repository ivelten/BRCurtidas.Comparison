using System;
using System.Linq;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class UserName
    {
        public UserName(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            if (userName.Contains(' '))
                throw new ArgumentNullException("UserName can not contain spaces.", nameof(userName));
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator ==(UserName x, UserName y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(UserName x, UserName y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<UserName, T> Apply<T>(UserName x, Func<string, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as UserName;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }
}