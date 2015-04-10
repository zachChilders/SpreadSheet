using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TinySpreadsheet
{
    /// <summary>
    /// This manages our momento class, which allows save states.
    /// We can Load, Save, Undo, and Redo from this class.  YAY.
    /// </summary>
    public static class StateManager
    {
        private static readonly Stack<Momento> States = new Stack<Momento>();

        /// <summary>
        /// Adds a momento to the internal stack
        /// </summary>
        public static void SaveState()
        {
            Momento m = new Momento();
            m.Capture();
            States.Push(m);
        }

        /// <summary>
        /// Removes a momento from the stack.
        /// </summary>
        /// <returns></returns>
        public static void RevertState()
        {
            try
            {
               States.Pop();
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine("Can't revert anymore.");
            }
        }

        /// <summary>
        /// Writes the latest momento to a file.
        /// </summary>
        public static void Save()
        {
            States.Peek().Save();
        }

        /// <summary>
        /// Loads the state from a file.
        /// </summary>
        public static void Load()
        {
            States.Clear();
            Momento m = new Momento();
            m.Load();
            States.Push(m);
        }


    }
}
