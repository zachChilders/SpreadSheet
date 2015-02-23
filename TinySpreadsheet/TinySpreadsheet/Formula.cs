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
        private static Regex alphaNum = new Regex(@"^[A-Za-z]+[0-9]+$");

        public static Double solve(Cell c)
        {
            String cellFormulaString = c.CellFormula.Replace(" ", "");
            //if (rgx.IsMatch(cellFormulaString))
            //{
                Queue<FormulaToken> cellFormula = Tokenizer.Tokenize(cellFormulaString);
                Queue<FormulaToken> pfix = postFix(cellFormula); //This should be tokenized somewhere.
                return evaluate(pfix);
            //}
            //return Double.NaN;
        }

        public static String ResolveDependencies(Cell current)
        {
            StringBuilder expandedFormula = new StringBuilder();
            Queue<FormulaToken> tokens = Tokenizer.Tokenize(current.CellFormula);

            foreach (FormulaToken token in tokens)
            {
                if (alphaNum.IsMatch(token.Token))
                {
                    expandedFormula.Append(GetCell(ResolveDependencies(token.Token))); //Go resolve this token first.
                }
                else
                {
                    expandedFormula.Append(token.Token);
                }
            }

            return expandedFormula.ToString();
        }

        private static Double evaluate(Queue<FormulaToken> postFixed)
        {
            Stack<String> eval = new Stack<String>();
            while (postFixed.Count > 0)
            {
                double num1;
                double num2;
                double result;
                FormulaToken currentToken = postFixed.Dequeue();
                switch (currentToken.Token)
                {

                    case "*":
                        num1 = Double.Parse(eval.Pop().ToString());
                        num2 = Double.Parse(eval.Pop().ToString());
                        result = num2 * num1;
                        eval.Push(result.ToString());
                        break;
                    case "/":
                        num1 = Double.Parse(eval.Pop().ToString());
                        num2 = Double.Parse(eval.Pop().ToString());
                        if (num2 != 0)
                        {
                            result = num2 / num1;
                            eval.Push(result.ToString());
                        }
                        else
                        {
                            return Double.NaN;
                        }
                        break;
                    case "+":
                        num1 = Double.Parse(eval.Pop().ToString());
                        num2 = Double.Parse(eval.Pop().ToString());
                        result = num2 + num1;
                        eval.Push(result.ToString());
                        break;
                    case "-":
                        num1 = Double.Parse(eval.Pop().ToString());
                        num2 = Double.Parse(eval.Pop().ToString());
                        result = num2 - num1;
                        eval.Push(result.ToString());
                        break;
                    default:
                        eval.Push(currentToken.Token);
                        break;
                }
            }
            return Double.Parse(eval.Pop());
        }

        private static Queue<FormulaToken> postFix(Queue<FormulaToken> infix)
        {
            Queue<FormulaToken> output = new Queue<FormulaToken>();
            Stack<FormulaToken> stack = new Stack<FormulaToken>();

            int topPrio = 0;
            while (infix.Count > 0)
            {
                Console.WriteLine(output.ToString());
                FormulaToken currentToken = infix.Dequeue();
                int currPrio = priority(currentToken);
                if (!stack.isEmpty())
                {
                    topPrio = priority(stack.Peek());
                }

                if (currPrio == 1) // A digit or Cell
                {
                    output.Enqueue(currentToken);
                }
                else if (currentToken.Token == "(") //Handle parentheses with recursion
                {
                    //We want the NEXT character, not this left banana.
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
                        output.Enqueue(stack.Pop());
                        if (!stack.isEmpty())
                        {
                            topPrio = priority(stack.Peek());
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
                output.Enqueue(stack.Pop());
            }
            return output;
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
