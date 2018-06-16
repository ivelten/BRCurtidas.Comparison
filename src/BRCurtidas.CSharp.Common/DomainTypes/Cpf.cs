using System;
using System.Text.RegularExpressions;

namespace BRCurtidas.CSharp.Common.DomainTypes
{
    public class Cpf
    {
        public Cpf(string cpf)
        {
            if (cpf == null)
                throw new ArgumentNullException(nameof(cpf));

            if (!IsValidFormat(cpf))
                throw new ArgumentException("CPF number is not recognized as a valid CPF.", nameof(cpf));

            if (!VerifyingDigitsAreCorrect(Canonicalize(cpf)))
                throw new ArgumentException("Verifying digits of CPF are not valid.");

            Value = cpf;
        }

        public const string RegexPattern = @"^\d{3}\.\d{3}\.\d{3}-\d{2}$";

        private static string Canonicalize(string cpf)
        {
            return cpf.Trim().Replace(".", "").Replace("-", "");
        }

        private static bool IsValidFormat(string cpf)
        {
            return new Regex(RegexPattern).IsMatch(cpf);
        }

        private static bool VerifyingDigitsAreCorrect(string cpf)
        {
            var numbers = cpf.Substring(0, 9);

            var sum = 0;

            var mul1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 9; i++)
                sum += int.Parse(numbers[i].ToString()) * mul1[i];

            var remaining = sum % 11;

            if (remaining < 2)
                remaining = 0;
            else
                remaining = 11 - remaining;

            var digito = remaining.ToString();
            numbers = numbers + digito;
            sum = 0;

            var mul2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 10; i++)
                sum += int.Parse(numbers[i].ToString()) * mul2[i];

            remaining = sum % 11;

            if (remaining < 2)
                remaining = 0;
            else
                remaining = 11 - remaining;

            digito = digito + remaining.ToString();

            return cpf.EndsWith(digito);
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator ==(Cpf x, Cpf y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(Cpf x, Cpf y)
        {
            if (x == null && y == null)
                return false;

            if (x == null || y == null)
                return true;

            return x.Value != y.Value;
        }

        public static Func<Cpf, T> Apply<T>(Cpf x, Func<string, T> f)
        {
            return e => f(x.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Cpf;

            if (casted == null)
                return false;

            return casted.Value == Value;
        }
    }
}