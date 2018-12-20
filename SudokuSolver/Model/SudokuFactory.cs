using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Model
{
    /// <summary>
    /// Sudoku factory class.
    /// </summary>
    public static class SudokuFactory
    {
        /// <summary>
        /// Separator used for bigger sudokus.
        /// </summary>
        public const string SEPARATOR = ",";

        /// <summary>
        /// dictionary holding possible sizes of Sudoku as keys and size of rectangle as values.
        /// </summary>
        public static Dictionary<int, (byte, byte)> POSSIBLE_SIZES { get; } = new Dictionary<int, (byte, byte)>()
        {
            [4] = (2, 2),
            [6] = (3, 2),
            [9] = (3, 3),
            [12] = (4, 3),
        };

        /// <summary>
        /// Create sudoku instance from multiline string.
        /// </summary>
        /// <example>
        /// Correct format of sudoku is:
        /// "293040100
        /// 516230740
        /// 847156000
        /// 354002690
        /// 600415000
        /// 000900000
        /// 000394802
        /// 000600005
        /// 000521000"
        /// or:
        /// "134265
        /// 526134
        /// 243516
        /// 615423
        /// 461352
        /// 352641"
        /// For bigger sudoku than 9, values should be separated by SudokuFactory.SEPARATOR - ','.
        /// </example>
        /// <param name="sudoku">Sudoku written as a string.</param>
        /// <returns></returns>
        public static Sudoku CreateFromString(string sudoku)
        {
            var lines = new List<List<string>>();
            var withSeparator = false;

            foreach (var line_ in sudoku.Split('\n'))
            {
                var sublist = new List<string>();
                var line = line_.Trim();
                if (!withSeparator && line.Contains(SEPARATOR))
                {
                    withSeparator = true;
                }
                if (withSeparator && decimal.TryParse(line.Replace(SEPARATOR, ""), out decimal _))
                {
                    sublist.AddRange(line.Split(new string[] { SEPARATOR }, StringSplitOptions.None));
                    lines.Add(sublist);
                }
                else if (decimal.TryParse(line, out decimal _))
                {
                    sublist.AddRange(line.Select(s => s.ToString()));
                    lines.Add(sublist);
                }
            }

            byte size = (byte)lines.Count;
            foreach (var line in lines)
            {
                if ((line.Count != size) || !(POSSIBLE_SIZES.ContainsKey(size)))
                {
                    throw new ArgumentException("Incorrect input string");
                }

            }

            Cell[,] cells = new Cell[size, size];
            for (byte row = 0; row < size; row++)
            {
                for (byte column = 0; column < size; column++)
                {
                    var value = byte.Parse(lines[row][column].ToString());
                    if (value != 0)
                        cells[row, column] = new Cell(row, column, size, false, value);
                    else
                        cells[row, column] = new Cell(row, column, size);
                }
            }

            var (width, height) = POSSIBLE_SIZES[size];
            return new Sudoku(cells, size, width, height);
        }
    }
}
