using System.Collections.Generic;

namespace SudokuSolver.Model
{
    /// <summary>
    /// Class holding sudoku board and solving it.
    /// </summary>
    public class SudokuSolver
    {
        /// <summary>
        /// Sudoku board.
        /// </summary>
        private Sudoku _Sudoku;

        /// <summary>
        /// List of patterns for solving sudoku board.
        /// </summary>
        private List<Pattern> _Patterns = new List<Pattern>();

        /// <summary>
        /// Initiate a SudokuSolver with sudoku.
        /// </summary>
        /// <param name="sudoku">Initiated sudoku object.</param>
        public SudokuSolver(Sudoku sudoku = null)
        {
            _Sudoku = sudoku;
            _Patterns.AddRange(Pattern.GetPatternsWithoutBruteForce());
        }

        /// <summary>
        /// Solve sudoku using patterns. If typical patterns do not solve sudoku, use Brute Force.
        /// </summary>
        /// <returns>True if sudoku is solved. Otherwise sudoku is wrong.</returns>
        public bool Solve()
        {
            var restart = true;
            while (restart)
            {
                restart = false;
                foreach (var pattern in _Patterns)
                {
                    var solve_again = pattern.Solve(_Sudoku, false);
                    if (_Sudoku.IsSolved())
                    {
                        restart = false;
                        break;
                    }
                    else if (solve_again)
                        restart = true;
                }
            }
            // If not solved by means of typical patterns - use Brute Force.
            if (!_Sudoku.IsSolved())
            {
                var pattern = new BruteForce();
                pattern.Solve(_Sudoku, false);
            }
            return _Sudoku.IsSolved();
        }

        /// <summary>
        /// Print Sudoku for diagnostic purpose.
        /// </summary>
        /// <returns>Diagnostic string.</returns>
        public override string ToString()
        {
            return _Sudoku.ToString();
        }
    }
}
