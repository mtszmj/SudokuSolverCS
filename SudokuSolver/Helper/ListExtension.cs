using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Helper
{
    public static class ListExtension
    {
        /// <summary>
        /// Get last element and remove it from a list.
        /// </summary>
        public static T Pop<T>(this List<T> list)
        {
            T element = list.Last();
            list.RemoveAt(list.Count - 1);
            return element;
        }
    }
}
