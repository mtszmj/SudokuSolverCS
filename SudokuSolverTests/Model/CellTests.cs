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
    public class CellTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CellNotEditableAndValueIsZeroTest()
        {
            // arrange
            Cell cell;

            // act
            cell = new Cell(0, 0, 9, editable: false, value: 0);

            // assert - Expects exception
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CellValueOverMaxTest()
        {
            // arrange
            byte MAX_VALUE = 9;
            Cell cell;

            // act
            cell = new Cell(0, 0, MAX_VALUE, value: (byte)(MAX_VALUE + 1));

            // assert - Expects exception
        }

        [TestMethod()]
        public void CellNotEditableAndCorrectValueTest()
        {
            // arrange
            byte MAX_VALUE = 9;
            Cell cell;

            // act
            cell = new Cell(0, 0, MAX_VALUE, editable: false, value: 1);

            // assert
            Assert.IsFalse(cell.Editable);
            Assert.AreEqual(1, cell.Value);
            Assert.AreEqual(0, cell.PossibleValues.Count);
        }

        [TestMethod()]
        public void CellIsEditableAndCorrectValueTest()
        {
            // arrange
            byte MAX_VALUE = 9;
            Cell cell;

            // act
            cell = new Cell(0, 0, MAX_VALUE, editable: true, value: 1);

            // assert
            Assert.IsTrue(cell.Editable);
            Assert.AreEqual(1, cell.Value);
            Assert.AreEqual(9, cell.PossibleValues.Count);
        }

        [TestMethod()]
        public void SetValueTest()
        {
            // arrange
            var cell = new Cell(0, 0, 9, true, 0);

            // act
            cell.Value = 1;

            //assert
            Assert.AreEqual(1, cell.Value);
            Assert.AreEqual(0, cell.PossibleValues.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void SetValueCellNotEditableTest()
        {
            // arrange
            var cell = new Cell(0, 0, 9, false, 0);

            // act
            cell.Value = 1;

            // assert
        }

        [TestMethod()]
        public void ClearTest()
        {
            // arrange
            var cell = new Cell(0, 0, 9, true, 1);

            // act
            cell.Clear();

            // assert
            Assert.AreEqual(0, cell.Value);
        }

        [TestMethod()]
        public void ClearCellNotEditableTest()
        {
            // arrange
            var cell = new Cell(0, 0, 9, false, 1);

            // act
            cell.Clear();

            // assert
            Assert.AreEqual(1, cell.Value);
        }

        [TestMethod()]
        public void PossibleValuesTest()
        {
            // arrange
            byte MAX_VALUE = 9;
            Cell cell;
            ISet<byte> possibleValues = new HashSet<byte>();
            for (byte value = 1; value <= MAX_VALUE; value++)
            {
                possibleValues.Add(value);
            }

            // act
            cell = new Cell(0, 0, MAX_VALUE, true, 0);

            // assert
            bool AreEqual = cell.PossibleValues.SetEquals(possibleValues);
            Assert.IsTrue(AreEqual);
        }

        [TestMethod()]
        public void InitPossibleValuesTest()
        {
            // arrange
            byte MAX_VALUE = 9;
            var cell = new Cell(0, 0, MAX_VALUE, true, 1);

            // act
            cell.Clear();
            cell.InitPossibleValues(MAX_VALUE);

            // assert
            Assert.AreEqual(MAX_VALUE, cell.PossibleValues.Count);
        }

        [TestMethod()]
        public void IntersectPossibleValuesTest()
        {
            // arrange
            var cell = new Cell(0, 0, 9, true, 0);
            ISet<byte> interectValues = new HashSet<byte>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // act and assert
            interectValues.Remove(1);
            cell.IntersectPossibleValues(interectValues);
            Assert.IsTrue(cell.PossibleValues.SetEquals(interectValues));

            interectValues.Remove(2);
            interectValues.Remove(3);
            cell.IntersectPossibleValues(interectValues);
            Assert.IsTrue(cell.PossibleValues.SetEquals(interectValues));
            Assert.AreEqual(6, cell.PossibleValues.Count);
        }

        [TestMethod()]
        public void RemovePossibleValueTest()
        {
            // arrange
            byte MAX_VALUE = 9;
            var cell = new Cell(0, 0, MAX_VALUE, true, 0);
            var set = new HashSet<byte>() { 2, 3, 4, 5, 6, 7, 8, 9 };

            // act
            cell.RemovePossibleValue(1);

            // assert
            Assert.IsTrue(cell.PossibleValues.SetEquals(set));
        }
    }
}