using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace TinySpreadsheet
{
    static class Formula
    {

        private static Cell currentCell;
        private static StringBuilder output = new StringBuilder();
        private static Stack stack = new Stack();

        public static void solve(Cell c)
        {
            currentCell = c;
            evaluate();
        }

        private static void evaluate()
        {

            currentCell.cellFormula = postFix(currentCell.cellFormula);    
        }

        private static string postFix(String infix)
        {
            
            foreach (char c in infix)
            {
                if (char.IsDigit(c))  //It's either a digit
                {
                    stack.Push(c);
                }
                else if (stack.Count > 2) //
                {
                    output.Append(stack.Pop());
                    output.Append(stack.Pop());
                    output.Append(c);
                }
                else if (stack.isEmpty())
                {
                    stack.Push(c);
                }
            
            }

            while (!stack.isEmpty())
            {
                output.Append(stack.Pop());
            }
            String result = output.ToString();
            output.Clear();
            return result;
        }

    }
}
