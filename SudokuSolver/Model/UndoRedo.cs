using SudokuSolver.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model
{
    /// <summary>
    /// Class storing actions of setting values to Sudoku's cells in order to perform 'undo' and 'redo' actions.
    /// </summary>
    [Serializable]
    public class UndoRedo
    {
        /// <summary>
        /// Initialize UndoRedo object.
        /// </summary>
        public UndoRedo() { }

        /// <summary>
        /// Initialize UndoRedo object by deep coping given copyFrom object.
        /// </summary>
        /// <param name="copyFrom">Object to deep copy from.</param>
        public UndoRedo(UndoRedo copyFrom)
        {
            foreach (var item in copyFrom._Undo)
                _Undo.Add(item);
            foreach (var item in copyFrom._Redo)
                _Redo.Add(item);
        }

        /// <summary>
        /// Performed actions as list of tuples with row, column, old value, value and method.
        /// </summary>
        public List<(byte row, byte column, byte oldValue, byte value, string method)> _Undo = new List<(byte, byte, byte, byte, string)>();

        /// <summary>
        /// Actions after 'undo' operation are moved to 'redo' list.
        /// </summary>
        public List<(byte row, byte column, byte oldValue, byte value, string method)> _Redo = new List<(byte, byte, byte, byte, string)>();

        /// <summary>
        /// Store data of action performed on a cell. When new data is inserted, 'Redo' list is cleared.
        /// </summary>
        /// <param name="row">Cell's row parameter.</param>
        /// <param name="column">Cell's column parameter.</param>
        /// <param name="oldValue">Cell's value before writing a new value.</param>
        /// <param name="value">Value written to the cell.</param>
        /// <param name="method">Name of the method writing value (optional). Defaults to ''.</param>
        public void AddAction(byte row, byte column, byte oldValue, byte value, string method = "")
        {
            _Undo.Add((row, column, oldValue, value, method));
            _Redo.Clear();
        }

        /// <summary>
        /// Return length of Undo list.
        /// </summary>
        /// <returns>Length of Undo list</returns>
        public int UndoLength()
        {
            return _Undo.Count;
        }

        /// <summary>
        /// Return length of Redo list.
        /// </summary>
        /// <returns>Length of Rndo list</returns>
        public int RedoLength()
        {
            return _Redo.Count();
        }

        /// <summary>
        /// Get last action performed on Sudoku as: row, column, value before writing, value written, method, number of
        /// remaining actions in undo list and number of actions in redo list.Returned action is also written to redo list.
        /// It means that updating Cell's value with returned data should be performed without calling add_action function,
        /// because it clears redo list.
        /// </summary>
        /// <returns>Tuple: row, column, oldValue, value, method, lengthOfUndoList, lengthOfRedoList.</returns>
        public (byte row, byte column, byte oldValue, byte value, string method, int lengthOfUndoList, int lengthOfRedoList) Undo()
        {
            var (row, column, oldValue, value, method) = _Undo.Pop();
            _Redo.Add((row, column, oldValue, value, method));
            return (row, column, oldValue, value, method, _Undo.Count, _Redo.Count);
        }

        /// <summary>
        /// Get undone action. Returned action is also written to undo list. If you want to write value to a cell, do not
        /// call add_action function, because it clears redo list and adds action to undo list(you would get two the same
        /// actions in a row).
        /// </summary>
        /// <returns>Tuple: row, column, oldValue, value, method, lengthOfUndoList, lengthOfRedoList.</returns>
        public (byte row, byte column, byte oldValue, byte value, string method, int lengthOfUndoList, int lengthOfRedoList) Redo()
        {
            var (row, column, oldValue, value, method) = _Redo.Pop();
            _Undo.Add((row, column, oldValue, value, method));
            return (row, column, oldValue, value, method, _Undo.Count, _Redo.Count);
        }
    }
}
