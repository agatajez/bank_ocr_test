using BankOCR;
using System.Collections.Generic;
using System.IO;

namespace BankOcrKata
{
    public static class AccountNumberReader
    {
        private const int lineLength = 27;
        private const int digitHeight = 3;

        public static List<AccountNumber> GetAccountNumbersFrom(string input)
        {
            var accounts = new List<AccountNumber>();
            AccountNumber account = null;
            var reader = new StringReader(input);
            var line = string.Empty;
            var lineIndex = 0;
            while (line != null)
            {
                line = reader.ReadLine();

                if (line == null)
                    continue;

                if (line == string.Empty)
                {
                    account = new AccountNumber();
                    lineIndex = 0;
                    accounts.Add(account);
                }
                else if (line.Length != lineLength)
                    throw new InvalidDataException($"Invalid line lenght ({line.Length}).");
                else
                    UpdateDigitModel(line, account, lineIndex++);
            }

            return accounts;
        }

        private static void UpdateDigitModel(string line, AccountNumber account, int index)
        {
            int digitIndex = 0;
            for (int i = 0; i < lineLength; i += 3)
            {
                var digit = index == 0 ? new Digit() : account.Number[digitIndex++];
                if (!account.Number.Contains(digit))
                    account.Number.Add(digit);

                if (index > 0 && !char.IsWhiteSpace(line[i]))
                    digit.DigitModel.Add(new Line(index, 0));

                if (!char.IsWhiteSpace(line[i + 1]))
                    digit.DigitModel.Add(new Line(index, 1));

                if (index > 0 && !char.IsWhiteSpace(line[i + 2]))
                    digit.DigitModel.Add(new Line(index, 2));

                if (index == digitHeight - 1)
                    digit.TrySetValue();
            }
        }
                
    }

}