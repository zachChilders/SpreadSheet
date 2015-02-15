using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinySpreadsheet.Dependency;

namespace TinySpreadsheet
{
    public static class Tokenizer
    {
        public enum TokenType { CELL, OP, NUM, BANANA };

        public static Queue<Tuple<String, Tokenizer.TokenType>> Tokenize(String formula)
        {

        }

        public static DependencyMap GetDependencies(String formula)
        {

            return null;
        }

    }
}
