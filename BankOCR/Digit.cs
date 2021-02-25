using BankOCR;
using System.Collections.Generic;
using System.Linq;

namespace BankOcrKata
{
    public sealed class Digit
    {
        public int? DigitValue { get; set; }
        public List<Line> DigitModel { get; set; }

        public Digit()
        {
            DigitModel = new List<Line>();
        }

        public Digit(char number) : base()
        {
            DigitModel = new List<Line>();
            DigitValue = int.Parse(number.ToString());
        }

        public void TrySetValue()
        {            
            var possibleValues = DigitModelHelper.GetDigitsByBarsNumber(DigitModel.Count);
            if (!possibleValues.Any())
                return;

            foreach (var value in possibleValues)
            {
                var model = DigitModelHelper.GetDigitModel(value);
                if (DigitModelHelper.AreEqualModels(model, DigitModel))
                {
                    DigitValue = value;
                    return;
                }
            }
        }

        public List<int> GetAlternativeValues()
        {
            var lines = DigitModel.Count;
            var alternatives = DigitModelHelper.GetDigitsByBarsNumber(lines + 1);
            alternatives.AddRange(DigitModelHelper.GetDigitsByBarsNumber(lines - 1));

            var result = new List<int>();
            foreach (var alternativeValue in alternatives)
            {
                var model = DigitModelHelper.GetDigitModel(alternativeValue);
                if (DigitModelHelper.AreInterchangeableModels(model, DigitModel))
                    result.Add(alternativeValue);
            }
            
            return result;
        }
    }
}