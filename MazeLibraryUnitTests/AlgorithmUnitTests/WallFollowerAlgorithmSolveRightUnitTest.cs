using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary.Algorithms;
using MazeLibrary;
using System.Collections.Generic;
using MazeLibraryUnitTests.Helpers;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class WallFollowerAlgorithmSolveRightUnitTest : AlgorithmTestBase
    {
        private WallFollowerAlgorithm _wallFollowerAlgorithm = new WallFollowerAlgorithm(0);

        [TestMethod]
        public void CheckRouteEqualsActualRouteGrid1()
        {
            Grid grid = GridFactory.GetGrid1();

            List<Cell> route = new List<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            _wallFollowerAlgorithm.SolveRight(ref grid, out route, out visitedCells);
            

            CheckAllInRouteAreSameGrid1(route);
        }

        [TestMethod]
        public void CheckRouteEqualsActualRouteGrid2()
        {
            Grid grid = GridFactory.GetGrid2();

            List<Cell> route = new List<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            _wallFollowerAlgorithm.SolveRight(ref grid, out route, out visitedCells);
            

            CheckAllInRouteAreSameGrid2(route);
        }
        //[TestMethod]
        //public void GridHasCorrectNumberOfCells()
        //{
        //    _wallFollowerAlgorithmSolveLeft.Generate(_smallGrid);
        //    Assert.AreEqual(30, _smallGrid.Width * _smallGrid.Height);
        //}

        //[TestMethod]
        //public void AllCellsExist()
        //{
        //    _wallFollowerAlgorithmSolveLeft.Generate(_smallGrid);
        //    CheckAllCellsExist(_smallGrid);
        //}

        //[TestMethod]
        //public void AllEdgesAreFullyIntialized()
        //{
        //    _wallFollowerAlgorithmSolveLeft.Generate(_smallGrid);
        //    CheckAllCellsHaveFullyInitializedEdges(_smallGrid);
        //}

        //[TestMethod]
        //public void AllBordersCreated()
        //{
        //    _wallFollowerAlgorithmSolveLeft.Generate(_smallGrid);
        //    CheckAllBordersExist(_smallGrid);
        //}

        //[TestMethod]
        //public void AllCellsHaveAtLeastOnePassageWay()
        //{
        //    _wallFollowerAlgorithmSolveLeft.Generate(_smallGrid);
        //    CheckAllCellsHaveAtLeastOnePassageWay(_smallGrid);
        //}
    }
}
