using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Model
{
    /// <summary>
    /// A class solving sudoku by One Possibility pattern. Extends Pattern class.    
    /// 
    /// The pattern looks for cells with only one possible value and writes it to the cell.
    /// </summary>
    public class OnePossibility : Pattern
    {
        /// <summary>
        /// Priority for a pattern. The pattern with the lowest priority value is the first to be used for solving.
        /// </summary>
        protected override int Priority => 100;

        /// <summary>
        /// Solve sudoku with OnePossibility pattern.
        /// 
        /// The pattern goes through all the cells in sudoku and checks if there is only one possible value to write. If it
        /// is True then function writes that value to a cell.It checks following cells until the end unless argument
        /// solve_one is True.In this situation it finishes operating on sudoku after first write.
        /// </summary>
        /// <param name="sudoku">Sudoku object to solve.</param>
        /// <param name="solveOne">Parameter to enable finishing function after writing one value to a cell.</param>
        /// <returns>True if function wrote value to at least one cell.</returns>
        public override bool Solve(Sudoku sudoku, bool solveOne = false)
        {
            var size = sudoku.Size;
            var wasChanged = false;
            for (byte row = 0; row < size; row++)
            {
                for (byte column = 0; column < size; column++)
                {
                    var possibilities = sudoku.GetCellPossibilities(row, column);
                    if (possibilities.Count == 1)
                    {
                        sudoku.SetCellValue(row, column, possibilities.First(), Name);
                        wasChanged = true;
                        if (solveOne)
                        {
                            return true;
                        }
                    }
                }
            }
            return wasChanged;
        }
    }

}
