using BRCurtidas.CSharp.Common.DomainTypes;

namespace BRCurtidas.CSharp.Data
{
    public class Entity<T>
    {
        public Id Id { get; set; }

        public override bool Equals(object obj)
        {
            var casted = obj as Entity<T>;

            if (casted == null)
                return false;

            return casted.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<T> x, Entity<T> y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Id == y.Id;
        }

        public static bool operator !=(Entity<T> x, Entity<T> y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Id != y.Id;
        }
    }
}