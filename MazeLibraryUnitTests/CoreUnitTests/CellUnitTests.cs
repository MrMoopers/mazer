using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary;
using System.Drawing;
using MazeLibrary.Helpers;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class CellUnitTests : AlgorithmTestBase
    {
        [TestMethod]
        public void CellNameIsCorrect()
        {
            Cell cell = new Cell("Cell_New", 0, 0);
            Assert.AreSame("Cell_New", cell.Name);

        }

        [TestMethod]
        public void CellIntialisedEdgeCountIsCorrect()
        {
            Cell cell = _tinyGrid.CreateCell(0, 0);

            Assert.IsTrue(cell.InitialisedEdgeCount == 0);

            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                Assert.IsTrue(cell.InitialisedEdgeCount == i);
                _tinyGrid.CreateWall(cell, null, (Direction)i);
                Assert.IsTrue(cell.InitialisedEdgeCount == i + 1);
            }
        }

        [TestMethod]
        public void CellisFullyIntialisedIsCorrect()
        {
            Cell cell = _tinyGrid.CreateCell(0, 0);

            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                _tinyGrid.CreateWall(cell, null, (Direction)i);
            }
            Assert.IsTrue(cell.IsFullyInitialized);
        }

        [TestMethod]
        public void CellGetEdgeReturnsCorrectEdge()
        {
            _tinyGrid.CreateCell(0, 0);
            Cell cell = _tinyGrid.GetCell(0, 0);

            _tinyGrid.CreateWall(cell, null, Direction.North);

            Assert.IsTrue(cell.GetEdge(Direction.North) is CellWall);
        }

        [TestMethod]
        public void CellSetsEdgeCorrectly()
        {
            _smallGrid.CreateCell(0, 0);
            Cell cell = _smallGrid.GetCell(0, 0);
            CellPassage cp = new CellPassage();

            cell.SetEdge(Direction.North, cp);

            Assert.IsTrue(cell.GetEdge(Direction.North) is CellPassage);


        }

        [TestMethod]
        public void CellGetsRandomUninitializedDirectionCorrectly()
        {
            _smallGrid.CreateCell(0, 0);
            Cell cell = _smallGrid.GetCell(0, 0);

            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                Direction direction = cell.GetRandomUninitializedDirection();
                Assert.IsTrue(cell.GetEdge(direction) == null);
                _smallGrid.CreateWall(cell, null, (Direction)i);
            }
        }

        //[TestMethod]
        //public void CellGetsRandomUninitializedDirectionReturnsNullIfAllCellEdgesIntialized() //Purposely fails
        //{
        //    _smallGrid.CreateCell(0, 0);
        //    Cell cell = _smallGrid.GetCell(0, 0);

        //    for (int i = 0; i < DirectionHelper.Count; i++)
        //    {
        //        Direction direction = cell.GetRandomUninitializedDirection();
        //        _smallGrid.CreateWall(cell, null, (Direction)i);
        //    }

        //    Assert.IsTrue(cell.GetRandomUninitializedDirection() == null);
        //}
    }
}
