using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Helper;
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

        [TestMethod()]
        public void CopyTest()
        {
            // arrange
            var puzzle = @"100008400
                            020004900
                            903256000
                            600000571
                            410805062
                            532000004
                            000582709
                            001300040
                            008100005
                            ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);

            // act
            var sudokuCopy = new Sudoku(sudoku);
            sudoku.SetCellValue(0, 1, 7, "manual");

            // assert
            Assert.AreEqual(7, sudoku.GetCellValue(0, 1));
            Assert.AreEqual(0, sudokuCopy.GetCellValue(0, 1));
        }

        [TestMethod()]
        public void CopyTest2()
        {
            // arrange
            var puzzle = @"100008400
                            020004900
                            903256000
                            600000571
                            410805062
                            532000004
                            000582709
                            001300040
                            008100005
                            ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);
            var solver = new SudokuSolver(sudoku);

            // act
            solver.Solve();
            var sudokuCopy = new Sudoku(sudoku);
            sudokuCopy.SetCellValue(0, 1, 0, "manual");
            sudokuCopy.UpdatePossibleValuesInAllRegions();

            // assert
            Assert.IsTrue(sudoku.IsSolved());
            Assert.IsFalse(sudokuCopy.IsSolved());
        }

        [TestMethod()]
        public void SerializationCopyTest()
        {
            // arrange
            var puzzle = @"100008400
                            020004900
                            903256000
                            600000571
                            410805062
                            532000004
                            000582709
                            001300040
                            008100005
                            ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);

            // act
            var sudokuCopy = sudoku.DeepCopy();
            sudoku.SetCellValue(0, 1, 7, "manual");

            // assert
            Assert.AreEqual(7, sudoku.GetCellValue(0, 1));
            Assert.AreEqual(0, sudokuCopy.GetCellValue(0, 1));
        }

        [TestMethod()]
        public void SerializationCopyTest2()
        {
            // arrange
            var puzzle = @"100008400
                            020004900
                            903256000
                            600000571
                            410805062
                            532000004
                            000582709
                            001300040
                            008100005
                            ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);
            var solver = new SudokuSolver(sudoku);

            // act
            solver.Solve();
            var sudokuCopy = sudoku.DeepCopy();
            sudokuCopy.SetCellValue(0, 1, 0, "manual");
            sudokuCopy.UpdatePossibleValuesInAllRegions();

            // assert
            Assert.IsTrue(sudoku.IsSolved());
            Assert.IsFalse(sudokuCopy.IsSolved());
        }
    }
}
