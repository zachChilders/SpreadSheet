using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinySpreadsheet.Tokenize
{
    public class FormulaToken
    {
        public FormulaToken(String token, Tokenizer.TokenType type)
        {
            Token = token;
            Type = type;
        }

        public String Token{ get; private set; }
        public Tokenizer.TokenType Type{ get; private set; }
    }
}
