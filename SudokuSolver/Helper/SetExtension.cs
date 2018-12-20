using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Helper
{
    public static class SetExtension
    {
        /// <summary>
        /// Get first element from a set. First element might differ, but function is foreseen 
        /// to select element from a set containing only one value.
        /// </summary>
        public static T First<T>(this ISet<T> set)
        {
            foreach (var element in set)
                return element;

            return default(T);
        }

        /// <summary>
        /// Convert ISet as IEnumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this ISet<T> set)
        {
            return set.Select(element => element);
        }
    }
}
