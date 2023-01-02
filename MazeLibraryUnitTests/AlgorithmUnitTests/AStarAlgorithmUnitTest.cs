using MazeLibrary;
using MazeLibrary.Algorithms;
using MazeLibraryUnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class AStarAlgorithmUnitTest : AlgorithmTestBase
    {
        private AStarAlgorithm _aStarAlgorithm = new AStarAlgorithm(0);

        [TestMethod]
        public void CheckRouteEqualsActualRouteGrid1()
        {
            Grid grid = GridFactory.GetGrid1();

            List<Cell> route = new List<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            _aStarAlgorithm.Solve(ref grid, out route, out visitedCells);
            route.Reverse();

            CheckAllInRouteAreSameGrid1(route);
        }

        [TestMethod]
        public void CheckRouteEqualsActualRouteGrid2()
        {
            Grid grid = GridFactory.GetGrid2();

            List<Cell> route = new List<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            _aStarAlgorithm.Solve(ref grid, out route, out visitedCells);
            route.Reverse();

            CheckAllInRouteAreSameGrid2(route);
        }


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
