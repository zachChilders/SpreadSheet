using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinySpreadsheet.Dependency
{
    public class Dependency
    {
        Tuple<Cell, bool> dependency;

        /// <summary>
        /// Instantiates a new dependency with a given Cell and bool.
        /// </summary>
        /// <param name="c">The cell reference to associate with a type of dependency.</param>
        /// <param name="direct">The type of dependency indicating whether it's direct or indirect.</param>
        public Dependency(Cell c, bool direct)
        {
            dependency = new Tuple<Cell, bool>(c, direct);
        }

        /// <summary>
        /// Gets the cell associated with this dependency.
        /// </summary>
        public Cell Cell
        {
            get
            {
                return dependency.Item1;
            }
        }

        /// <summary>
        /// Gets whether this is a direct dependency or not.
        /// </summary>
        public bool IsDirect
        {
            get
            {
                return dependency.Item2;
            }
        }
    }
}
