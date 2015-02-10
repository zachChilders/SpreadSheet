using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace TinySpreadsheet
{
    static class Formula
    {
        private static Regex rgx = new Regex(@"^(\(*(\d+[\+\/\-\*])*\d+\)*)([\+\/\-\*](\(*(\d+[\+\/\-\*])*\d+\)*))*$");    //Valid Formula regex check

        public static Double solve(Cell c)
        {
            String cellFormula = c.CellFormula.Replace(" ", "");
            if (rgx.IsMatch(cellFormula))
            {
                string pfix = postFix(cellFormula);
                return evaluate(pfix);
            }
            return Double.NaN;
        }

        private static Double evaluate(String postFixed)
        {
            Stack<String> eval = new Stack<String>();
            foreach(char c in postFixed)
            {
                double num1;
                double num2;
                double result;
                switch(c)
                {

                    case '*':
                        num1 = Double.Parse(eval.Pop().ToString());
                        num2 = Double.Parse(eval.Pop().ToString());
                        result = num1 * num2;
                        eval.Push(result.ToString());
                        break;
                    case '/':
                        num1 = Double.Parse(eval.Pop().ToString());
                        num2 = Double.Parse(eval.Pop().ToString());
                        if (num2 != 0)
                        {
                            result = num1 / num2;
                            eval.Push(result.ToString());
                        }
                        break;
                    case '+':
                        num1 = Double.Parse(eval.Pop().ToString());
                        num2 = Double.Parse(eval.Pop().ToString());
                        result = num1 + num2;
                        eval.Push(result.ToString());
                        break;
                    case '-':
                        num1 = Double.Parse(eval.Pop().ToString());
                        num2 = Double.Parse(eval.Pop().ToString());
                        result = num1 - num2;
                        eval.Push(result.ToString());
                        break;
                    default:
                        eval.Push(c.ToString());
                        break;
                }
            }
            return Double.Parse(eval.Pop());
        }

        private static string postFix(String infix)
        {

            StringBuilder output = new StringBuilder();
            Stack stack = new Stack();
            for (int i = 0; i < infix.Length; i++)
            {
                int topPrio = 0;
                int currPrio = priority(infix[i]);

                if (!stack.isEmpty())
                {
                    topPrio = priority((char)stack.Peek());
                }

                if (currPrio == 1) // A digit or Cell
                {
                    output.Append(infix[i]);
                }
                else if (infix[i] == '(') //Handle parentheses with recursion
                {
                    i++; //We want the NEXT character, not this left banana.
                    //Copy the Parenthesed equation
                    StringBuilder tmp = new StringBuilder();
                    for (int t = i; infix[t] != ')'; t++, i++)
                    {
                        tmp.Append(infix[t]);
                    }
                    //postFix it and add it to our current string
                    output.Append(postFix(tmp.ToString()));

                }
                else if (topPrio >= currPrio) //Higher priority operator
                {
                    while (topPrio >= currPrio && !(stack.isEmpty()))
                    {
                        output.Append(stack.Pop());
                        if (!stack.isEmpty())
                        {
                            topPrio = priority((char)stack.Peek());
                        }
                    }
                    stack.Push(infix[i]);
                }
                else //Normal priority operator, move along.
                {
                    stack.Push(infix[i]);
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

        private static int priority(char op)
        {
            switch (op)
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
