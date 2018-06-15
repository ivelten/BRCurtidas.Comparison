using System;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class Url
    {
        public Url(string url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            if (Uri.TryCreate(url, UriKind.Absolute, out var result))
            {
                var scheme = result.Scheme.ToLowerInvariant();

                if (scheme != "http" && scheme != "https")
                    throw new ArgumentException("URL scheme is invalid. Accepted schemes are http and https.");
            }
            else
            {
                throw new ArgumentException("URL format is invalid.", nameof(url));
            }

            Value = url;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(Url x, Url y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(Url x, Url y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<Url, T> Apply<T>(Url x, Func<string, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Url;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }
}