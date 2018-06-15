namespace BRCurtidas.CSharp.Data
{
    public class Payment
    {
        public string Reference { get; set; }

        public PaymentMethod Method { get; set; }

        public static bool operator ==(Payment x, Payment y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Reference == y.Reference && x.Method == y.Method;
        }

        public static bool operator !=(Payment x, Payment y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Reference != y.Reference && x.Method != y.Method;
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode() ^ Method.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Payment;

            if (casted == null)
                return false;

            return casted.Reference == Reference && casted.Method == Method;
        }
    }
}