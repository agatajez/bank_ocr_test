using System;
using System.Linq;

namespace BankOcrKata
{
    public sealed class Digit
    {
        public bool HasTopBar => DigitModel[0, 1] == 1;
        public bool HasTopLeftBar => DigitModel[1, 0] == 1;
        public bool HasTopRightBar => DigitModel[1, 2] == 1;
        public bool HasMiddleBar => DigitModel[1, 1] == 1;
        public bool HasBottomLeftBar => DigitModel[2, 0] == 1;
        public bool HasBottomRightBar => DigitModel[2, 2] == 1;
        public short? DigitValue { get; set; }
        public short[,] DigitModel { get; set; }

        public Digit()
        {
            DigitModel = new short[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        }

        public Digit(char number) : base()
        {
            DigitValue = short.Parse(number.ToString());
        }
    }
}