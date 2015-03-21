using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinySpreadsheet.Tokenize
{
    /// <summary>
    /// A class containing information needed to represent a piece of information from a formula, such as a number, operator, or cell.
    /// </summary>
    public class FormulaToken
    {
        /// <summary>
        /// Creates a new token with the given token string and type.
        /// </summary>
        /// <param name="token">The token string to be saved.</param>
        /// <param name="type">The TokenType associated with the information in token.</param>
        public FormulaToken(String token, Tokenizer.TokenType type)
        {
            Token = token;
            Type = type;
        }

        /// <summary>
        /// Gets the stored token string.
        /// </summary>
        public String Token{ get; private set; }

        /// <summary>
        /// Gets the stored TokenType associated with the stored token string.
        /// </summary>
        public Tokenizer.TokenType Type{ get; private set; }
    }
}
