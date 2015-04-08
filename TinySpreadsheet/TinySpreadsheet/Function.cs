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
                String FirstCell = CellRange.Slice(0, split);
                String LastCell = CellRange.Slice(split, CellRange.Length);
                
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
        /// Example:
        ///     A => (i)^1 = 1
        ///     AA = (i)^2 + (i)^1= 27
        ///     AAA = (i)^
        /// The Function is recursive.
        /// 
        ///     
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private static int getRowIndex(string Row)
        {
            int sum = 0;
            int lenstr = Row.Length;
            int i = (Row[0] - 'A' + 1);
            if (Row[0] != null)
            {
                sum = (Extensions.POW(26, lenstr - 1) * i) + getRowIndex(Extensions.Slice(Row,1,lenstr));
            }
            return sum;
        }
        private static String getRows(string row)
        {
            throw new NotImplementedException();
        }
        private static string Slice(string Row, int p, double lenstr)
        {
            throw new NotImplementedException();
        }
    }
}
