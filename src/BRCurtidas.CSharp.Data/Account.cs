using BRCurtidas.CSharp.Common.DomainTypes;

namespace BRCurtidas.CSharp.Data
{
    public class Account
    {
        public UserName UserName { get; set; }

        public PasswordHash PasswordHash { get; set; }

        public static bool operator ==(Account x, Account y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.UserName == y.UserName && x.PasswordHash == y.PasswordHash;
        }

        public static bool operator !=(Account x, Account y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.UserName != y.UserName && x.PasswordHash != y.PasswordHash;
        }

        public override int GetHashCode()
        {
            return UserName.GetHashCode() ^ PasswordHash.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Account;

            if (casted == null)
                return false;

            return casted.UserName == UserName && casted.PasswordHash == PasswordHash;
        }
    }
}