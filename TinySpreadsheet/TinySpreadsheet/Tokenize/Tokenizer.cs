using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using TinySpreadsheet.Dependencies;

namespace TinySpreadsheet.Tokenize
{
    public static class Tokenizer
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum TokenType { CELL, OP, NUM, LBANANA, RBANANA, MACRO };

        /// <summary>
        /// Determines if the given character is an operator.
        /// </summary>
        /// <param name="c">The character to be evaluated.</param>
        /// <returns>A FormulaToken of the given character. If c is not an operator, null is returned.</returns>
        private static FormulaToken GetOp(char c)
        {
            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                    return new FormulaToken(c.ToString(), TokenType.OP);
                case '(':
                    return new FormulaToken(c.ToString(), TokenType.LBANANA);
                case ')':
                    return new FormulaToken(c.ToString(), TokenType.RBANANA);
                default:
                    return null;
            }

        }

        /// <summary>
        /// Creates a FormulaToken from a given string.
        /// </summary>
        /// <param name="s">String containing non-operators.</param>
        /// <returns>A FormulaToken of type NUM if s is a number, or type CELL if not a number.</returns>
        private static FormulaToken GetNotOp(String s)
        {
            double d;
            return double.TryParse(s, out d) ? new FormulaToken(s, TokenType.NUM) : new FormulaToken(s, TokenType.CELL);
        }

        ///Incomplete
        public static bool IsMinus(FormulaToken token)
        {
            return token.Token == "-";
        }


        /// <summary>
        /// Creates a Queue of FormulaTokens from a given formula string.
        /// </summary>
        /// <param name="formula">A string with a valid basic mathematical formula.</param>
        /// <returns>A Queue of FormulaTokens in the order they appear in the given formula string.</returns>
        public static Queue<FormulaToken> Tokenize(String formula)
        {
            Queue<FormulaToken> tokenQueue = new Queue<FormulaToken>();
            StringBuilder num = new StringBuilder();

            for (int i = 0; i < formula.Length; i++)
            {
                char c = formula[i];

                FormulaToken thisop;
                //If two letters are next to each other, it's probably a macro
                if (IsMacro(ref formula, i))
                {
                    while (formula[i] != ')')
                    {
                        num.Append(formula[i]);
                        i++;
                    }
                    tokenQueue.Append(Function.Resolve(new FormulaToken(num.ToString(), TokenType.MACRO)));
                    num.Clear();

                }
                else if ((thisop = GetOp(c)) != null)
                {
                    if (num.Length > 0)
                    {
                        num.Insert(0, (""));
                        tokenQueue.Enqueue(GetNotOp(num.ToString()));
                        num.Clear();
                    }
                    tokenQueue.Enqueue(thisop);
                }
                else
                    num.Append(c);
            }

            if (num.Length > 0)
                tokenQueue.Enqueue(GetNotOp(num.ToString()));

            return tokenQueue;

        }

        /// <summary>
        /// Finds any cell dependencies for the given cell.
        /// </summary>
        /// <param name="cell">A cell to find dependencies with.</param>
        /// <returns>A map of all connected dependencies.</returns>
        public static DependencyMap GetDependencies(Cell cell)
        {
            String formula = cell.CellFormula;
            Queue<FormulaToken> tokens = Tokenize(formula);
            DependencyMap dependencies = new DependencyMap(cell);

            foreach (FormulaToken t in tokens)
            {
                if (t.Type == TokenType.CELL)
                {
                    //Find the cell reference from map of SpreadSheet with index
                    dependencies.Add(new Dependency(ExtractCell(t.Token.Replace("-", "")), true));
                }
            }

            //For each direct dependency in dependencies, add their dependencies to ours as indirect.
            int count = dependencies.Count;
            for (int i = 0; i < count; i++)
            {
                foreach (Dependency d in dependencies[i].Cell.Dependencies)
                {
                    //Overwrites or adds a new Cell. By overwriting it to indirect, we can reduce the number of potential calls for Change.
                    dependencies[d.Cell.Name] = new Dependency(d.Cell, false);
                }
            }

            return dependencies;
        }

        /// <summary>
        /// Extracts a cell using the static column map.
        /// </summary>
        /// <param name="cellName">The name of the cell for identification.</param>
        /// <returns>The referenced cell.</returns>
        public static Cell ExtractCell(String cellName)
        {
            StringBuilder column = new StringBuilder();
            StringBuilder row = new StringBuilder();

            //Go through each character and append to the appropriate StringBuilder
            foreach (char c in cellName)
            {
                if (Char.IsLetter(c))
                    column.Append(c);
                else
                    row.Append(c);
            }

            //Get the index of the cell in the column.
            int index;
            if (!Int32.TryParse(row.ToString(), out index))
                throw new Exception("Not a cell");

            return MainWindow.SpreadSheet[column.ToString().ToUpper()][index - 1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool IsMacro(ref String formula, int i)
        {
            try
            {
                //If the given character is a letter followed by two  more letters and a (, it's likely a macro
                //SUM(, AVG(
                return Char.IsLetter(formula[i]) && Char.IsLetter(formula[i + 1]) && Char.IsLetter(formula[i + 2]) &&
                       (formula[i + 3] == '(');
            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }

        }

    }
}
