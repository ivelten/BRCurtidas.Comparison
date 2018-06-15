using BRCurtidas.CSharp.Common.DomainTypes;

namespace BRCurtidas.CSharp.Data
{
    public class Address
    {
        public String100 Line1 { get; set; }

        public String100 Line2 { get; set; }

        public static bool operator ==(Address x, Address y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Line1 == y.Line1 && x.Line2 == y.Line2;
        }

        public static bool operator !=(Address x, Address y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Line1 != y.Line1 && x.Line2 != y.Line2;
        }

        public override int GetHashCode()
        {
            return Line1.GetHashCode() ^ Line2.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Address;

            if (casted == null)
                return false;

            return casted.Line1 == Line1 && casted.Line2 == Line2;
        }
    }
}