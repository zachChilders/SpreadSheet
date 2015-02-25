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
        private Action<Cell> subscribeCallback;

        public DependencyMap(Cell owner, Action<Cell> subscribe = null, Action<Cell> errorCallback = null)
        {
            Dependencies = new OrderedDictionary();
            Owner = owner;
            subscribeCallback = subscribe;
            ErrorCallback = errorCallback;
        }

        ~DependencyMap()
        {
            Unsubscribe();
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
        public Action<Cell> SubscribeCallback 
        {
            get
            {
                return subscribeCallback;
            }
            set
            {
                Subscribe(value);
            }
        }

        /// <summary>
        /// Gets or sets the callback for what happens when an error occurs somewhere
        /// </summary>
        public Action<Cell> ErrorCallback { get; set; }

        //Our methods
        public void Unsubscribe()
        {
            if (subscribeCallback == null)
                return;

            foreach(Dependency d in Dependencies)
            {
                if (d.IsDirect)
                    d.Cell.Changed -= subscribeCallback;
            }

            subscribeCallback = null;
        }

        /// <summary>
        /// Subscribes a given method to any changes in dependencies.
        /// </summary>
        /// <param name="subscribe">A method that takes in a Cell as an argument.</param>
        /// <returns></returns>
        private bool Subscribe(Action<Cell> subscribe)
        {
            if (subscribeCallback == null)
                subscribeCallback = subscribe;
            else
            {
                //Unsubscribe to start fresh.
                Unsubscribe();

                //We have all dependencies, so if the owner is in it, there's a problem.
                if(Dependencies.Contains(Owner.Name))
                {
                    ErrorCallback(Owner);
                    return false;
                }

                subscribeCallback = subscribe;

                //Subscribe to all direct dependencies.
                foreach(Dependency d in Dependencies)
                {
                    if (!d.IsDirect)
                        break;

                    d.Cell.Changed += subscribeCallback;
                }
            }

            return true;
        }
    }
}
