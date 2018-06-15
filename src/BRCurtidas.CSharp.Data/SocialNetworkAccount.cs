using BRCurtidas.CSharp.Common.DomainTypes;

namespace BRCurtidas.CSharp.Data
{
    public class SocialNetworkAccount
    {
        public SocialNetworkType Type { get; set; }

        public Url Profile { get; set; }

        public static bool operator ==(SocialNetworkAccount x, SocialNetworkAccount y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Type == y.Type && x.Profile == y.Profile;
        }

        public static bool operator !=(SocialNetworkAccount x, SocialNetworkAccount y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Type != y.Type && x.Profile != y.Profile;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Profile.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as SocialNetworkAccount;

            if (casted == null)
                return false;

            return casted.Type == Type && casted.Profile == Profile;
        }
    }
}