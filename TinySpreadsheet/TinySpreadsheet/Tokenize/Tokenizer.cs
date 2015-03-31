using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinySpreadsheet.Dependencies;

namespace TinySpreadsheet.Tokenize
{
    public static class Tokenizer
    {
        public enum TokenType { CELL, OP, NUM, LBANANA, RBANANA };

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
            if (double.TryParse(s, out d))
            {
                return new FormulaToken(s, TokenType.NUM);
            }
            else
            {
                return new FormulaToken(s, TokenType.CELL);
            }

        }

        ///Incomplete
        public static bool IsMinus(FormulaToken token)
        {
            if (token.Token == "-")
                return true;
            else return false;
        }

        ////Incomplete
        //public static Queue<FormulaToken> DistributeNeg(String inBanana)
        //{
        //    FormulaToken thistoken;
        //    // check for leading negative
        //    for ( int i = 0; i < inBanana.Length; i++)
        //    {
        //        char c = inBanana[i];
        //        char prevchar = inBanana[i];
        //        if (isminus(getOP(c)) )  // && is num or cell
        //            //make negative
                
        //    }
        //    // check for negative after operator
        //    // check left banana
        //        //
        //        //distribute negative within banana

        //        return s;
        //}

        /// <summary>
        /// Creates a Queue of FormulaTokens from a given formula string.
        /// </summary>
        /// <param name="formula">A string with a valid basic mathematical formula.</param>
        /// <returns>A Queue of FormulaTokens in the order they appear in the given formula string.</returns>
        public static Queue<FormulaToken> Tokenize(String formula)
        {
            Queue<FormulaToken> TokenQueue = new Queue<FormulaToken>();
            StringBuilder num = new StringBuilder();
            bool lastOP = false;
            bool isNegative = false;
            for (int i = 0; i < formula.Length; i++ )
            {
                char c = formula[i];

                FormulaToken thisop;

                if ((thisop = GetOp(c)) != null)
                {
                    if (num.Length > 0)
                    {
                        num.Insert(0, (isNegative ? "-" : ""));
                        TokenQueue.Enqueue(GetNotOp(num.ToString()));
                        num.Clear();
                    }
                    TokenQueue.Enqueue(thisop);
                }
                else
                    num.Append(c);
            }

            if (num.Length > 0)
                TokenQueue.Enqueue(GetNotOp(num.ToString()));

            return TokenQueue;

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
                    //Find the cell reference from map of Columns with index
                    dependencies.Add(new Dependency(ExtractCell(t.Token.Replace("-","")), true));
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
            
            return MainWindow.Columns[column.ToString().ToUpper()][index - 1];
        }

    }
}
