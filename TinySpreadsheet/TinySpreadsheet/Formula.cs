using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace TinySpreadsheet
{
    static class Formula
    {

        public static void solve(Cell c)
        {
            //currentCell = c;
            evaluate(c);
        }

        private static void evaluate(Cell c)
        {
            Regex rgx = new Regex(@"^(\(*(\d+[\+\/\-\*])*\d+\)*)([\+\/\-\*](\(*(\d+[\+\/\-\*])*\d+\)*))*$");    //Valid Formula regex check
            if (rgx.IsMatch(c.CellFormula))
            {
                string pfix = postFix(c.CellFormula);
                Console.WriteLine(pfix);        //Do something with formula
            }   
        }

        private static string postFix(String infix)
        {
            StringBuilder output = new StringBuilder();
            Stack stack = new Stack();
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
