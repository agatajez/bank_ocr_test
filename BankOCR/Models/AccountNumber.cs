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
            AlternativeNumbers = new List<AccountNumber>();
        }

        public AccountNumber(string number) : base()
        {
            AlternativeNumbers = new List<AccountNumber>();
            Number = new List<Digit>();
            for (int i = 0; i < number.Length; i++)
                Number.Add(new Digit(number[i]));
        }

        public List<Digit> Number { get; set; }
        public List<AccountNumber> AlternativeNumbers { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            Number.ForEach(x => builder.Append(x.DigitValue == null ? "?" : x.DigitValue.ToString()));

            if (AlternativeNumbers?.Any() == true)
            {
                if (AlternativeNumbers.Count() == 1 && !HasValidChecksum())
                {
                    return AlternativeNumbers[0].ToString();
                }
                else
                {
                    var numbers = AlternativeNumbers.Select(x => $"'{x}'").ToList();
                    builder.Append($" AMB [{string.Join(", ", numbers.OrderBy(x => x))}]");
                }
            }
            else if (Number.Any(x => x.DigitValue == null))
                builder.Append(" ILL");

            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;

            var other = (AccountNumber)obj;
            return ToString() == other.ToString();
        }

        public bool HasValidChecksum()
        {
            if (HasUnrecognisedDigits())
                return false;

            var checksum = GetChecksum();
            return checksum == 0;
        }

        public bool HasUnrecognisedDigits()
            => Number.Any(x => x.DigitValue == null);

        public List<AccountNumber> GetAlternativeNumbersForUnrecognised()
        {
            var alternativeAccountNumbers = new List<AccountNumber>();

            // only for user story 4: "It turns out that often when a number comes back as 
            // ERR or ILL it is because the scanner has failed to pick up on one pipe or underscore for one of the figures."
            var unrecognized = Number.FirstOrDefault(digit => digit.DigitValue == null);
            if (unrecognized != null)
            {
                var alternativeValues = unrecognized.GetAlternativeValues();
                var numberAsText = string.Empty;
                foreach (var alternative in alternativeValues)
                {
                    numberAsText = this.ToString().Substring(0, 9);
                    numberAsText = numberAsText.Replace("?", alternative.ToString());

                    var altAccount = new AccountNumber(numberAsText);
                    if (altAccount.HasValidChecksum())
                        alternativeAccountNumbers.Add(altAccount);
                }
            }

            return alternativeAccountNumbers;
        }

        public List<AccountNumber> GetAlternativeNumbers()
        {
            var result = new List<AccountNumber>();
            var digits = new List<Digit>();
            Number.ForEach(x =>
            {
                if (!digits.Any(y => y.DigitValue == x.DigitValue))
                    digits.Add(x);
            });

            var numberAsText = this.ToString().Substring(0, 9);
            foreach (var digit in digits)
            {
                var digitOccurrences = numberAsText.Count(x => x.ToString() == digit.DigitValue.ToString());
                var alternativeValues = digit.GetAlternativeValues();
                foreach (var newValue in alternativeValues)
                {
                    var digitValueIndex = 0;
                    for (int i = 0; i < digitOccurrences; i++)
                    {
                        digitValueIndex = numberAsText.IndexOf(digit.DigitValue.ToString(), digitValueIndex);

                        var newAccountNumber = new AccountNumber(numberAsText
                        .Remove(digitValueIndex, 1)
                        .Insert(digitValueIndex, newValue.ToString()));

                        if (newAccountNumber.HasValidChecksum() && !result.Any(x => x.Equals(newAccountNumber)))
                            result.Add(newAccountNumber);

                        digitValueIndex++;
                    }
                }
            }

            return result.Distinct().ToList();
        }

        private int GetChecksum()
        {
            var sum = 0;
            var index = 0;
            for (int i = 9; i > 0; i--)
                sum += Number[index++].DigitValue.Value * i;

            return sum % 11;
        }
    }

}