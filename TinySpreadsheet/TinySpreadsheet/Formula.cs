using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TinySpreadsheet.Tokenize;


namespace TinySpreadsheet
{
    /// <summary>
    /// A static class for validating and evaluating cell inputs.
    /// </summary>
    static class Formula
    {
        private static readonly Regex Rgx = new Regex(@"^([A-Z]{3}?(-{0,1}\()*((\d|[A-Z]\d+)+[\+\/\-\*\:])*(-{0,1}\d|[A-Z]\d+)+\)*)([\+\/\-\*](\(*((\d|[A-Z]\d+)+[\+\/\-\*])*-{0,1}(\d|[A-Z]\d+)+\)*))*$");    //Valid Formula regex check

        /// <summary>
        /// Attempts to evaluate a cell using its current input. 
        /// </summary>
        /// <param name="c">The Cell to be evaluated.</param>
        /// <returns>A Double representing the solved value.</returns>
        /// <remarks>A Double is returned as a value representation of the input. If the input cannot be recognized as something that can be evaluated to a number, Double.NaN is returned.</remarks>
        public static Double? Solve(Cell c)
        {
            String cellFormulaString = c.CellFormula.Replace(" ", "");
            cellFormulaString = cellFormulaString.Replace("=", "");
            c.CellFormula = cellFormulaString;
            if (!Rgx.IsMatch(cellFormulaString))
            {
                return null;
            }

            Queue<FormulaToken> cellFormula = ResolveDependencies(Tokenizer.Tokenize(cellFormulaString));
            Queue<FormulaToken> pfix = PostFix(cellFormula); //This should be tokenized somewhere.

            try
            {
                return Evaluate(pfix);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return Double.NaN;
            }
          
        }

        /// <summary>
        /// Converts any cell references inside a given FormulaToken queue to their respective numerical values.
        /// </summary>
        /// <param name="tokens">A queue of FormulaTokens.</param>
        /// <returns>A new queue of FormulaTokens with Cells substituted with their values.</returns>
        private static Queue<FormulaToken> ResolveDependencies(Queue<FormulaToken> tokens)
        {
            Queue<FormulaToken> outTokens = new Queue<FormulaToken>();
            while(tokens.Count > 0)
            {
                FormulaToken token = tokens.Dequeue();
                if (token.Type == Tokenizer.TokenType.CELL) //If it's a cell, replace with that cells formula
                {
                    if (token.Token[0] == '-')
                    {
                        Double cellContents = Double.Parse(Tokenizer.ExtractCell(token.Token.Substring(1)).CellDisplay);
                        cellContents *= -1;
                        outTokens.Enqueue(new FormulaToken(cellContents.ToString(), Tokenizer.TokenType.NUM));
                    }
                    else
                    {
                        outTokens.Enqueue(new FormulaToken(Tokenizer.ExtractCell(token.Token).CellDisplay, Tokenizer.TokenType.NUM));
                    }
                }
                else
                {
                    outTokens.Enqueue(token);
                }
            }

            return outTokens;
        }

        /// <summary>
        /// Attempts to evaluate a postfixed queue of FormulaTokens. It's expected that Formula.postFix() was used.
        /// </summary>
        /// <param name="postFixed">Postfixed queue of FormulaToken.</param>
        /// <returns>A Double representing the solved value of the given formula.</returns>
        private static Double Evaluate(Queue<FormulaToken> postFixed)
        {
            Stack<String> eval = new Stack<String>();
            while (postFixed.Count > 0)
            {
                double num1;
                double num2;
                double result;
                const double tolerance = 0.000000001;
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
                        
                        if (Math.Abs(num2) > tolerance)
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

        /// <summary>
        /// Rearranges a given queue of FormulaTokens to be in the postfix format.
        /// </summary>
        /// <param name="infix">A queue of infixed FormulaTokens.</param>
        /// <returns>A postfixed version of the input.</returns>
        private static Queue<FormulaToken> PostFix(Queue<FormulaToken> infix)
        {
            Queue<FormulaToken> output = new Queue<FormulaToken>();
            Stack<FormulaToken> stack = new Stack<FormulaToken>();

            int topPrio = 0;
            while (infix.Count > 0)
            {
                Console.WriteLine(output.ToString());
                FormulaToken currentToken = infix.Dequeue();
                int currPrio = Priority(currentToken);
                if (!stack.IsEmpty())
                {
                    topPrio = Priority(stack.Peek());
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
                    output.Append(PostFix(tmp));
                }
                else if (topPrio >= currPrio) //Higher priority operator
                {
                    while (topPrio >= currPrio && !(stack.IsEmpty()))
                    {
                        output.Enqueue(stack.Pop());
                        if (!stack.IsEmpty())
                        {
                            topPrio = Priority(stack.Peek());
                        }
                    }
                    stack.Push(currentToken);
                }
                else //Normal priority operator, move along.
                {
                    stack.Push(currentToken);
                }

            }

            while (!stack.IsEmpty())
            {
                output.Enqueue(stack.Pop());
            }
            return output;
        }

        /// <summary>
        /// Determines the priority of a given FormulaToken for postfixing purposes.
        /// </summary>
        /// <param name="op">The FormulaToken to be prioritized.</param>
        /// <returns>An integer representation of priority for the given token.</returns>
        private static int Priority(FormulaToken op)
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
