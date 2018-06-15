using BRCurtidas.CSharp.Common.DomainTypes;

namespace BRCurtidas.CSharp.Data
{
    public class Product
    {
        public Currency Price { get; set; }

        public bool Enabled { get; set; }

        public String100 Description { get; set; }

        public String50 Title { get; set; }

        public static bool operator ==(Product x, Product y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Price == y.Price && x.Enabled == y.Enabled && x.Description == y.Description && x.Title == y.Title;
        }

        public static bool operator !=(Product x, Product y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Price != y.Price && x.Enabled != y.Enabled && x.Description != y.Description && x.Title != y.Title;
        }

        public override int GetHashCode()
        {
            return Price.GetHashCode() ^ Enabled.GetHashCode() ^ Description.GetHashCode() ^ Title.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Product;

            if (casted == null)
                return false;

            return casted.Price == Price && casted.Enabled == Enabled && casted.Description == Description && casted.Title == Title;
        }
    }
}