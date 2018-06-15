using BRCurtidas.CSharp.Common.DomainTypes;

namespace BRCurtidas.CSharp.Data
{
    public class User
    {
        public String50 Name { get; set; }

        public EmailAddress Email { get; set; }

        public Cpf Cpf { get; set; }

        public PhoneNumber Phone { get; set; }

        public Account Account { get; set; }

        public Address Address { get; set; }

        public static bool operator ==(User x, User y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Name == y.Name && x.Email == y.Email && x.Cpf == y.Cpf && x.Phone == y.Phone && x.Account == y.Account && x.Address == y.Address;
        }

        public static bool operator !=(User x, User y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Name != y.Name && x.Email != y.Email && x.Cpf != y.Cpf && x.Phone != y.Phone && x.Account != y.Account && x.Address != y.Address;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Email.GetHashCode() ^ Cpf.GetHashCode() ^ Phone.GetHashCode() ^ Account.GetHashCode() ^ Address.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as User;

            if (casted == null)
                return false;

            return casted.Name == Name && casted.Email == Email && casted.Cpf == Cpf && casted.Phone == Phone && casted.Account == Account && casted.Address == Address;
        }
    }
}