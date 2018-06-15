using System;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class String50
    {
        public String50(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            if (s.Length > 50)
                throw new ArgumentException("Text value must not have more than 50 characters.");

            Value = s;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator ==(String50 x, String50 y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(String50 x, String50 y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<String50, T> Apply<T>(String50 x, Func<string, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as String50;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }
}