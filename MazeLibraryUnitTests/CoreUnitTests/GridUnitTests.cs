using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary;
using System.Drawing;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class GridUnitTests : AlgorithmTestBase
    {
        [TestMethod]
        public void AllGridsHaveCorrectNames()
        {
            Assert.AreSame("tinyGrid", _tinyGrid.Name);
            Assert.AreSame("smallGrid", _smallGrid.Name);
            Assert.AreSame("mediumGrid", _mediumGrid.Name);
            Assert.AreSame("largeGrid", _largeGrid.Name);
            Assert.AreSame("veryLargeGrid", _veryLargeGrid.Name);
            Assert.AreSame("extremelyLargeGrid", _extremelyLargeGrid.Name);
        }

        [TestMethod]
        public void AllGridsHaveCorrectWidths()
        {
            Assert.IsTrue(1 == _tinyGrid.Width);
            Assert.IsTrue(5 == _smallGrid.Width);
            Assert.IsTrue(11 == _mediumGrid.Width);
            Assert.IsTrue(31 == _largeGrid.Width);
            Assert.IsTrue(102 == _veryLargeGrid.Width);
            Assert.IsTrue(210 == _extremelyLargeGrid.Width);
        }

        [TestMethod]
        public void AllGridsHaveCorrectHeights()
        {
            Assert.IsTrue(1 == _tinyGrid.Height);
            Assert.IsTrue(6 == _smallGrid.Height);
            Assert.IsTrue(13 == _mediumGrid.Height);
            Assert.IsTrue(54 == _largeGrid.Height);
            Assert.IsTrue(74 == _veryLargeGrid.Height);
            Assert.IsTrue(190 == _extremelyLargeGrid.Height);
        }

        [TestMethod]
        public void GridCreatesNonNullCell()
        {
            Cell cell = _smallGrid.CreateCell(0, 0);
            Assert.IsNotNull(cell);
        }

        [TestMethod]
        public void GridContainsOnGridPoint()
        {
            int x = 0;
            int y = 0;
         
            Assert.IsTrue(_smallGrid.ContainsPoint(x, y));
        }

        [TestMethod]
        public void GridContainsOffGridPoint()
        {
            int x = -1;
            int y = -1;

            Assert.IsFalse(_smallGrid.ContainsPoint(x, y));
        }

        [TestMethod]
        public void GridCreatesPassage()
        {
            Cell cell1 = _smallGrid.CreateCell(0, 0);
            Cell cell2 = _smallGrid.CreateCell(1, 0);

            _smallGrid.CreatePassage(cell1, cell2, Direction.East);

            Assert.IsTrue(cell1.GetEdge(Direction.East) is CellPassage);
            Assert.IsTrue(cell2.GetEdge(Direction.West) is CellPassage);

            Assert.IsFalse(cell1.GetEdge(Direction.East) is CellWall);
            Assert.IsFalse(cell2.GetEdge(Direction.West) is CellWall);
        }

        [TestMethod]
        public void GridCreatesWall()
        {
            Cell cell1 = _smallGrid.CreateCell(0, 0);
            Cell cell2 = _smallGrid.CreateCell(1, 0);

            _smallGrid.CreateWall(cell1, cell2, Direction.East);

            Assert.IsTrue(cell1.GetEdge(Direction.East) is CellWall);
            Assert.IsTrue(cell2.GetEdge(Direction.West) is CellWall);

            Assert.IsFalse(cell1.GetEdge(Direction.East) is CellPassage);
            Assert.IsFalse(cell2.GetEdge(Direction.West) is CellPassage);
        }

        [TestMethod]
        public void GridGetsNonNullCell()
        {
            _smallGrid.CreateCell(0, 0);

            Cell cell = _smallGrid.GetCell(0,0);
            Assert.IsNotNull(cell);
        }
    }
}
