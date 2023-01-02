using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary.Algorithms;
using MazeLibrary;
using MazeLibraryUnitTests.Helpers;
using System.Collections.Generic;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class DrunkardsWalkAlgorithmUnitTest : AlgorithmTestBase
    {
        private DrunkardsWalkAlgorithm _drunkardsWalkAlgorithm = new DrunkardsWalkAlgorithm(0);

        [TestMethod]
        public void CheckRouteEqualsActualRouteGrid1()
        {
            Grid grid = GridFactory.GetGrid1();

            List<Cell> route = new List<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            _drunkardsWalkAlgorithm.Solve(ref grid, out route, out visitedCells);
            

            CheckAllInRouteAreSameGrid1(route);
        }

        [TestMethod]
        public void CheckRouteEqualsActualRouteGrid2()
        {
            Grid grid = GridFactory.GetGrid2();

            List<Cell> route = new List<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            _drunkardsWalkAlgorithm.Solve(ref grid, out route, out visitedCells);
            

            CheckAllInRouteAreSameGrid2(route);
        }

        [TestMethod]
        public void GridHasCorrectNumberOfCells()
        {
            _drunkardsWalkAlgorithm.Generate(_smallGrid);
            Assert.AreEqual(30, _smallGrid.Width * _smallGrid.Height);
        }

        [TestMethod]
        public void AllCellsExist()
        {
            _drunkardsWalkAlgorithm.Generate(_smallGrid);
            CheckAllCellsExist(_smallGrid);
        }

        [TestMethod]
        public void AllEdgesAreFullyIntialized()
        {
            _drunkardsWalkAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveFullyInitializedEdges(_smallGrid);
        }

        [TestMethod]
        public void AllBordersCreated()
        {
            _drunkardsWalkAlgorithm.Generate(_smallGrid);
            CheckAllBordersExist(_smallGrid);
        }

        [TestMethod]
        public void AllCellsHaveAtLeastOnePassageWay()
        {
            _drunkardsWalkAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveAtLeastOnePassageWay(_smallGrid);
        }
    }
}
