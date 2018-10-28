using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Model.Tests
{
    [TestClass()]
    public class RegionTests
    {
        [TestMethod()]
        public void AddTest()
        {
            // arrange
            var region = new Region();
            var cell01 = new Cell(0, 0, 9);
            var cell02 = new Cell(0, 0, 9);

            // act
            region.Add(cell01);
            Cell cell = region.Cells.ToList()[0];

            // assert
            Assert.AreEqual(1, region.Cells.Count);
            Assert.AreEqual(cell01, cell);
            Assert.IsFalse(region.Cells.Contains(cell02));
        }

        [TestMethod()]
        public void RemovePossibleValueCellIsInRegionTest()
        {
            // arrange
            var region = new Region();
            var cell01 = new Cell(0, 0, 9);
            var set_2_9 = new HashSet<byte>();

            for (byte i = 2; i <= 9; i++)
            {
                set_2_9.Add(i);
            }

            // act
            region.Add(cell01);
            region.RemovePossibleValueIfCellIsInRegion(cell01, 1);

            // assert
            Assert.IsTrue(cell01.PossibleValues.SetEquals(set_2_9));
        }

        [TestMethod()]
        public void RemovePossibleValueCellIsNotInRegionTest()
        {
            // arrange
            var region = new Region();
            var cell01 = new Cell(0, 0, 9);
            var cell02 = new Cell(0, 0, 9);
            var set_1_9 = new HashSet<byte>();

            for (byte i = 1; i <= 9; i++)
            {
                set_1_9.Add(i);
            }

            // act
            region.Add(cell01);
            region.RemovePossibleValueIfCellIsInRegion(cell02, 1);

            // assert
            Assert.IsTrue(cell01.PossibleValues.SetEquals(set_1_9));
        }

        [TestMethod()]
        public void UpdatePossibleValuesTest()
        {
            // arrange
            byte MAX_VALUE = 4;
            var region = new Region();
            Cell[] cells = new Cell[] {
                new Cell(0, 0, MAX_VALUE),
                new Cell(0, 1, MAX_VALUE),
                new Cell(0, 2, MAX_VALUE),
                new Cell(0, 3, MAX_VALUE, false, 1)
            };
            foreach(var cell in cells)
            {
                region.Add(cell);
            }
            var set_2_9 = new HashSet<byte>() { 2, 3, 4 };

            // act
            region.UpdatePossibleValues();


            // assert
            foreach(var cell in region.Cells)
            {
                if (cell.Value == 0)
                {
                    Assert.IsTrue(cell.PossibleValues.SetEquals(set_2_9));
                }
                else
                {
                    Assert.AreEqual(0, cell.PossibleValues.Count);
                }
            }
        }

        [TestMethod()]
        public void IsNotSolvedTest()
        {
            // arrange
            var region = new Region();
            region.Add(new Cell(0, 0, 4, true, 0));
            region.Add(new Cell(0, 1, 4, true, 2));
            region.Add(new Cell(0, 2, 4, true, 3));
            region.Add(new Cell(0, 3, 4, false, 4));

            // act

            // assert
            Assert.IsFalse(region.IsSolved());
        }

        [TestMethod()]
        public void IsSolvedTest()
        {
            // arrange
            var region = new Region();
            region.Add(new Cell(0, 0, 4, true, 0));
            region.Add(new Cell(0, 1, 4, true, 2));
            region.Add(new Cell(0, 2, 4, true, 3));
            region.Add(new Cell(0, 3, 4, false, 4));

            // act
            region.Cells.ToList()[0].Value = 1;

            // assert
            Assert.IsTrue(region.IsSolved());
        }

        [TestMethod()]
        public void IsWrongTest()
        {
            // arrange
            var region = new Region();
            region.Add(new Cell(0, 0, 4, true, 2));
            region.Add(new Cell(0, 1, 4, true, 2));
            region.Add(new Cell(0, 2, 4, true, 3));
            region.Add(new Cell(0, 3, 4, false, 4));
            
            // act

            // assert
            Assert.IsFalse(region.IsSolved());
        }

        [TestMethod()]
        public void IsNotPossibleToSolveTest()
        {
            // arrange
            var region = new Region();
            var cell = new Cell(0, 0, 4, true, 0);
            region.Add(cell);
            region.Add(new Cell(0, 1, 4, true, 2));
            region.Add(new Cell(0, 2, 4, true, 3));
            region.Add(new Cell(0, 3, 4, false, 4));

            // act
            cell.IntersectPossibleValues(new HashSet<byte>());

            // assert
            Assert.IsTrue(region.IsNotPossibleToSolve());
        }
    }
}