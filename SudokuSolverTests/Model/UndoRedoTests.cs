using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model.Tests
{
    [TestClass()]
    public class UndoRedoTests
    {
        [TestMethod()]
        public void AddActionTest()
        {
            // arrange
            byte row = 1;
            byte column = 1;
            byte oldValue = 0;
            byte value = 3;
            string method = "manual";
            var undoRedo = new UndoRedo();

            // act
            undoRedo.AddAction(row, column, oldValue, value, method);
            undoRedo.AddAction(2, 2, 0, 4, "manual");

            // assert
            Assert.AreEqual(2, undoRedo.UndoLength());
        }

        [TestMethod()]
        public void UndoTest()
        {
            // arrange
            var undoRedo = new UndoRedo();
            undoRedo.AddAction(1, 2, 3, 4, "manual");
            undoRedo.AddAction(5, 6, 7, 8, "manually");

            // act and assert
            Assert.AreEqual(2, undoRedo.UndoLength());
            Assert.AreEqual(0, undoRedo.RedoLength());

            var (row, column, oldValue, value, method, undoLength, redoLength) = undoRedo.Undo();
            Assert.AreEqual(5, row);
            Assert.AreEqual(6, column);
            Assert.AreEqual(7, oldValue);
            Assert.AreEqual(8, value);
            Assert.AreEqual("manually", method);
            Assert.AreEqual(1, undoLength);
            Assert.AreEqual(1, redoLength);

            (row, column, oldValue, value, method, undoLength, redoLength) = undoRedo.Undo();
            Assert.AreEqual(1, row);
            Assert.AreEqual(2, column);
            Assert.AreEqual(3, oldValue);
            Assert.AreEqual(4, value);
            Assert.AreEqual("manual", method);
            Assert.AreEqual(0, undoLength);
            Assert.AreEqual(2, redoLength);
        }

        [TestMethod()]
        public void RedoTest()
        {
            // arrange
            var undoRedo = new UndoRedo();
            undoRedo.AddAction(1, 2, 3, 4, "manual");
            undoRedo.AddAction(5, 6, 7, 8, "manually");

            // act
            var (row, column, oldValue, value, method, undoLength, redoLength) = undoRedo.Undo();
            (row, column, oldValue, value, method, undoLength, redoLength) = undoRedo.Undo();
            (row, column, oldValue, value, method, undoLength, redoLength) = undoRedo.Redo();

            // assert
            Assert.AreEqual(1, row);
            Assert.AreEqual(2, column);
            Assert.AreEqual(3, oldValue);
            Assert.AreEqual(4, value);
            Assert.AreEqual("manual", method);
            Assert.AreEqual(1, undoLength);
            Assert.AreEqual(1, redoLength);
        }
    }
}