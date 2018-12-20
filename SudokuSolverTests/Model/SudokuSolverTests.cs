using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model.Tests
{
    [TestClass()]
    public class SudokuSolverTests
    {
        [TestMethod()]
        public void SolveTest()
        {
            var sudoku = new Sudoku();

            sudoku.SetCellValue(0, 0, 1);
            sudoku.SetCellValue(0, 5, 8);
            sudoku.SetCellValue(0, 6, 4);

            sudoku.SetCellValue(1, 1, 2);
            sudoku.SetCellValue(1, 5, 4);
            sudoku.SetCellValue(1, 6, 9);

            sudoku.SetCellValue(2, 0, 9);
            sudoku.SetCellValue(2, 2, 3);
            sudoku.SetCellValue(2, 3, 2);
            sudoku.SetCellValue(2, 4, 5);
            sudoku.SetCellValue(2, 5, 6);

            sudoku.SetCellValue(3, 0, 6);
            sudoku.SetCellValue(3, 6, 5);
            sudoku.SetCellValue(3, 7, 7);
            sudoku.SetCellValue(3, 8, 1);

            sudoku.SetCellValue(4, 0, 4);
            sudoku.SetCellValue(4, 1, 1);
            sudoku.SetCellValue(4, 3, 8);
            sudoku.SetCellValue(4, 5, 5);
            sudoku.SetCellValue(4, 7, 6);
            sudoku.SetCellValue(4, 8, 2);

            sudoku.SetCellValue(5, 0, 5);
            sudoku.SetCellValue(5, 1, 3);
            sudoku.SetCellValue(5, 2, 2);
            sudoku.SetCellValue(5, 8, 4);

            sudoku.SetCellValue(6, 3, 5);
            sudoku.SetCellValue(6, 4, 8);
            sudoku.SetCellValue(6, 5, 2);
            sudoku.SetCellValue(6, 6, 7);
            sudoku.SetCellValue(6, 8, 9);

            sudoku.SetCellValue(7, 2, 1);
            sudoku.SetCellValue(7, 3, 3);
            sudoku.SetCellValue(7, 7, 4);

            sudoku.SetCellValue(8, 2, 8);
            sudoku.SetCellValue(8, 3, 1);
            sudoku.SetCellValue(8, 8, 5);

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

            Assert.AreEqual(puzzle, sudoku.ToString());

            var solver = new SudokuSolver(sudoku);
            solver.Solve();

            var solution = @"175938426
                            826714953
                            943256187
                            689423571
                            417895362
                            532671894
                            364582719
                            751369248
                            298147635
                            ".Replace(" ", "");

            Assert.AreEqual(solution, sudoku.ToString());
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod()]
        public void Solve2Test()
        {
            var sudoku = new Sudoku();

            sudoku.SetCellValue(0, 0, 1);
            sudoku.SetCellValue(0, 5, 8);
            sudoku.SetCellValue(0, 6, 4);

            sudoku.SetCellValue(1, 1, 2);
            sudoku.SetCellValue(1, 5, 4);
            sudoku.SetCellValue(1, 6, 9);

            sudoku.SetCellValue(2, 0, 9);
            sudoku.SetCellValue(2, 2, 3);
            sudoku.SetCellValue(2, 3, 2);
            sudoku.SetCellValue(2, 4, 5);
            sudoku.SetCellValue(2, 5, 6);

            sudoku.SetCellValue(3, 0, 6);
            sudoku.SetCellValue(3, 6, 5);
            sudoku.SetCellValue(3, 7, 7);
            sudoku.SetCellValue(3, 8, 1);

            sudoku.SetCellValue(4, 0, 4);
            sudoku.SetCellValue(4, 1, 1);
            sudoku.SetCellValue(4, 3, 8);
            sudoku.SetCellValue(4, 5, 5);
            sudoku.SetCellValue(4, 7, 6);
            sudoku.SetCellValue(4, 8, 2);

            sudoku.SetCellValue(5, 0, 5);
            sudoku.SetCellValue(5, 1, 3);
            sudoku.SetCellValue(5, 2, 2);
            sudoku.SetCellValue(5, 8, 4);

            sudoku.SetCellValue(6, 3, 5);
            sudoku.SetCellValue(6, 4, 8);
            sudoku.SetCellValue(6, 5, 2);
            sudoku.SetCellValue(6, 6, 7);
            sudoku.SetCellValue(6, 8, 9);

            sudoku.SetCellValue(7, 2, 1);
            sudoku.SetCellValue(7, 3, 3);
            sudoku.SetCellValue(7, 7, 4);

            sudoku.SetCellValue(8, 2, 8);
            sudoku.SetCellValue(8, 3, 1);
            sudoku.SetCellValue(8, 8, 5);
            sudoku.SetCellValue(8, 8, 5);

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

            Assert.AreEqual(puzzle, sudoku.ToString());

            var solver = new SudokuSolver(sudoku);
            solver.Solve();

            var solution = @"175938426
                            826714953
                            943256187
                            689423571
                            417895362
                            532671894
                            364582719
                            751369248
                            298147635
                            ".Replace(" ", "");

            Assert.AreEqual(solution, sudoku.ToString());
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod()]
        public void Solve3Test()
        {
            var puzzle = @"293040100
                        516230740
                        847156000
                        354002690
                        600415000
                        000900000
                        000394802
                        000600005
                        000521000
                        ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);

            Assert.AreEqual(puzzle, sudoku.ToString());

            var solver = new SudokuSolver(sudoku);
            solver.Solve();

            var solution = @"293748156
                            516239748
                            847156239
                            354872691
                            629415387
                            781963524
                            165394872
                            432687915
                            978521463
                            ".Replace(" ", "");

            Assert.AreEqual(solution, sudoku.ToString());
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod()]
        public void Solve6x6Test()
        {
            var puzzle = @"100000
                        020000
                        003000
                        000400
                        000050
                        000000
                        ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);

            Assert.AreEqual(puzzle, sudoku.ToString());

            var solver = new SudokuSolver(sudoku);
            solver.Solve();

            var solution = @"134265
                            526134
                            243516
                            615423
                            461352
                            352641
                            ".Replace(" ", "");

            Assert.AreEqual(solution, sudoku.ToString());
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod()]
        public void Solve4x4Test()
        {
            var puzzle = @"1000
                        0200
                        0030
                        0004
                        ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);

            Assert.AreEqual(puzzle, sudoku.ToString());

            var solver = new SudokuSolver(sudoku);
            solver.Solve();

            var solution = @"1342
                            4213
                            2431
                            3124
                            ".Replace(" ", "");

            Assert.AreEqual(solution, sudoku.ToString());
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod()]
        public void Solve12x12Test()
        {
            var puzzle = @"5, 0, 0,10, 0,12, 0, 0, 1, 7, 0,11
                             0, 3, 2, 0, 0, 0, 0, 6, 0,10, 4, 0
                             1, 6, 0, 0, 0, 0, 0, 0, 0, 0,12, 0
                             3, 0, 0, 0,11, 0, 6, 0, 0, 0, 0, 8
                             0, 4, 0, 0,12, 1, 0, 7,11, 0, 0, 0
                             0, 0, 0, 6, 0,10, 4, 8, 0, 0, 0,12
                             2, 0, 0, 0,10, 4, 8, 0, 6, 0, 0, 0
                             0, 0, 0, 4, 6, 0, 7,11, 0, 0, 1, 0
                            11, 0, 0, 0, 0, 3, 0, 2, 0, 0, 0,10
                             0,10, 0, 0, 0, 0, 0, 0, 0, 0, 8, 4
                             0, 7, 8, 0, 9, 0, 0, 0, 0, 2,11, 0
                             6, 0, 9, 5, 0, 0,11, 0,10, 0, 0, 7
                        ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);

            Assert.AreEqual(puzzle, sudoku.ToString().Replace(" ", ""));

            var solver = new SudokuSolver(sudoku);
            solver.Solve();
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod()]
        public void SolveEmpty4x4Test()
        {
            var puzzle = @"0000
                        0000
                        0000
                        0000
                        ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);
            var solver = new SudokuSolver(sudoku);
            solver.Solve();
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod()]
        public void SolveEmpty6x6Test()
        {
            var puzzle = @"000000
                        000000
                        000000
                        000000
                        000000
                        000000
                        ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);
            var solver = new SudokuSolver(sudoku);
            solver.Solve();
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod()]
        public void SolveEmpty9x9Test()
        {
            var puzzle = @"000000000
                        000000000
                        000000000
                        000000000
                        000000000
                        000000000
                        000000000
                        000000000
                        000000000  
                        ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);
            var solver = new SudokuSolver(sudoku);
            solver.Solve();
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod()]
        public void SolveEmpty12x12Test()
        {
            var puzzle = @"0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                        ".Replace(" ", "");
            var sudoku = SudokuFactory.CreateFromString(puzzle);
            var solver = new SudokuSolver(sudoku);
            solver.Solve();
            Assert.IsTrue(sudoku.IsSolved());
        }
    }
}
