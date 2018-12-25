using System;
using System.Collections.Generic;

namespace SudokuSolver.Model
{
    /// <summary>
    /// Basic class to store data of one cell.
    /// </summary>
    [Serializable]
    public class Cell
    {
        /// <summary>
        /// Value of the cell between 0 (empty) and maximum value.
        /// </summary>
        private byte _Value;

        /// <summary>
        /// Initiate a cell. Write arguments to instance attributes and create a set of possible values. Populate the set
        /// with values from range [1, max_value] if the cell is editable.
        /// </summary>
        /// <param name="row">Row parameter of a cell.</param>
        /// <param name="column">Column parameter of a cell.</param>
        /// <param name="maxValue">Maximum value that cell can get (sudoku size).</param>
        /// <param name="editable">Defines if object's value can be edited or is permanent. Defaults to True.</param>
        /// <param name="value">Value of the cell between 0 (empty) and MAX_VALUE. Defaults to 0.</param>
        /// <exception cref="ArgumentException">Throws if data given is incorrect: 
        /// Cell is not editable and value is equal to 0;
        /// Value is below 0 or above maxValue;
        /// Row or column is below 1 or above maxValue.
        /// </exception>
        public Cell(byte row, byte column, byte maxValue, bool editable = true, byte value = 0)
        {
            if (!editable && value == 0)
            {
                throw new ArgumentException("Cell is not editable and no value was given");
            }
            else if (value < 0 || value > maxValue)
            {
                throw new ArgumentException($"Inorrect value ({value} is not in <0,{maxValue}>");
            }
            else if (!((0 <= row) && (row <= maxValue))
                    || !((0 <= column) && (column <= maxValue)))
            {
                throw new ArgumentException($"Incorrect row or column ({row},{column} not in <0,{maxValue}>");
            }

            Editable = editable;
            Row = row;
            Column = column;
            _Value = value;

            if (editable)
            {
                for (byte i = 1; i <= maxValue; i++)
                {
                    PossibleValues.Add(i);
                }
            }
        }

        /// <summary>
        /// Row parameter of a cell.
        /// </summary>
        public byte Row { get; }

        /// <summary>
        /// Column parameter of a cell.
        /// </summary>
        public byte Column { get; }

        /// <summary>
        /// Defines if object's value can be edited or is permanent.
        /// </summary>
        public bool Editable { get; }

        /// <summary>
        /// A set of values that are possible to write to the cell.
        /// </summary>
        public ISet<byte> PossibleValues { get; } = new HashSet<byte>();

        /// <summary>
        /// Value of the cell between 0 (empty) and maximum value. Write value if a cell is editable. Clear a set of possible values if value is not 0.
        /// </summary>
        /// <exception cref="ArgumentException">Throw if Editable attribute is False</exception>
        public byte Value
        {
            get { return _Value; }
            set
            {
                if (Editable == false)
                {
                    throw new ArgumentException("Cell is not editable.", nameof(Editable));
                }
                else if (value == 0)
                {
                    _Value = 0;
                }
                else
                {
                    _Value = value;
                    PossibleValues.Clear();
                }
            }
        }

        /// <summary>
        /// Fill the set of possible values with full range [1, max_value].
        /// </summary>
        /// <param name="maxValue">Maximum possible value that can be written to the cell.</param>
        public void InitPossibleValues(byte maxValue)
        {
            for (byte i = 1; i <= maxValue; i++)
            {
                PossibleValues.Add(i);
            }
        }

        /// <summary>
        /// Intersect cell's possible values with given set (i.e. set of possible values from region).
        /// </summary>
        /// <param name="set">Set to intersect with instance's possible values.</param>
        public void IntersectPossibleValues(IEnumerable<byte> set)
        {
            PossibleValues.IntersectWith(set);
        }

        /// <summary>
        /// Clear the cell's value if the cell is editable.
        /// </summary>
        public void Clear()
        {
            if (Editable)
            {
                Value = 0;
            }
        }

        /// <summary>
        /// Remove possible value from a set (if present).
        /// </summary>
        /// <param name="value">Value removed from set of possible values.</param>
        public void RemovePossibleValue(byte value)
        {
            PossibleValues.Remove(value);
        }

        /// <summary>
        /// Print cell's attributes for diagnostic purpose.
        /// </summary>
        /// <returns>Diagnostic string.</returns>
        public override string ToString()
        {
            return $"[{Row},{Column}]: editable: {Editable}: {Value} / {PossibleValues}";
        }
    }
}
