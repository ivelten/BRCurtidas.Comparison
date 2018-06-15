using System;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class String100
    {
        public String100(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            if (s.Length > 50)
                throw new ArgumentException("Text value must not have more than 100 characters.");

            Value = s;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator ==(String100 x, String100 y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(String100 x, String100 y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<String100, T> Apply<T>(String100 x, Func<string, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as String100;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }
}