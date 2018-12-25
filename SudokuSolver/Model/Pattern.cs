using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model
{
    public abstract class Pattern
    {
        /// <summary>
        /// Return a name of the pattern.
        /// </summary>
        public string Name => GetType().Name;

        /// <summary>
        /// Priority for a pattern. The pattern with the lowest priority value is the first to be used for solving.
        /// </summary>
        protected abstract int Priority { get; }

        /// <summary>
        /// Solve sudoku with a implemented pattern and return True if any Cell was changed. If not, return false.
        /// </summary>
        /// <param name="sudoku">Sudoku object to which apply solution algorithm.</param>
        /// <param name="solveOne">If True, finish solving after modifying one Cell. Defaults to False.</param>
        /// <returns>Value determining if any Cell was modified. False means that algorithm is no longer able to find 
        /// solution to any Cell.</returns>
        public abstract bool Solve(Sudoku sudoku, bool solveOne = false);

        /// <summary>
        /// Get a list off all patterns except Brute Force.
        /// </summary>
        /// <returns>A list of Pattern objects.</returns>
        public static IEnumerable<Pattern> GetPatternsWithoutBruteForce()
        {
            var list = new List<Pattern>();
            foreach (var type in typeof(Pattern).Assembly.GetTypes())
            {
                if(typeof(Pattern).IsAssignableFrom(type) // derives from Pattern
                    && type != typeof(Pattern)            // but it is neither Pattern
                    && type != typeof(BruteForce))        // nor BruteForce
                {
                    list.Add((Pattern)Activator.CreateInstance(type));
                }
            }
            list.Sort((x, y) => x.Priority.CompareTo(y.Priority));

            return list;
        }

        /// <summary>
        /// Get a list off all patterns (including Brute Force).
        /// </summary>
        /// <returns>A list of Pattern objects.</returns>
        public static IEnumerable<Pattern> GetPatternsWithBruteForce()
        {
            var list = new List<Pattern>(GetPatternsWithoutBruteForce());
            list.Add((Pattern)Activator.CreateInstance(typeof(BruteForce)));

            return list;
        }
    }
}
