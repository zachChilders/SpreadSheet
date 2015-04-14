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


    }
}
