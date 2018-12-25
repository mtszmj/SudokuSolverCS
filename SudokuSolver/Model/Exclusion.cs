using SudokuSolver.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model
{
    /// <summary>
    /// A class solving sudoku by Exclusion pattern. Extends Pattern class.
    ///
    /// The pattern checks cells in Region of sudoku and excludes possible values that exists in multiple cells.If there is
    /// remaining possible value it is written to the cell.
    /// </summary>
    public class Exclusion : Pattern
    {
        /// <summary>
        /// Priority for a pattern. The pattern with the lowest priority value is the first to be used for solving.
        /// </summary>
        protected override int Priority => 200;

        /// <summary>
        /// Solve sudoku with Exclusion pattern.
        /// 
        /// The pattern checks cells in Region of sudoku.If there is a cell that has multiple possibilities but one of them
        /// does not exist in other cells then the function writes the possible value to the cell.
        /// 
        /// Algorithm:
        /// Go through each region of the sudoku.Then go though each cell of the region. Check possible values of each
        /// cell. Update dictionary where a key is possible value and item is a tuple consisting of:
        /// - number of times each possible value exists in cells of the region,
        /// - a list of cells that have the possible value.
        /// 
        /// After the region is checked and possible values are counted, go though the dictionary and check if there is
        /// the possible value that exists only in one cell. If it is present, write the value to the cell.
        /// </summary>
        /// <param name="sudoku">Sudoku object to solve.</param>
        /// <param name="solveOne">Parameter to enable finishing function after writing one value to a cell.</param>
        /// <returns>True if functions wrote value to at least one cell.</returns>
        public override bool Solve(Sudoku sudoku, bool solveOne = false)
        {
            var countPossibilitiesDict = new Dictionary<byte, (int, ICollection<Cell>)>();
            var wasChanged = false;

            foreach (var region in sudoku.Regions)
            {
                countPossibilitiesDict.Clear();
                foreach(var cell in region.Cells)
                {
                    foreach(var possibleValue in cell.PossibleValues)
                    {
                        (int count, ICollection<Cell> cellsList) = countPossibilitiesDict.GetValueOrDefault(possibleValue, (0, new List<Cell>()));
                        cellsList.Add(cell);
                        countPossibilitiesDict[possibleValue] = (++count, cellsList);
                    }
                }

                foreach(var keyValue in countPossibilitiesDict)
                {
                    var value = keyValue.Key;
                    var (count, cellsList) = keyValue.Value;
                    if(count == 1)
                    {
                        sudoku.SetCellValue(cellsList.First().Row, cellsList.First().Column, value, Name);
                        wasChanged = true;
                        if (solveOne)
                            break;
                    }
                }
            }
            return wasChanged;
        }
    }
}
