using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinySpreadsheet.Dependency
{
    partial class DependencyMap : IEnumerable<Dependency>
    {
        public int Count
        {
            get
            {
                return Dependencies.Count;
            }
        }

        //Allows us to easily use the class with square brackets
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
        System.Collections.Generic.IEnumerator<Dependency> System.Collections.Generic.IEnumerable<Dependency>.GetEnumerator()
        {
            return (IEnumerator<Dependency>)Dependencies.Values.GetEnumerator();
        }

        //Apparently this is still needed.
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Dependencies.Values.GetEnumerator();
        }
    }
}
