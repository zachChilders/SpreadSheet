using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace TinySpreadsheet.Dependencies
{
    /// <summary>
    /// A map of Dependencies describing cells and their relationships as dependencies.
    /// </summary>
    public partial class DependencyMap
    {
        private Action subscribeCallback;

        public DependencyMap(Cell owner)
        {
            Dependencies = new OrderedDictionary();
            Owner = owner;
            subscribeCallback = null;
            ErrorCallback = null;
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
        public Action SubscribeCallback 
        {
            get
            {
                throw new NotImplementedException();
            }
            set{}
        }

        /// <summary>
        /// Gets or sets the callback for what happens when an error occurs somewhere
        /// </summary>
        public Action ErrorCallback { get; set; }

        //Our methods
        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        private bool Subscribe()
        {
            throw new NotImplementedException();
        }
    }
}
