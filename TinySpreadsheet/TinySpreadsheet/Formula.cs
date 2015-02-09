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
            for (int i = 0; i < infix.Length; i++)
            {

                int topPrio = 0;
                if (!stack.isEmpty())
                {
                    topPrio = priority((char)stack.Peek());
                }
                int currPrio = priority(infix[i]);
                if (topPrio < priority(infix[i])) //Low Priority
                {
                    stack.Push(infix[i]);
                }
                else if (infix[i] == '(')
                {
                    //Copy the Parenthesed equation
                    StringBuilder tmp = new StringBuilder();
                    for (int t = i; infix[t] != ')'; t++)
                    {
                        tmp.Append(infix[t]);
                        i += t;
                    }
                    //postFix it and add it to our current string
                    output.Append(postFix(tmp.ToString()));
                    
                }
                else if (topPrio >= currPrio)
                {
                    char c = (char)stack.Pop();
                    while (priority(c) < currPrio)
                    {
                        output.Append(c);
                        c = (char)stack.Pop();
                    }
                }
                else
                {
                    stack.Push(infix[i]);
                }
            
            }

            while (!stack.isEmpty())
            {
                output.Append(stack.Pop());
            }
            String result = output.ToString();
            Console.WriteLine(result);
            output.Clear();
            return result;
        }

        private static int priority(char op)
        {
            switch(op)
            {
                case '(':
                case ')':
                    return 4;
                case '*':
                case '/':
                    return 3;
                case '+':
                case '-':
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
