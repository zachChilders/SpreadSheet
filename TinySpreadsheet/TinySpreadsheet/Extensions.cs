using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinySpreadsheet.Tokenize;

namespace TinySpreadsheet
{
    /// <summary>
    /// A simple set of extension methods for a variety of classes.
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Resets the StringBuilder to a Length and Capacity of 0.
        /// </summary>
        public static void Clear(this StringBuilder value)
        {
            value.Length = 0;
            value.Capacity = 0;
        }

        /// <summary>
        /// Checks to see if the Stack is empty.
        /// </summary>
        /// <returns>True if empty. False if not empty.</returns>
        public static bool isEmpty(this Stack<FormulaToken> s)
        {
            return (s.Count == 0);
        }

        /// <summary>
        /// Appends a given Queue of FormulaTokens to the end of this Queue.
        /// </summary>
        /// <param name="qIn">The Queue whose items will be appended onto this Queue.</param>
        /// <remarks>The given Queue, qIn, will be completely dequeued after appending.</remarks>
        public static void Append(this Queue<FormulaToken> q, Queue<FormulaToken> qIn)
        {
            while (qIn.Count > 0)
            {
                q.Enqueue(qIn.Dequeue());
            }
        }

        /// <summary>
        /// Creates a substring from the the string, given a start and end index.
        /// </summary>
        /// <param name="start">The starting character index in the string. Inclusive.</param>
        /// <param name="end">The ending character index in the string. Exclusive. Supports negatives.</param>
        /// <returns>The resulting substring from the fiven start and end.</returns>
        /// <remarks>
        /// With end being exclusive, it should be noted that the index would be the index of the letter you'd like to end on +1.
        /// </remarks>
        /// <example>
        /// This example shows how to get "ello" from "Hello, world."
        /// <code>
        ///     String s = "Hello, world.";
        ///     Console.Write(s.Slice(1, 5)); //Prints "ello"
        /// </code>
        /// </example>
        /// <example>
        /// This example shows how to get "world" from "Hello, world." using a negative end.
        /// <code>
        ///     String s = "Hello, world.";
        ///     Console.Write(s.Slice(7, -1)); //Prints "world"
        /// </code>
        /// -1 in this example represents the index of '.', which is the last character in s. -2 would be 'd'.
        /// </example>
        public static string Slice(this string source, int start, int end)
        {
            if (end < 0) // Keep this for negative end support
            {
                end = source.Length + end;
            }
            int len = end - start;               // Calculate length
            return source.Substring(start, len); // Return Substring of length
        }
        /// <summary>
        /// Power function that takes and returns integers.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int POW(int a, int b)
        {
            int ans = a;
            if (b == 0)
            {
                return 1;
            }
            else
            {
                for (int i = 1; i < b; i++)
                {
                    ans *= a;
                }
                return ans;
            }
        }
    }
}
