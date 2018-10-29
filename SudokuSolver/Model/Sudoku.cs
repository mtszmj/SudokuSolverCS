using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.Model
{
    public class Sudoku
    {
        private UndoRedo _UndoRedo;
        private byte _RectangleWidth;
        private byte _RectangleHeight;

        public Sudoku(byte size = 9, Cell[,] cells = null, byte rectangleWidth = 3, byte rectangleHeight = 3)
        {
            if (cells == null)
            {
                Cells = new Cell[size, size];
                Size = size;
            }
            else if (cells.Rank == 2 && cells.GetLength(1) == cells.GetLength(2))
            {
                Cells = cells;
                Size = (byte)cells.GetUpperBound(1);
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
            Regions = new Region[rowsCount + columnsCount + rectanglesCount];  // Generate regions for rows, columns and rectangles.

            for (var row = 0; row < Size; row++)
            {
                for (var column = 0; column < Size; column++)
                {
                    Regions[row].Add(Cells[row, column]);  // Populate row regions.
                    Regions[rowsCount + column].Add(Cells[row, column]);  // Populate column regions.
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

            UpdatePossibleValuesInAllRegions();
        }

        public byte Size { get; }
        public Cell[,] Cells { get; }
        public Region[] Regions { get; }

        private bool IsRowAndColumnInRange(byte row, byte column)
        {
            if (0 <= row && row < Size && 0 <= column && column < Size)
            {
                return true;
            }
            else throw new ArgumentException($"{nameof(row)} or {nameof(column)} out of range [0,{Size - 1}] - ({row},{column})");
        }

        public byte GetCellValue(byte row, byte column)
        {
            return Cells[row, column].Value;
        }

        public void SetCellValue(byte row, byte column, byte value, string method = "")
        {
            if (!IsRowAndColumnInRange(row, column))
            { }
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

        private void RemovePossibleValue(Cell cell, byte value)
        {
            foreach (var region in Regions)
            {
                region.RemovePossibleValueIfCellIsInRegion(cell, value);
            }
        }

        public ISet<byte> GetCellPossibilities(byte row, byte column)
        {
            return Cells[row, column].PossibleValues;
        }

        public bool IsEditable(byte row, byte column)
        {
            return Cells[row, column].Editable;
        }

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

        public bool IsWrong()
        {
            foreach (var region in Regions)
            {
                if (!region.IsNotPossibleToSolve())
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (var row = 0; row < Size; row++)
            {
                int counter = 1;
                for (var column = 0; row < Size; row++)
                {
                    if (Size < 10)
                    {
                        builder.Append($"{Cells[row, column].Value}");
                    }
                    else
                    {
                        builder.Append($"{Cells[row, column].Value}, -2");
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

        public void CopyFrom(Sudoku sudoku)
        {
            for (var i = _UndoRedo.UndoLength(); i < sudoku._UndoRedo.UndoLength(); i++)
            {
                var (row, column, oldValue, value, method) = sudoku._UndoRedo._Undo[i];
                SetCellValue(row, column, value, method);
            }
        }
    }
}

