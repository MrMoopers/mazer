using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary.Algorithms;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class WilsonsAlgorithmUnitTest : AlgorithmTestBase
    {
        private WilsonsAlgorithm _wilsonsAlgorithm = new WilsonsAlgorithm();

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
