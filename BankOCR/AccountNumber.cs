using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankOcrKata
{
    public class AccountNumber
    {
        public AccountNumber()
        {
            Number = new List<Digit>(9);
        }

        public AccountNumber(string number)
        {
            Number = new List<Digit>();
            for (int i = 0; i < number.Length; i++)
                Number.Add(new Digit(number[i]));
                
        }

        public List<Digit> Number { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            Number.ForEach(x => builder.Append(x.DigitValue == null ? "?" : x.DigitValue.ToString()));

            if (Number.Any(x => x.DigitValue == null))
                builder.Append(" ILL");

            return builder.ToString();
        }

        public bool HasValidChecksum()
        {
            if (Number.Any(x => x.DigitValue == null) || !Number.All(x => x.DigitValue >= 0 && x.DigitValue <= 9))
                return false;

            var sum = 0;
            var index = 0;
            for (int i = 9; i > 0; i--)
                sum += Number[index++].DigitValue.Value * i;

            return sum % 11 == 0;
        }
    }

}