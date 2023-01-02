using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary.Algorithms;
using MazeLibrary;
using MazeLibraryUnitTests.Helpers;
using System.Collections.Generic;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class RecursiveBackTrackerAlgorithmUnitTest : AlgorithmTestBase
    {
        private RecursiveBackTrackerAlgorithm _recursiveBackTrackerAlgorithm = new RecursiveBackTrackerAlgorithm(0);

        [TestMethod]
        public void CheckRouteEqualsActualRouteGrid1()
        {
            Grid grid = GridFactory.GetGrid1();

            List<Cell> route = new List<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            _recursiveBackTrackerAlgorithm.Solve(ref grid, out route, out visitedCells);
            

            CheckAllInRouteAreSameGrid1(route);
        }

        [TestMethod]
        public void CheckRouteEqualsActualRouteGrid2()
        {
            Grid grid = GridFactory.GetGrid2();

            List<Cell> route = new List<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            _recursiveBackTrackerAlgorithm.Solve(ref grid, out route, out visitedCells);
            

            CheckAllInRouteAreSameGrid2(route);
        }

        [TestMethod]
        public void GridHasCorrectNumberOfCells()
        {
            _recursiveBackTrackerAlgorithm.Generate(_smallGrid);
            Assert.AreEqual(30, _smallGrid.Width * _smallGrid.Height);
        }

        [TestMethod]
        public void AllCellsExist()
        {
            _recursiveBackTrackerAlgorithm.Generate(_smallGrid);
            CheckAllCellsExist(_smallGrid);
        }

        [TestMethod]
        public void AllEdgesAreFullyIntialized()
        {
            _recursiveBackTrackerAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveFullyInitializedEdges(_smallGrid);
        }

        [TestMethod]
        public void AllBordersCreated()
        {
            _recursiveBackTrackerAlgorithm.Generate(_smallGrid);
            CheckAllBordersExist(_smallGrid);
        }

        [TestMethod]
        public void AllCellsHaveAtLeastOnePassageWay()
        {
            _recursiveBackTrackerAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveAtLeastOnePassageWay(_smallGrid);
        }
    }
}
