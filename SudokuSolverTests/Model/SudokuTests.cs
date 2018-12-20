using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace SudokuSolver.Model.Tests
{
    [TestClass()]
    public class SudokuTests
    {
        [TestMethod()]
        public void EmptySudokuTest()
        {
            var sudoku = new Sudoku();
            foreach (var cell in sudoku.Cells)
            {
                Assert.IsTrue(cell.Editable);
                Assert.AreEqual(0, cell.Value);
            }
        }

        [TestMethod()]
        public void CreateSudokuFromCellsTest()
        {
            byte size = 9;
            var cells = new Cell[size, size];
            for (byte row = 0; row < size; row++)
            {
                for (byte column = 0; column < size; column++)
                {
                    if (row == column)
                        cells[row, column] = new Cell(row, column, size, false, (byte)(row + 1));
                    else
                        cells[row, column] = new Cell(row, column, size);
                }
            }
            var sudoku = new Sudoku(cells);

            for (byte row = 0; row < size; row++)
            {
                for (byte column = 0; column < size; column++)
                {
                    if (row == column)
                    {
                        Assert.IsFalse(sudoku.Cells[row, column].Editable);
                        Assert.AreEqual(row + 1, sudoku.Cells[row, column].Value);
                    }
                    else
                    {
                        Assert.IsTrue(sudoku.Cells[row, column].Editable);
                        Assert.AreEqual(0, sudoku.Cells[row, column].Value);
                    }
                }
            }
        }

        [TestMethod()]
        public void GetCellsValueTest()
        {
            byte size = 9;
            var cells = new Cell[size, size];
            for (byte row = 0; row < size; row++)
            {
                for (byte column = 0; column < size; column++)
                {
                    if (row == column)
                        cells[row, column] = new Cell(row, column, size, false, (byte)(row + 1));
                    else
                        cells[row, column] = new Cell(row, column, size);
                }
            }
            var sudoku = new Sudoku(cells);

            Assert.AreEqual(4, sudoku.GetCellValue(3, 3));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod()]
        public void GetCellsValueIndexOutOfRangeTest()
        {
            var sudoku = new Sudoku();
            Assert.AreEqual(4, sudoku.GetCellValue(0, 99));
        }

        [TestMethod()]
        public void EditableTest()
        {
            byte size = 9;
            var cells = new Cell[size, size];
            for (byte row = 0; row < size; row++)
            {
                for (byte column = 0; column < size; column++)
                {
                    if (row == column)
                        cells[row, column] = new Cell(row, column, size, false, (byte)(row + 1));
                    else
                        cells[row, column] = new Cell(row, column, size);
                }
            }
            var sudoku = new Sudoku(cells);

            for (byte row = 0; row < size; row++)
            {
                for (byte column = 0; column < size; column++)
                {
                    if (row == column)
                    {
                        Assert.IsFalse(sudoku.Cells[row, column].Editable);
                    }
                    else
                    {
                        Assert.IsTrue(sudoku.Cells[row, column].Editable);
                    }
                }
            }
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod()]
        public void SetCellsValueTest()
        {
            var sudoku = new Sudoku();
            byte size = sudoku.Size;
            sudoku.SetCellValue(0, 0, (byte)(size + 1));
        }
    }
}
