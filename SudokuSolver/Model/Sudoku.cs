using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.Model
{
    /// <summary>
    /// Class that contains Sudoku board - two dimensional array of cells.
    /// </summary>
    [Serializable]
    public class Sudoku
    {
        /// <summary>
        /// Container storing actions done on Sudoku.
        /// </summary>
        private UndoRedo _UndoRedo;

        /// <summary>
        /// Width of each rectangle region (3 for Sudoku 9x9).
        /// </summary>
        private byte _RectangleWidth;

        /// <summary>
        /// Height of each rectangle region (3 for Sudoku 9x9).
        /// </summary>
        private byte _RectangleHeight;

        /// <summary>
        /// Initialize Sudoku object.
        /// </summary>
        /// <param name="cells">List of cells in Sudoku.</param>
        /// <param name="size">Size of the Sudoku.</param>
        /// <param name="rectangleWidth">Width of each rectangle region (3 for Sudoku 9x9).</param>
        /// <param name="rectangleHeight">Height of each rectangle region (3 for Sudoku 9x9).</param>
        public Sudoku(Cell[,] cells = null, byte size = 9, byte rectangleWidth = 3, byte rectangleHeight = 3)
        {
            if (cells == null)
            {
                Cells = new Cell[size, size];
                Size = size;
                for (byte r = 0; r < size; r++)
                    for (byte c = 0; c < size; c++)
                        Cells[r, c] = new Cell(r, c, size);
            }
            else if (cells.Rank == 2 && cells.GetLength(0) == cells.GetLength(1))
            {
                Cells = cells;
                Size = (byte)(cells.GetUpperBound(0) + 1);
            }
            else
            {
                throw new ArgumentException("Incorrect data input", nameof(cells));
            }

            _UndoRedo = new UndoRedo();
            _RectangleWidth = rectangleWidth;
            _RectangleHeight = rectangleHeight;

            int rowsCount, columnsCount, rectanglesCount;
            rowsCount = columnsCount = Size;
            rectanglesCount = (Size * Size) / (_RectangleWidth * _RectangleHeight);  // Number of rectangles = Board size / Rectangle size.
            var regionCount = rowsCount + columnsCount + rectanglesCount;
            Regions = new Region[regionCount];  // Generate regions for rows, columns and rectangles.
            for (var i = 0; i < regionCount; i++) { Regions[i] = new Region(); }

            PopulateRegions();
            UpdatePossibleValuesInAllRegions();
        }

        /// <summary>
        /// Size of the Sudoku.
        /// </summary>
        public byte Size { get; }

        /// <summary>
        /// List of cells in Sudoku.
        /// </summary>
        public Cell[,] Cells { get; }

        /// <summary>
        /// List of regions (rows, columns, rectangles).
        /// </summary>
        public Region[] Regions { get; }

        /// <summary>
        /// Populate regions (rows, columns, rectangles) with cells.
        /// </summary>
        private void PopulateRegions()
        {
            for (var row = 0; row < Size; row++)
            {
                for (var column = 0; column < Size; column++)
                {
                    Regions[row].Add(Cells[row, column]);  // Populate row regions.
                    Regions[Size + column].Add(Cells[row, column]);  // Populate column regions.
                }
            }

            // Populate rectangle regions.
            int widthSize = Size / _RectangleWidth;
            int heightSize = Size / _RectangleHeight;
            int reg = Size * 2 - 1;
            for (var xStart = 0; xStart < heightSize; xStart++)
            {
                for (var yStart = 0; yStart < widthSize; yStart++)
                {
                    reg++;
                    for (var x = xStart * widthSize; x < (xStart + 1) * widthSize; x++)
                    {
                        for (var y = yStart * heightSize; y < (yStart + 1) * heightSize; y++)
                        {
                            Regions[reg].Add(Cells[x, y]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if row and integer are in range [0, size-1].
        /// </summary>
        /// <param name="row">A row number ([0, size-1]).</param>
        /// <param name="column">A column number ([0, size-1]).</param>
        /// <returns>True if a row and column are in the correct range.</returns>
        /// <exception cref="ArgumentException">Throw if row or column is outside [0, size-1] range.</exception>
        private bool IsRowAndColumnInRange(byte row, byte column)
        {
            if (0 <= row && row < Size && 0 <= column && column < Size)
            {
                return true;
            }
            else throw new ArgumentException($"{nameof(row)} or {nameof(column)} out of range [0,{Size - 1}] - ({row},{column})");
        }

        /// <summary>
        /// Return a value of a cell.
        /// </summary>
        /// <param name="row">A row number.</param>
        /// <param name="column">A column number.</param>
        /// <returns>Value of a cell placed in [row, column].</returns>
        public byte GetCellValue(byte row, byte column)
        {
            IsRowAndColumnInRange(row, column);
            return Cells[row, column].Value;
        }

        /// <summary>
        /// Set a value of a cell. Remove the value from possible values of ranges that the cell belongs to (row, column, 
        /// rectangle). Add action to _UndoRedo.
        /// </summary>
        /// <param name="row">A row number ([0, size-1]).</param>
        /// <param name="column">A column number ([0, size-1]).</param>
        /// <param name="value">Value to write to a cell.</param>
        /// <param name="method">Optional method identifier. Defaults to ''.</param>
        /// <exception cref="ArgumentException">Throw if:
        /// - editable attribute is False 
        /// - value is not in a range [0, MAX_VALUE]</exception>
        public void SetCellValue(byte row, byte column, byte value, string method = "")
        {
            if (!IsRowAndColumnInRange(row, column))
            {
                var msg = $"Incorrect row or column: {row},{column}, max: {Size - 1}";
                throw new ArgumentException(msg);
            }
            else if (value < 0 || value > Size)
            {
                string msg = $"Incorrect value: {value}, max: {Size}.";
                throw new ArgumentException(msg, nameof(value));
            }
            else
            {
                Cell cell = Cells[row, column];
                byte oldValue = cell.Value;
                cell.Value = value;
                RemovePossibleValue(cell, value);
                _UndoRedo.AddAction(row, column, oldValue, value, method);
            }
        }

        /// <summary>
        /// Remove value from cells' possible values in the same region as given cell (row, column, rectangle).
        /// </summary>
        /// <param name="cell">Cell object.</param>
        /// <param name="value">Value to remove from possible values.</param>
        private void RemovePossibleValue(Cell cell, byte value)
        {
            foreach (var region in Regions)
            {
                region.RemovePossibleValueIfCellIsInRegion(cell, value);
            }
        }

        /// <summary>
        /// Return a set of possible values that can be written to the cell from given row and column.
        /// </summary>
        /// <param name="row">A row number ([0, size-1]).</param>
        /// <param name="column">A column number ([0, size-1]).</param>
        /// <returns>Possible values that can be written to the cell.</returns>
        public ISet<byte> GetCellPossibilities(byte row, byte column)
        {
            return Cells[row, column].PossibleValues;
        }

        /// <summary>
        /// Check if a cell from given row and column is editable.
        /// </summary>
        /// <param name="row">A row number ([0, size-1]).</param>
        /// <param name="column">A column number ([0, size-1]).</param>
        /// <returns>True if the cell is editable.</returns>
        public bool IsEditable(byte row, byte column)
        {
            return Cells[row, column].Editable;
        }

        /// <summary>
        /// Update (refresh) possible values in all regions (meaning all cells).
        /// 
        /// Use the function after modifying cell's value, clearing its value or undoing action.
        /// </summary>
        public void UpdatePossibleValuesInAllRegions()
        {
            for (var column = 0; column < Size; column++)
            {
                for (var row = 0; row < Size; row++)
                {
                    if (Cells[row, column].Value == 0)
                    {
                        Cells[row, column].InitPossibleValues(Size);
                    }
                }
            }

            foreach (var region in Regions)
            {
                region.UpdatePossibleValues();
            }
        }

        /// <summary>
        /// Check if Sudoku is solved.
        /// </summary>
        /// <returns>True if Sudoku is solved correctly. False otherwise.</returns>
        public bool IsSolved()
        {
            foreach (var region in Regions)
            {
                if (!region.IsSolved())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if Sudoku is not possible to solve.
        /// 
        /// Sudoku is not possible to solve if there is a region (row, column, rectangle) containing a cell with no value
        /// (zero) and zero possible values to write or if there are at least two cells with the same value.
        /// </summary>
        /// <returns>True if there is a region with no solution.</returns>
        public bool IsWrong()
        {
            foreach (var region in Regions)
            {
                if (region.IsNotPossibleToSolve())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Copy data from given argument 'sudoku' to current instance by repeating actions of sudoku's undo list.
        /// </summary>
        /// <param name="sudoku">Sudoku instance to copy actions from.</param>
        public void CopyFrom(Sudoku sudoku)
        {
            for (var i = _UndoRedo.UndoLength(); i < sudoku._UndoRedo.UndoLength(); i++)
            {
                var (row, column, oldValue, value, method) = sudoku._UndoRedo._Undo[i];
                SetCellValue(row, column, value, method);
            }
        }

        /// <summary>
        /// Print Sudoku for diagnostic purpose.
        /// </summary>
        /// <returns>Diagnostic string.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (var row = 0; row < Size; row++)
            {
                int counter = 1;
                for (var column = 0; column < Size; column++)
                {
                    if (Size < 10)
                    {
                        builder.Append($"{Cells[row, column].Value}");
                    }
                    else
                    {
                        builder.Append($"{Cells[row, column].Value,2}");
                        if (counter < Size)
                        {
                            builder.Append(",");
                            counter++;
                        }
                    }
                }
                builder.Append(System.Environment.NewLine);
            }
            return builder.ToString();
        }
    }
}

