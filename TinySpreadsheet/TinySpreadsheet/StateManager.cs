using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TinySpreadsheet
{
    public static class StateManager
    {
        private static readonly Stack<Momento> States = new Stack<Momento>();

        /// <summary>
        /// Adds a momento to the internal stack
        /// </summary>
        private static void Add()
        {
            Momento m = new Momento();
            m.Capture();
            States.Push(m);
        }

        /// <summary>
        /// Removes a momento from the stack.
        /// </summary>
        /// <returns></returns>
        private static Momento Remove()
        {
            Momento newState = null;
            try
            {
                newState = States.Pop();
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine("Can't restore anymore.");
            }
            return newState;
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
