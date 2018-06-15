using System;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class Quantity
    {
        public Quantity(int id)
        {
            if (id < 0)
                throw new ArgumentException("Value must be longer than 0.", nameof(id));

            Value = id;
        }

        public int Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator ==(Quantity x, Quantity y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(Quantity x, Quantity y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<Quantity, T> Apply<T>(Quantity x, Func<int, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Quantity;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }
}