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

        public static string Slice(this string source, int start, int end)
        {
            if (end < 0) // Keep this for negative end support
            {
                end = source.Length + end;
            }
            int len = end - start;               // Calculate length
            return source.Substring(start, len); // Return Substring of length
        }
    }
}
