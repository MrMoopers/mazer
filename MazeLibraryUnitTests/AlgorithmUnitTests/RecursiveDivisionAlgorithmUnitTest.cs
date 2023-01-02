using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary.Algorithms;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class RecursiveDivisionAlgorithmUnitTest : AlgorithmTestBase
    {
        private RecursiveDivisionAlgorithm _recursiveDivisionAlgorithm = new RecursiveDivisionAlgorithm(0);

        [TestMethod]
        public void GridHasCorrectNumberOfCells()
        {
            _recursiveDivisionAlgorithm.Generate(_smallGrid);
            Assert.AreEqual(30, _smallGrid.Width * _smallGrid.Height);
        }

        [TestMethod]
        public void AllCellsExist()
        {
            _recursiveDivisionAlgorithm.Generate(_smallGrid);
            CheckAllCellsExist(_smallGrid);
        }

        [TestMethod]
        public void AllEdgesAreFullyIntialized()
        {
            _recursiveDivisionAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveFullyInitializedEdges(_smallGrid);
        }

        [TestMethod]
        public void AllBordersCreated()
        {
            _recursiveDivisionAlgorithm.Generate(_smallGrid);
            CheckAllBordersExist(_smallGrid);
        }

        [TestMethod]
        public void AllCellsHaveAtLeastOnePassageWay()
        {
            _recursiveDivisionAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveAtLeastOnePassageWay(_smallGrid);
        }
    }
}
