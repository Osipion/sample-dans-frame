using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DansGameCore
{

     

    public class Counters : IEnumerable<KeyValuePair<string, double>>
    {
        /// <summary>
        /// The method signature for the try add or update evaluator method
        /// </summary>
        /// <param name="KeyAlreadyExists">A flag indicating whether the key already exists</param>
        /// <param name="current_value">If KeyAlreadyExists is set to true, this is the current value of the key. Otherwise, it is 0</param>
        /// <returns>The updated value of the counter</returns>
        public delegate double AddOrUpdateValueEvaluatorDelegate(bool KeyAlreadyExists, double current_value);

        private Dictionary<string, double> counters = new Dictionary<string, double>();

        /// <summary>
        /// Adds the specifed counter to the counters object. If the counter name already exists, an exception is thrown
        /// </summary>
        /// <param name="counter_name">The unique name of the counter</param>
        /// <param name="initial_value">The initial value to set it to</param>
        public void Add(string counter_name, double initial_value)
        {
            this.counters.Add(counter_name, initial_value);
        }

        /// <summary>
        /// Increments the specified counter. If the counter is not found, an exception is thrown
        /// </summary>
        /// <param name="counter_name">The name of the counter to increment</param>
        /// <param name="value">The value to increment it by (this is added on to the existing value)</param>
        public void Increment(string counter_name, double value)
        {
            try
            {
                this.counters[counter_name] += value;
            }
            catch(Exception e)
            {
                throw new ArgumentException("The specified counter '" + counter_name + "' did not exist in the counter set.", e);
            }
        }

        /// <summary>
        /// If the specified counter exists, it's value is set to the value returned by value_evaluator. Otherwise, it is created first and then set to that value
        /// </summary>
        /// <param name="counter">The name of the counter</param>
        /// <param name="value_evaluator">A function that will generate the value based on whether it was found, and it's current value</param>
        public void AddOrUpdate(string counter, AddOrUpdateValueEvaluatorDelegate value_evaluator)
        {
            if (this.counters.ContainsKey(counter))
                this.counters[counter] = value_evaluator(true, this.counters[counter]);
            else
                this.counters.Add(counter, value_evaluator(false, 0D));
        }

        /// <summary>
        /// Increments the given counter if it exists
        /// </summary>
        /// <param name="counter">The counter to increment</param>
        /// <param name="value_to_add_on">The value to add on</param>
        /// <returns>True if the counter was found, false otherwise</returns>
        public bool IncrementIfExists(string counter, double value_to_add_on)
        {
            if(this.counters.ContainsKey(counter))
            {
                this.counters[counter] += value_to_add_on;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the counter with the specified name
        /// </summary>
        /// <param name="counter_name"></param>
        /// <returns></returns>
        public double this[string counter_name]
        {
            get
            {
                return this.counters[counter_name];
            }
            set
            {
                this.counters[counter_name] = value;
            }
        }

        /// <summary>
        /// Removes the counter with the given name, if it exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The counter that was removed, or a default value if it was not found</returns>
        public KeyValuePair<string, double> Remove(string key)
        {
            var ret = this.counters.FirstOrDefault(x => x.Key == key);

            if (!ret.Equals(default(KeyValuePair<string, double>)))
                this.counters.Remove(key);

            return ret;
        }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            return this.counters.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
