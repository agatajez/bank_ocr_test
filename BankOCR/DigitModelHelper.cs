using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankOCR
{
    public class DigitModelHelper
    {
        private static readonly Dictionary<int, List<int>> _digitsByBarsNumberDict = new Dictionary<int, List<int>>
        {
            { 2, new List<int> { 1 } },
            { 3, new List<int> { 7 } },
            { 4, new List<int> { 4 } },
            { 5, new List<int> { 2, 3, 5 } },
            { 6, new List<int> { 0, 6, 9 } },
            { 7, new List<int> { 8 } }
        };

        private static readonly Dictionary<int, List<Line>> _digitModelDict = new Dictionary<int, List<Line>>
        {
            { 0, new List<Line>()
                { 
                    new Line(0, 1),
                    new Line(1, 0),
                    new Line(1, 2),
                    new Line(2, 0),
                    new Line(2, 1),
                    new Line(2, 2),
                } 
            },
            { 1, new List<Line>()
                {
                    new Line(1, 2),
                    new Line(2, 2)
                }
            },
            { 2, new List<Line>()
                {
                    new Line(0, 1),
                    new Line(1, 1),
                    new Line(1, 2),
                    new Line(2, 0),
                    new Line(2, 1),
                }
            },
            { 3, new List<Line>()
                {
                    new Line(0, 1),
                    new Line(1, 1),
                    new Line(1, 2),
                    new Line(2, 1),
                    new Line(2, 2),
                }
            },
            { 4, new List<Line>()
                {
                    new Line(1, 0),
                    new Line(1, 1),
                    new Line(1, 2),
                    new Line(2, 2),
                }
            },
            { 5, new List<Line>()
                {
                    new Line(0, 1),
                    new Line(1, 0),
                    new Line(1, 1),
                    new Line(2, 1),
                    new Line(2, 2),
                }
            },
            { 6, new List<Line>()
                {
                    new Line(0, 1),
                    new Line(1, 0),
                    new Line(1, 1),
                    new Line(2, 0),
                    new Line(2, 1),
                    new Line(2, 2),
                }
            },
            { 7, new List<Line>()
                {
                    new Line(0, 1),
                    new Line(1, 2),
                    new Line(2, 2),
                }
            },
            { 8, new List<Line>()
                {
                    new Line(0, 1),
                    new Line(1, 0),
                    new Line(1, 1),
                    new Line(1, 2),
                    new Line(2, 0),
                    new Line(2, 1),
                    new Line(2, 2),
                }
            },
            { 9, new List<Line>()
                {
                    new Line(0, 1),
                    new Line(1, 0),
                    new Line(1, 1),
                    new Line(1, 2),
                    new Line(2, 1),
                    new Line(2, 2),
                }
            }
        };

        public static List<int> GetDigitsByBarsNumber(int bars)
        {
            return _digitsByBarsNumberDict.ContainsKey(bars)
                        ? _digitsByBarsNumberDict[bars]
                        : new List<int>(0);
        }

        public static List<Line> GetDigitModel(int value)
        {
            return _digitModelDict.ContainsKey(value)
                          ? _digitModelDict[value]
                          : new List<Line>(0);
        }

        public static bool AreEqualModels(List<Line> modelA, List<Line> modelB)
        {
            if (modelA == null || modelB == null)
                return false;

            if (modelA.Count != modelB.Count)
                return false;

            modelA = modelA.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            modelB = modelB.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();

            return modelA.All(a => modelB.Any(b => b.Equals(a)));
        }

        public static bool AreInterchangeableModels(List<Line> modelA, List<Line> modelB)
        {
            if (AreEqualModels(modelA, modelB))
                return true;

            var modelAOnly = modelA.Count(x => !modelB.Any(b => b.Equals(x)));
            var modelBOnly = modelB.Count(x => !modelA.Any(a => a.Equals(x)));

            return modelAOnly < 2 && modelBOnly < 2;
        }
    }
}
