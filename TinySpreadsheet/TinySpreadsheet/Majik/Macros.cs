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
        /// <param name="s"></param>
        /// <returns>((A1 + A2 + A3)/3)</returns>
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
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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

        private static String Random(Queue<String> s)
        {
            if (s.Count != 0)
            {
                return "NaN";
            }
            Random r = new Random();
            return r.Next().ToString();
        }

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
