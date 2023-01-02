using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary;
using System.Drawing;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class CellEdgeUnitTest : AlgorithmTestBase
    {
        [TestMethod]
        public void CellEdgeHasCorrectDirection()
        {
            Cell cell = _smallGrid.CreateCell(0, 0);
            Cell neighbourCell;
            CellEdge cellEdge;

            _smallGrid.CreateWall(cell, null, Direction.North);
            cellEdge = cell.GetEdge(Direction.North);
            Assert.IsTrue(cellEdge.Direction == Direction.North);

            neighbourCell = _smallGrid.CreateCell(1, 0);
            _smallGrid.CreatePassage(cell, neighbourCell, Direction.East);
            cellEdge = cell.GetEdge(Direction.East);
            Assert.IsTrue(cellEdge.Direction == Direction.East);

            neighbourCell = _smallGrid.CreateCell(0, 1);
            _smallGrid.CreateWall(cell, neighbourCell, Direction.South);
            cellEdge = cell.GetEdge(Direction.South);
            Assert.IsTrue(cellEdge.Direction == Direction.South);
        }

        [TestMethod]
        public void CellEdgeHasCorrectStartCell()
        {
            Cell cell = _smallGrid.CreateCell(0, 0);
            Cell neighbourCell;
            CellEdge cellEdge;

            _smallGrid.CreateWall(cell, null, Direction.North);
            cellEdge = cell.GetEdge(Direction.North);
            Assert.IsTrue(cellEdge.StartCell == cell);

            neighbourCell = _smallGrid.CreateCell(1, 0);
            _smallGrid.CreatePassage(cell, neighbourCell, Direction.East);
            cellEdge = cell.GetEdge(Direction.East);
            Assert.IsTrue(cellEdge.StartCell == cell);

            neighbourCell = _smallGrid.CreateCell(0, 1);
            _smallGrid.CreateWall(cell, neighbourCell, Direction.South);
            cellEdge = cell.GetEdge(Direction.South);
            Assert.IsTrue(cellEdge.StartCell == cell);

        }

        [TestMethod]
        public void CellEdgeHasCorrectEndCell()
        {
            Cell cell = _smallGrid.CreateCell(0, 0);
            Cell neighbourCell;
            CellEdge cellEdge;

            _smallGrid.CreateWall(cell, null, Direction.North);
            cellEdge = cell.GetEdge(Direction.North);
            Assert.IsTrue(cellEdge.EndCell == null);

            neighbourCell = _smallGrid.CreateCell(1, 0);
            _smallGrid.CreatePassage(cell, neighbourCell, Direction.East);
            cellEdge = cell.GetEdge(Direction.East);
            Assert.IsTrue(cellEdge.EndCell == neighbourCell);

            neighbourCell = _smallGrid.CreateCell(0, 1);
            _smallGrid.CreateWall(cell, neighbourCell, Direction.South);
            cellEdge = cell.GetEdge(Direction.South);
            Assert.IsTrue(cellEdge.EndCell == neighbourCell);
        }

        //[TestMethod]
        //public void CellEdgeIntializedCorrectly()
        //{
        //    Cell cell = _smallGrid.CreateCell(0, 0);
        //    Cell neighbourCell = _smallGrid.CreateCell(1, 0);
        //    _smallGrid.CreateWall(cell, neighbourCell, Direction.East);

        //    CellEdge cellEdge = cell.GetEdge(Direction.East);

        //    cellEdge.Initialize(cell, neighbourCell, Direction.East);

        //}
    }
}
