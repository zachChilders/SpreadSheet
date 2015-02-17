using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TinySpreadsheet.Tokenize;


namespace TinySpreadsheet
{
    static class Formula
    {
        private static Regex rgx = new Regex(@"^(\(*(\d+[\+\/\-\*])*\d+\)*)([\+\/\-\*](\(*(\d+[\+\/\-\*])*\d+\)*))*$");    //Valid Formula regex check

        public static Double solve(Cell c)
        {
            String cellFormulaString = c.CellFormula.Replace(" ", "");
            if (rgx.IsMatch(cellFormulaString))
            {
                Queue<FormulaToken> cellFormula = Tokenizer.Tokenize(cellFormulaString);
                String pfix = postFix(cellFormula); //This should be tokenized somewhere.
                return evaluate(pfix);
            }
            return Double.NaN;
        }

        private static Double evaluate(String postFixed)
        {
            Stack<String> eval = new Stack<String>();
            foreach (char c in postFixed)
            {
                double num1;
                double num2;
                double result;
                switch (c)
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
                        else
                        {
                            return Double.NaN;
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

        private static String postFix(Queue<FormulaToken> infix)
        {
              
            StringBuilder output = new StringBuilder();
            Stack stack = new Stack();

            int topPrio = 0;
            while (infix.Count > 0)
            {
                FormulaToken currentToken = infix.Dequeue();
                int currPrio = priority(currentToken);
                if (!stack.isEmpty())
                {
                    topPrio = priority((FormulaToken)stack.Peek());
                }

                if (currPrio == 1) // A digit or Cell
                {
                    output.Append(currentToken);
                }
                else if (currentToken.Token == "(") //Handle parentheses with recursion
                {
                    //i++; //We want the NEXT character, not this left banana.
                    currentToken = infix.Dequeue();
                    //Copy the Parenthesed equation
                    Queue<FormulaToken> tmp = new Queue<FormulaToken>();
                    while (currentToken.Token != ")")
                    {
                        tmp.Enqueue(currentToken);
                        currentToken = infix.Dequeue();
                    }
                    output.Append(postFix(tmp));

                }
                else if (topPrio >= currPrio) //Higher priority operator
                {
                    while (topPrio >= currPrio && !(stack.isEmpty()))
                    {
                        output.Append(stack.Pop());
                        if (!stack.isEmpty())
                        {
                            topPrio = priority((FormulaToken)stack.Peek());
                        }
                    }
                    stack.Push(currentToken);
                }
                else //Normal priority operator, move along.
                {
                    stack.Push(currentToken);
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

        private static int priority(FormulaToken op)
        {
            switch (op.Token)
            {
                case "(":
                case ")":
                    return 4;
                case "*":
                case "/":
                    return 3;
                case "+":
                case "-":
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
