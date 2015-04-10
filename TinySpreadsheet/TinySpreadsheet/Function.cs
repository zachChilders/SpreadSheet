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
        private static readonly Dictionary<String, Func<Queue<String>, Queue<FormulaToken>>> LookupTable = new Dictionary<String, Func<Queue<String>, Queue<FormulaToken>>>();

        /// <summary>
        /// Constructor builds a lookup table of functions.
        /// </summary>
        static Function()
        {
            LookupTable.Add("AVG", Average); // This adds the Average function under AVG
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jrrToken"></param>
        /// <returns>Result of a given function</returns>
        public static Double Resolve(FormulaToken jrrToken)
        {
            //Get first 3 letters
            //Lookup in dictionary
            //call on expanded cell range
           // LookupTable["AVG"](jrrToken.ToString()); // this calls the function from lookup table
            throw new NotImplementedException();
        }

        /// <summary>
        /// Expands a string like "A1:A3" to A1, A2, A3
        /// </summary>
        /// <param name="cellRange"></param>
        /// <returns>Every Cell in a given range</returns>
        private static Queue<String> ExpandCellRange(String cellRange)
        {
            throw new NotImplementedException();
            int split = GetSplit(cellRange);
            if (split != -1)
            {
                String firstCell = cellRange.Slice(0, split);
                String lastCell = cellRange.Slice(split, cellRange.Length);
                
            }
            else
            {
                System.Console.WriteLine("Format must be: A1:A5");
            }
        }

        /// <summary>
        /// Finds first occurence of ":" in a given string.
        /// </summary>
        /// <param name="cellRange"></param>
        /// <returns>The index of ":"</returns>
        private static int GetSplit(String cellRange)
        {
            int split = cellRange.IndexOf(':');
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
        /// <param name="row"></param>
        /// <returns></returns>
        private static int GetRowIndex(String row)
        {
            int lenstr = row.Length;
            int sum = 0;
            if (row == "")
            {
                return sum;
            }
            int i = (row[0] - 'A' + 1);
            sum = (int) ((Math.Pow(26, lenstr - 1) * i) + GetRowIndex(row.Slice(1, lenstr)));
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static String GetRows(String row)
        {
            throw new NotImplementedException();
        }

    }
}
