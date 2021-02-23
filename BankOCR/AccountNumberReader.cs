using System.Collections.Generic;
using System.IO;

namespace BankOcrKata
{
    public static class AccountNumberReader
    {
        private const int lineLength = 27;

        public static List<AccountNumber> GetAccountNumbersFrom(string input)
        {
            var accounts = new List<AccountNumber>();
            var reader = new StringReader(input);
            var line = string.Empty;
            while (line != null)
            {
                line = reader.ReadLine();
                if (line == null)
                    continue;

                var account = new AccountNumber();

                line = reader.ReadLine();
                ReadTopLine(line, account);

                line = reader.ReadLine();
                ReadMiddleLine(line, account);

                line = reader.ReadLine();
                ReadBottomLine(line, account);

                accounts.Add(account);
            }

            return accounts;
        }

        private static void ReadTopLine(string topLine, AccountNumber account)
        {
            for (int i = 1; i < lineLength; i += 3)
            {
                var digit = new Digit();
                account.Number.Add(digit);

                if (!char.IsWhiteSpace(topLine[i]))
                    digit.DigitModel[0, 1] = 1;
            }
        }

        private static void ReadMiddleLine(string middleLine, AccountNumber account)
        {
            int digitIndex = 0;
            for (int i = 0; i < lineLength; i += 3)
            {
                var digit = account.Number[digitIndex];
                digitIndex++;

                if (!char.IsWhiteSpace(middleLine[i]))
                    digit.DigitModel[1, 0] = 1;

                if (!char.IsWhiteSpace(middleLine[i + 1]))
                    digit.DigitModel[1, 1] = 1;

                if (!char.IsWhiteSpace(middleLine[i + 2]))
                    digit.DigitModel[1, 2] = 1;

                if (!digit.HasTopBar)
                {
                    digit.DigitValue = (short)(digit.HasTopLeftBar ? 4 : 1);
                    continue;
                }

                if (!digit.HasTopLeftBar && !digit.HasMiddleBar)
                    digit.DigitValue = 7;
            }
        }

        private static void ReadBottomLine(string bottomLine, AccountNumber account)
        {
            int digitIndex = 0;
            for (int i = 0; i < lineLength; i += 3)
            {
                var digit = account.Number[digitIndex];
                digitIndex++;

                if (digit.DigitValue != null)
                    continue;
                if (!char.IsWhiteSpace(bottomLine[i]))
                    digit.DigitModel[2, 0] = 1;

                if (!char.IsWhiteSpace(bottomLine[i + 1]))
                    digit.DigitModel[2, 1] = 1;

                if (!char.IsWhiteSpace(bottomLine[i + 2]))
                    digit.DigitModel[2, 2] = 1;

                if (!digit.HasTopLeftBar)
                {
                    if (digit.HasMiddleBar)
                    {
                        if (digit.HasBottomLeftBar)
                            digit.DigitValue = 2;
                        else if (digit.HasTopRightBar)
                            digit.DigitValue = 3;

                        if (digit.DigitValue.HasValue)
                            continue;
                    }
                }
                else if (!digit.HasTopRightBar)
                {
                    digit.DigitValue = (short)(digit.HasBottomLeftBar ? 6 : 5);
                    continue;
                }
                else
                {
                    if (digit.HasMiddleBar)
                    {
                        if (digit.HasBottomLeftBar)
                            digit.DigitValue = 8;
                        else if (digit.HasBottomRightBar)
                            digit.DigitValue = 9;
                    }
                    else
                        digit.DigitValue = 0;

                    continue;
                }

            }
        }
    }

}