using SudokuSolver.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model
{
    /// <summary>
    /// A class solving sudoku by Brute Force (recursively). Extends Pattern class.
    /// </summary>
    public class BruteForce : Pattern
    {
        /// <summary>
        /// Object of internal class SudokuNode.
        /// </summary>
        private SudokuNode _Root;

        /// <summary>
        /// Solve sudoku by Brute Force (recursively).
        /// 
        /// Algorithm:
        /// Create object of internal class SudokuNode as a root of recursive method.If the sudoku is not wrong call
        /// 'solve' method the root object.
        /// </summary>
        /// <param name="sudoku">Sudoku object to solve.</param>
        /// <param name="solveOne">Parameter to enable finishing function after writing one value to a cell.</param>
        /// <returns>True if sudoku was solved. Otherwise False.</returns>
        public override bool Solve(Sudoku sudoku, bool solveOne = false)
        {
            _Root = new SudokuNode(sudoku);
            SudokuNode.SudokuSolved = null;

            if (!sudoku.IsWrong())
            {
                _Root.Solve();
                if(SudokuNode.SudokuSolved != null)
                {
                    sudoku.CopyFrom(SudokuNode.SudokuSolved);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Class representing variation of sudoku as a node of the tree in a recursive method of solving sudoku.
        /// </summary>
        private class SudokuNode
        {
            /// <summary>
            /// Method name.
            /// </summary>
            private const string METHOD = "BruteForce";

            /// <summary>
            /// Static object of Sudoku to which solved sudoku is written.
            /// </summary>
            public static Sudoku SudokuSolved = null;

            /// <summary>
            /// Collection of other patterns used to decrease number of iterations of Brute Force.
            /// </summary>
            private static IEnumerable<Pattern> Patterns { get; } = Pattern.GetPatternsWithoutBruteForce();

            /// <summary>
            /// List of permutations.
            /// </summary>
            private ICollection<SudokuNode> Children { get; } = new List<SudokuNode>();

            /// <summary>
            /// Current permutation of sudoku.
            /// </summary>
            private Sudoku Sudoku { get; set; }

            /// <summary>
            /// Initiate the SudokuNode with sudoku and optional arguments for writing value to a cell.
            /// 
            /// Write the value to a cell of row and column if the arguments are specified and not null.
            /// </summary>
            /// <param name="sudoku">Sudoku board.</param>
            /// <param name="row">A row number of a cell to which value should be written. Defaults to null.</param>
            /// <param name="column">A column number of a cell to which value should be written. Defaults to null.</param>
            /// <param name="value">Value to write to a cell. Defaults to null.</param>
            public SudokuNode(Sudoku sudoku, byte? row = null, byte? column = null, byte? value = null)
            {
                //Sudoku = new Sudoku(sudoku);
                Sudoku = sudoku.DeepCopy(); // changed from usage of copying constructors to 'Prototype' via serialization.
                if(row != null && column != null && value > 0)
                {
                    Sudoku.SetCellValue(row.GetValueOrDefault(), column.GetValueOrDefault(), value.GetValueOrDefault(), METHOD);
                }
            }

            /// <summary>
            /// Solve sudoku with BruteForce.
            ///
            /// Algorithm:
            /// First use typical patterns like OnePossibility and Exclusion in order to minimize recursion.
            ///
            /// If sudoku is solved - finish.If it is unsolvable return False.
            /// 
            /// Go through each row and column and if there is a cell with no value (zero), create children, each with one
            /// of the possible values written to the cell.
            ///
            /// Go through children and call solve method.
            /// </summary>
            public bool Solve()
            {
                foreach (var pattern in Patterns)
                {
                    var solveAgain = true;
                    while(solveAgain)
                    {
                        solveAgain = pattern.Solve(Sudoku, false);
                    }
                }

                if (Sudoku.IsSolved())
                {
                    BruteForce.SudokuNode.SudokuSolved = Sudoku;
                    return true;
                }
                else if (Sudoku.IsWrong())
                {
                    return false;
                }

                bool isDone = false;
                for(byte row = 0; row < Sudoku.Size; row++)
                {
                    for(byte column = 0; column < Sudoku.Size; column++)
                    {
                        if(Sudoku.GetCellValue(row, column) == 0)
                        {
                            foreach(var possibility in Sudoku.GetCellPossibilities(row, column))
                            {
                                var sudokuNode = new BruteForce.SudokuNode(Sudoku, row, column, possibility);
                                Children.Add(sudokuNode);
                            }
                            isDone = true;
                            break;
                        }
                    }
                    if (isDone)
                        break;
                }

                foreach(var child in Children)
                {
                    var result = child.Solve();
                    if (result)
                        return true;
                }

                return false;
            }
        }

    }
}
