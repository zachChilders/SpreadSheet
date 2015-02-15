using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinySpreadsheet.Dependency;

namespace TinySpreadsheet.Tokenize
{
    public static class Tokenizer
    {
        public enum TokenType { CELL, OP, NUM, BANANA };

        public static Queue<FormulaToken> Tokenize(String formula)
        {
            return null;
        }

        public static DependencyMap GetDependencies(String formula)
        {

            return null;
        }

    }
}
