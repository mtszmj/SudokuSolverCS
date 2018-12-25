using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Helper
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// Get Value for given key if exists. Else return default.
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type.</typeparam>
        /// <typeparam name="TValue">Dictionary value type.</typeparam>
        /// <param name="dict">Dictionary to take value from.</param>
        /// <param name="key">Key to select value from dictionary.</param>
        /// <param name="defaultValue">Default value if key does not exist. If not specified it is 'default(TValue)'.</param>
        /// <returns>Value for given key or given default value.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return dict.TryGetValue(key, out TValue value) ? value : defaultValue;
        }
    }
}
