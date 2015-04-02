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
        }


    }
}
