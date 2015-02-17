using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinySpreadsheet.Tokenize;

namespace TinySpreadsheet
{
    static class Extensions
    {

         /// <summary>
        ///     Clears the contents of the string builder.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="StringBuilder"/> to clear.
        /// </param>
        public static void Clear(this StringBuilder value)
        {
            value.Length = 0;
            value.Capacity = 0;
        }

        public static bool isEmpty(this Stack<FormulaToken> s)
        {
            return (s.Count == 0);
        }

        public static void Append(this Queue<FormulaToken> q, Queue<FormulaToken> qIn)
        {
            while (qIn.Count > 0)
            {
                q.Enqueue(qIn.Dequeue());
            }
        }
    }
}
