using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model
{
    /// <summary>
    /// Class holding a list of cells that have to have unique values. It might be:
    /// - row,
    /// - column,
    /// - rectangle.
    /// </summary>
    [Serializable]
    public class Region
    {
        /// <summary>
        /// A list of objects of type Cell.
        /// </summary>
        public ICollection<Cell> Cells { get; } = new List<Cell>();

        /// <summary>
        /// Add a cell to a list of cells if the list does not contain it.
        /// </summary>
        /// <param name="cell">Cell to be added to the list.</param>
        public void Add(Cell cell)
        {
            if (!Cells.Contains(cell))
            {
                Cells.Add(cell);
            }
        }

        /// <summary>
        /// Remove value from cell's possible values if the cell is in region.
        /// </summary>
        /// <param name="cell">Cell instance to check if it is in a region and to remove possible value from.</param>
        /// <param name="value">Possible value.</param>
        public void RemovePossibleValueIfCellIsInRegion(Cell cell, byte value)
        {
            if(Cells.Contains(cell))
            {
                foreach(var c in Cells)
                {
                    c.RemovePossibleValue(value);
                }
            }
        }

        /// <summary>
        /// Update possible values in all cells that the region contains.
        /// </summary>
        /// <remarks>
        /// <para>Create a set of possible values in a range [1, number of cells in a region]. Go through all cells and read their
        /// values.Remove read values from prepared set.Go through each cell and intersect cell's possible values with
        /// the set.</para>
        /// <para>Each cell belongs to 3 regions (row, column, rectangle), so its possible values should be intersected 3 times
        /// (by calling 'update_possible_values' method for all regions.</para>
        /// </remarks>
        public void UpdatePossibleValues()
        {
            var values = new HashSet<byte>();
            for (byte v = 1; v <= this.Cells.Count; v++)
            {
                values.Add(v);
            }

            foreach(var cell in Cells)
            {
                byte v = cell.Value;
                values.Remove(v);
            }

            foreach(var cell in Cells)
            {
                if(cell.Value == 0)
                {
                    cell.IntersectPossibleValues(values);
                }
            }
        }

        /// <summary>
        /// Check if the region is solved.
        /// </summary>
        /// <remarks>The region is solved if all cells have non-zero value, and all possible values from range
        /// [1, number of cells in a region] are used.</remarks>
        /// <returns>True if the region is solved. False otherwise.</returns>
        public bool IsSolved()
        {
            var values = new HashSet<byte>();
            foreach(var cell in Cells)
            {
                values.Add(cell.Value);
            }

            var expectedValues = new HashSet<byte>();
            for (byte v = 1; v <= Cells.Count; v++)
            {
                expectedValues.Add(v);
            }
            return values.SetEquals(expectedValues);
        }

        /// <summary>
        /// Check if a region is not possible to solve in a current configuration.
        /// </summary>
        /// <returns>True if there is a cell with no value (zero) and it has zero possible values or if there are at least
        /// two cells with the same value.</returns>
        public bool IsNotPossibleToSolve()
        {
            var set = new HashSet<byte>();
            foreach(var cell in Cells)
            {
                if(cell.Value == 0 && cell.PossibleValues.Count == 0)
                {
                    return true;
                }
                else if (set.Contains(cell.Value))
                {
                    return true;
                }
                else if (cell.Value != 0)
                {
                    set.Add(cell.Value);
                }
            }
            return false;
        }
    }
}
