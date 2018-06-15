using System;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class PasswordHash
    {
        private PasswordHash(string hash, PasswordHashInfo info)
        {
            Value = hash;
            Info = info;
        }

        public PasswordHash(string hash)
        {
            if (hash == null)
                throw new ArgumentNullException(nameof(hash));

            var info = new PasswordHashInfo(BCrypt.Net.BCrypt.InterrogateHash(hash));

            new PasswordHash(hash, info);
        }

        public static PasswordHash FromPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var info = new PasswordHashInfo(BCrypt.Net.BCrypt.InterrogateHash(hash));

            return new PasswordHash(hash, info);
        }

        public string Value { get; private set; }

        public PasswordHashInfo Info { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator ==(PasswordHash x, PasswordHash y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(PasswordHash x, PasswordHash y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<PasswordHash, T> Apply<T>(PasswordHash x, Func<string, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as PasswordHash;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }

    public class PasswordHashInfo
    {
        public PasswordHashInfo(string settings, string version, string workFactor)
        {
            Settings = settings;
            Version = version;
            WorkFactor = workFactor;
        }

        public PasswordHashInfo(BCrypt.Net.HashInformation info)
            : this(info.Settings, info.Version, info.WorkFactor)
        {
        }

        public string Settings { get; private set; }

        public string Version { get; private set; }

        public string WorkFactor { get; private set; }

        public override string ToString()
        {
            return $"{{ Settings : \"{Settings}\"; Version : \"{Version}\"; WorkFactor : \"{WorkFactor}\" }}";
        }

        public static bool operator ==(PasswordHashInfo x, PasswordHashInfo y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Settings == y.Settings && x.Version == y.Version && x.WorkFactor == y.WorkFactor;
        }

        public static bool operator !=(PasswordHashInfo x, PasswordHashInfo y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Settings != y.Settings && x.Version != y.Version && x.WorkFactor != y.WorkFactor;
        }

        public override int GetHashCode()
        {
            return Settings.GetHashCode() ^ Version.GetHashCode() ^ WorkFactor.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as PasswordHashInfo;

            if (casted == null)
                return false;

            return casted.Settings == Settings && casted.Version == Version && casted.WorkFactor == WorkFactor;
        }
    }
}