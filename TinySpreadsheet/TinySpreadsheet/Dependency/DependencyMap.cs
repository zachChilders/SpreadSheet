using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace TinySpreadsheet.Dependency
{
    /// <summary>
    /// A map of Dependencies describing cells and their relationships as dependencies.
    /// </summary>
    public class DependencyMap : IEnumerable<Dependency>
    {
        private Action subscribeCallback;

        public DependencyMap()
        {
            Dependencies = new OrderedDictionary();
        }

        /// <summary>
        /// Our map of dependencies
        /// </summary>
        public OrderedDictionary Dependencies { get; private set; }

        //Our properties
        /// <summary>
        /// Gets the owner of the dependencies in this map.
        /// </summary>
        public Cell Owner { get; private set; }

        /// <summary>
        /// Gets or sets the callback used for when dependencies change their values.
        /// </summary>
        public Action SubscribeCallback { get; set; } //Needs implementing

        /// <summary>
        /// Gets or sets the callback for what happens when an error occurs somewhere
        /// </summary>
        public Action ErrorCallback { get; set; }

        //Our methods
        public void Unsubscribe()
        {

        }

        private bool Subscribe()
        {
            return false;
        }


        //Methods and properties that wrap around the OrderedDictionary

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
        public IEnumerator<Dependency> GetEnumerator()
        {
            return (IEnumerator<Dependency>)Dependencies.Values.GetEnumerator();
        }
    }
}
