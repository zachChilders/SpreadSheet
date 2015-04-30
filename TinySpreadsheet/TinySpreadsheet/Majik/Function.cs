using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinySpreadsheet.Spreadsheet;
using TinySpreadsheet.Tokenize;

namespace TinySpreadsheet.Majik
{
    public static partial class Function
    {
        private static readonly SerializableDictionary<String, Func<Queue<String>, String>> LookupTable = new SerializableDictionary<string, Func<Queue<string>, string>>();

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
        /// Resolves a given FormulaToken as a macro.
        /// </summary>
        /// <param name="jrrToken">The macro token.</param>
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
        /// <param name="cellRange">String indicating a cell range.</param>
        /// <returns>Every Cell in a given range</returns>
        public static Queue<String> ExpandCellRange(String cellRange)
        {

            Queue<String> Cells = new Queue<String>();
            int split = cellRange.IndexOf(':');
            if (split != -1)
            {
                //split range into two cells
                string firstCell = cellRange.Slice(0, split);
                string lastCell = cellRange.Slice(split, cellRange.Length);

                //get column and row values
                int firstCol = GetColumnIndex(GetColumn(firstCell));
                int lastCol = GetColumnIndex(GetColumn(lastCell));
                int firstRow = getRow(firstCell);
                int lastRow = getRow(lastCell);

                // insert  to queue
                for (int i = firstCol; i <= lastCol; i++)
                {
                    for (int j = firstRow; j <= lastRow; j++)
                    {
                        String cell = SpreadsheetWindow.GenerateName(i);
                        String row = j.ToString();
                        cell = cell + row;
                        Cells.Enqueue(cell);
                    }
                }
                return Cells;

            }
            else
            {
                Cells.Enqueue(cellRange);
                return Cells;
            }

        }

        /// <summary>
        /// getRowIndex(string)
        /// Converts Cell row reference to an integer index. 
        /// </summary>
        /// <example>
        /// getRowIndex("AA") will return 27
        /// getRowIndex("BC") will return 55
        /// </example>
        private static int GetColumnIndex(String col)
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
                sum = (Extensions.Pow(26, lenstr - 1) * i) + GetColumnIndex(col.Slice(1, lenstr));
                return sum;

            }
        }

        /// <summary>
        /// Given a cell name, get the associated column name.
        /// </summary>
        /// <param name="cell">A cell name</param>
        /// <returns>The column portion of the cell's name.</returns>
        private static string GetColumn(string cell)
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

        /// <summary>
        /// Given a cell name, get the associated row name.
        /// </summary>
        /// <param name="cell">A cell name</param>
        /// <returns>The row portion of the cell's name.</returns>
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