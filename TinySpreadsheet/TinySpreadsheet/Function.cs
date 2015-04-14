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
        private static readonly Dictionary<String, Func<Queue<String>, String>> LookupTable = new Dictionary<String, Func<Queue<String>,String>>();

        /// <summary>
        /// Constructor builds a lookup table of functions.
        /// </summary>
        static Function()
        {
            LookupTable.Add("AVG", Average); // This adds the Average function under AVG
            LookupTable.Add("SUM", Sum);
            LookupTable.Add("MUL", Product);
            LookupTable.Add("RND", Random);
            LookupTable.Add("SIN", Sine);
            LookupTable.Add("COS", Cosine);
            LookupTable.Add("TAN", Tangent);
            LookupTable.Add("LOG", Logarithm);
            LookupTable.Add("SRT", SquareRoot);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jrrToken"></param>
        /// <returns>Result of a given function</returns>
        public static Queue<FormulaToken> Resolve(FormulaToken jrrToken)
        {
            String macro = jrrToken.Token.Substring(0, 3);
            String cells = jrrToken.Token.Replace("(", "");
            cells = cells.Replace(")", "");
            cells = cells.Slice(3, cells.Length);
            return Tokenizer.Tokenize(LookupTable[macro](ExpandCellRange(cells))); // this calls the function from lookup table
            
        }

        /// <summary>
        /// Expands a string like "A1:A3" to A1, A2, A3
        /// </summary>
        /// <param name="cellRange"></param>
        /// <returns>Every Cell in a given range</returns>
        private static Queue<String> ExpandCellRange(String cellRange)
        {
            int split = cellRange.IndexOf(':');
            if (split != -1)
            {
                //split range into two cells
                string firstCell = cellRange.Slice(0, split);
                string lastCell = cellRange.Slice(split, cellRange.Length);

                //get column and row values
                int firstCol = getColumnIndex(getColumn(firstCell));
                int lastCol = getColumnIndex(getColumn(lastCell));
                int firstRow = getRow(firstCell);
                int lastRow = getRow(lastCell);

                Queue<String> Cells = new Queue<String>();
                // insert  to queue
                for (int i = firstCol; i <= lastCol; i++)
                {
                    for (int j = firstRow; j <= lastRow; j++)
                    {
                        String cell = MainWindow.GenerateName(i);
                        String row = j.ToString();
                        cell = cell + row;
                        Cells.Enqueue(cell);
                    }
                }
                return Cells;

            }

            else
            {
                System.Console.WriteLine("Format must be: A1:A5");
                return null;
            }

        }

        /// <summary>
        /// getRowIndex(string)
        /// Converts Cell row reference to an integer index. 
        /// 
        /// Example:
        ///     getRowIndex("AA") will return 27
        ///     getRowIndex("BC") will return 55
        /// </summary>
        private static int getColumnIndex(String col)
        {
            int lenstr = col.Length;
            int sum = 0;
            if (col == "")
            {
                return sum;
            }
            else
            {
                int i = (col[0] - 'A' + 1);
                sum = (Extensions.Pow(26, lenstr - 1) * i) + getColumnIndex(col.Slice(1, lenstr));
                return sum;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static string getColumn(string cell)
        {
            System.Text.StringBuilder column = new System.Text.StringBuilder();
            foreach (char c in cell)
            {
                if (Char.IsLetter(c))
                {
                    column.Append(c);
                }
            }

            return column.ToString();
        }

        private static int getRow(string cell)
        {
            System.Text.StringBuilder row = new System.Text.StringBuilder();
            foreach (char c in cell)
            {
                if (Char.IsNumber(c))
                {
                    row.Append(c);
                }

            }
            int thisrow = int.Parse(row.ToString());
            return thisrow;
        }


    }
}