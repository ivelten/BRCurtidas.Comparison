using System;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class Currency
    {
        public Currency(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be more than zero.", nameof(amount));

            if (GetDecimalPlaces(amount) != 2)
                throw new ArgumentException("Too many digits places for cents. Amount should have cents of only two digits.");

            Value = amount;
        }

        private static int GetDecimalPlaces(decimal x, int acc = 0)
        {
            if (x % 1M == 0M)
                return GetDecimalPlaces(x * 10M, acc + 1);
            else
                return acc;
        }

        public decimal Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator ==(Currency x, Currency y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(Currency x, Currency y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<Currency, T> Apply<T>(Currency x, Func<decimal, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Currency;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }
}