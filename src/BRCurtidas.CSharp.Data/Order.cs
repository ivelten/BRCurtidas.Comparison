using BRCurtidas.CSharp.Common.DomainTypes;
using System;

namespace BRCurtidas.CSharp.Data
{
    public class Order
    {
        public User User { get; set; }

        public Product Product { get; set; }

        public Payment Payment { get; set; }

        public OrderStatus Status { get; set; }

        public Quantity Quantity { get; set; }

        public Currency Price { get; set; }

        public DateTime Created { get; set; }

        public SocialNetworkAccount SocialNetworkAccount { get; set; }

        public static bool operator ==(Order x, Order y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.User == y.User && x.Product == y.Product && x.Payment == y.Payment
                && x.Status == y.Status && x.Quantity == y.Quantity && x.Price == y.Price
                && x.Created == y.Created && x.SocialNetworkAccount == y.SocialNetworkAccount;
        }

        public static bool operator !=(Order x, Order y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.User != y.User && x.Product != y.Product && x.Payment != y.Payment
                && x.Status != y.Status && x.Quantity != y.Quantity && x.Price != y.Price
                && x.Created != y.Created && x.SocialNetworkAccount != y.SocialNetworkAccount;
        }

        public override int GetHashCode()
        {
            return User.GetHashCode() ^ Product.GetHashCode() ^ Payment.GetHashCode()
                ^ Status.GetHashCode() ^ Quantity.GetHashCode() ^ Price.GetHashCode()
                ^ Created.GetHashCode() ^ SocialNetworkAccount.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Order;

            if (casted == null)
                return false;

            return casted.User == User && casted.Product == Product && casted.Payment == Payment
                && casted.Status == Status && casted.Quantity == Quantity && casted.Price == Price
                && casted.Created == Created && casted.SocialNetworkAccount == SocialNetworkAccount;
        }
    }
}