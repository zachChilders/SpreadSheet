using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinySpreadsheet.Dependencies
{
    /// <summary>
    /// The portion of DependencyMap wrapped around OrderedDictionary functionality.
    /// </summary>
    partial class DependencyMap : IEnumerable
    {
        /// <summary>
        /// Gets the number of dependencies in the map.
        /// </summary>
        public int Count
        {
            get
            {
                return Dependencies.Count;
            }
        }

        //Allows us to easily use the class with square brackets
        /// <summary>
        /// Returns the cell with the given Cell name.
        /// </summary>
        /// <param name="cellName">The column-row name of the Cell in the desired Dependency.</param>
        /// <returns>A Dependency that contains a reference to the desired Cell.</returns>
        public Dependency this[String cellName]
        {
            get
            {
                return (Dependency)Dependencies[cellName];
            }

            set
            {
                Dependencies[cellName] = value;
            }
        }

        /// <summary>
        /// Gets the Dependency at the given index.
        /// </summary>
        /// <param name="index">The index of the Dependency.</param>
        /// <returns>The Dependency at the given index.</returns>
        public Dependency this[int index]
        {
            get
            {
                return (Dependency)Dependencies[index];
            }

            set
            {
                Dependencies[index] = value;
            }
        }

        /// <summary>
        /// Add a new dependency to the map. The Cell name is inferred from the given dependency.
        /// </summary>
        /// <param name="dependency">The dependency to keep track of.</param>
        public void Add(Dependency dependency)
        {
            Dependencies.Add(dependency.Cell.Name, dependency);
        }

        /// <summary>
        /// Add a new dependency to the map with a given name as the key.
        /// </summary>
        /// <param name="cellName">A given name to be used as a key.</param>
        /// <param name="dependency">The dependency to keep track of.</param>
        public void Add(String cellName, Dependency dependency)
        {
            Dependencies.Add(cellName, dependency);
        }

        /// <summary>
        /// Inserts a dependency at a given index.
        /// </summary>
        /// <param name="index">The position for the dependency to be inserted at.</param>
        /// <param name="dependency">The dependency to keep track of.</param>
        public void Insert(int index, Dependency dependency)
        {
            Dependencies.Insert(index, dependency.Cell.Name, dependency);
        }

        /// <summary>
        /// Inserts a dependency at a given index with a given name as the key.
        /// </summary>
        /// <param name="index">The position for the dependency to be inserted at.</param>
        /// <param name="cellName">A given name to be used as a key.</param>
        /// <param name="dependency">The dependency to keep track of.</param>
        public void Insert(int index, String cellName, Dependency dependency)
        {
            Dependencies.Insert(index, cellName, dependency);
        }

        /// <summary>
        /// Removes the dependency with the given key.
        /// </summary>
        /// <param name="key">The key associated with a tracked dependency.</param>
        public void Remove(String key)
        {
            Dependencies.Remove(key);
        }

        /// <summary>
        /// Removes the dependency at the given index.
        /// </summary>
        /// <param name="index">The position of the tracked dependency.</param>
        public void RemoveAt(int index)
        {
            Dependencies.RemoveAt(index);
        }

        //Allows us to use this class in a foreach loop.

        //Apparently this is still needed.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dependencies.Values.GetEnumerator();
        }
    }
}
