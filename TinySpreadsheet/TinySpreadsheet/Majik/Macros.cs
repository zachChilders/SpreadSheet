using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinySpreadsheet.Spreadsheet.Components;
using TinySpreadsheet.Tokenize;

namespace TinySpreadsheet.Majik
{
    public static partial class Function
    {
        /// <summary>
        /// Returns the average of a cell range.
        /// </summary>
        /// <param name="s">A queue of cell names.</param>
        /// <returns>An average formula based on the cell range given. Given (A1:A3), ((A1 + A2 + A3)/3) is returned.</returns>
        private static String Average(Queue<String> s)
        {
            StringBuilder sum = new StringBuilder();
            sum.Append("((");
            foreach (Cell c in s.Select(Tokenizer.ExtractCell))
            {
                sum.Append(c.CellFormula);
                sum.Append("+");
            }
            sum.Remove(sum.Length - 1, 1);
            sum.Append(")/");
            sum.Append(s.Count.ToString());
            sum.Append(")");
            return sum.ToString();
        }

        /// <summary>
        /// Returns the sum of a cell range.
        /// </summary>
        /// <param name="s">A queue of cell names.</param>
        /// <returns>A sum formula based on the given cell range. Given (A1:A3), (A1 + A2 + A3) is returned.</returns>
        private static String Sum(Queue<String> s)
        {
            StringBuilder sum = new StringBuilder();
            sum.Append("(");
            foreach (Cell c in s.Select(Tokenizer.ExtractCell))
            {
                sum.Append(c.CellFormula);
                sum.Append("+");
            }
            sum.Remove(sum.Length - 1, 1);
            sum.Append(")");
            return sum.ToString();
        }

        /// <summary>
        /// Returns the product of a cell range.
        /// </summary>
        /// <param name="s">A queue of cell names.</param>
        /// <returns>A product formula based on the given cell range. Given (A1:A3), (A1 * A2 * A3) is returned.</returns>
        private static String Product(Queue<String> s)
        {
            StringBuilder sum = new StringBuilder();
            sum.Append("(");
            foreach (Cell c in s.Select(Tokenizer.ExtractCell))
            {
                sum.Append(c.CellFormula);
                sum.Append("*");
            }
            sum.Remove(sum.Length - 1, 1);
            sum.Append(")");
            return sum.ToString();
        }

        /// <summary>
        /// Returns a random number.
        /// </summary>
        /// <param name="s">This should be an empty queue.</param>
        /// <returns>Returns a random number as a string.</returns>
        private static String Random(Queue<String> s)
        {
            if (s.Count != 0)
            {
                return "NaN";
            }
            Random r = new Random();
            return r.Next().ToString();
        }

        /************
         *Uhhhhh....*
         ************/
        private static String Sine(Queue<String> s)
        {
            if (s.Count != 1)
            {
                return "NaN";
            }
            return Math.Sin(Formula.Solve(Tokenizer.ExtractCell(s.Dequeue())).Value).ToString();///uhhhhh....
        }

        private static String Cosine(Queue<String> s)
        {
            if (s.Count != 1)
            {
                return "NaN";
            }
            return Math.Cos(Formula.Solve(Tokenizer.ExtractCell(s.Dequeue())).Value).ToString();///uhhhhh....
        }

        private static String Tangent(Queue<String> s)
        {
            if (s.Count != 1)
            {
                return "NaN";
            }
            return Math.Tan(Formula.Solve(Tokenizer.ExtractCell(s.Dequeue())).Value).ToString();///uhhhhh....
        }

        private static String Logarithm(Queue<String> s)
        {
            if (s.Count != 1)
            {
                return "NaN";
            }
            return Math.Log10(Formula.Solve(Tokenizer.ExtractCell(s.Dequeue())).Value).ToString();///uhhhhh....
        }

        private static String SquareRoot(Queue<String> s)
        {
            if (s.Count != 1)
            {
                return "NaN";
            }
            return Math.Sqrt(Formula.Solve(Tokenizer.ExtractCell(s.Dequeue())).Value).ToString();///uhhhhh....
        }


    }
}
