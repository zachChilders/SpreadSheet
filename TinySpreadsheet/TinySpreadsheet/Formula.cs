using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinySpreadsheet
{
    static class Formula
    {

        private static Cell currentCell;
        private static StringBuilder output = new StringBuilder();

        public static void solve(Cell c)
        {
            currentCell = c;
            evaluate();
        }

        private static void evaluate()
        {
            Console.WriteLine(currentCell.thisFormula);
           
        }

        private static string postFix(String infix)
        {
            
            
            foreach (char c in infix)
            {
                output.Append(c);
            }
            String result = output.ToString();
            output.Clear();
            return result;
        }

    }
}
