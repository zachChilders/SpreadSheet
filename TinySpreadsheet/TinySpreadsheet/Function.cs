using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinySpreadsheet.Tokenize;

namespace TinySpreadsheet
{
    public static partial class Function
    {
        private static Dictionary<String, Func<String, Queue<FormulaToken>>> LookupTable = new Dictionary<string, Func<string, Queue<FormulaToken>>>();

        static Function()
        {
            LookupTable.Add("AVG", Average); // This adds the Average function under AVG
        }

        public static Queue<FormulaToken> Resolve(FormulaToken JRRToken)
        {
            //Get first 3 letters
            //Lookup in dictionary
            //call on expanded cell range
            LookupTable["AVG"](JRRToken.ToString()); // this calls the function from lookup table
            throw new NotImplementedException();
        }

        private static Queue<FormulaToken> Average(string s)
        {
            throw new NotImplementedException();
        }

        private static Queue<String> ExpandCellRange(String CellRange)
        {
            throw new NotImplementedException();

            int split = GetSplit(CellRange);
            if (split != -1)
            {
                String FirstCell = Extensions.Slice(CellRange, 0, split);
                String LastCell = Extensions.Slice(CellRange, split, CellRange.Length);
                
            }
            else
            {
                System.Console.WriteLine("Format must be: A1:A5");
            }
        }
        private static int GetSplit(String CellRange)
        {
            int split = CellRange.IndexOf(':');
            return split;
        }
        /// <summary>
        /// getRowIndex(string)
        /// Converts Cell row reference to an integer index. 
        /// 
        /// Example:
        ///     getRowIndex("AA") will return 27
        ///     getRowIndex("BC") will return 55
        /// 
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private static int getRowIndex(string Row)
        {
            int lenstr = Row.Length;
            int i;
            int sum = 0;
            if (Row == "")
            {
                return sum;
            }
            else
            {
                i = (Row[0] - 'A' + 1);
                sum = (Extensions.POW(26, lenstr - 1) * i) + getRowIndex(Extensions.Slice(Row, 1, lenstr));
                return sum;

            }
        }
        private static String getRows(string row)
        {
            throw new NotImplementedException();
        }

    }
}
