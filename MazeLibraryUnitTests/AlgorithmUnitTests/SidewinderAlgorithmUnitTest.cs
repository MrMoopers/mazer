using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary.Algorithms;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class SidewinderAlgorithmUnitTest : AlgorithmTestBase
    {
        private SidewinderAlgorithm _sidewinderAlgorithm = new SidewinderAlgorithm(0);

        [TestMethod]
        public void GridHasCorrectNumberOfCells()
        {
            _sidewinderAlgorithm.Generate(_smallGrid);
            Assert.AreEqual(30, _smallGrid.Width * _smallGrid.Height);
        }

        [TestMethod]
        public void AllCellsExist()
        {
            _sidewinderAlgorithm.Generate(_smallGrid);
            CheckAllCellsExist(_smallGrid);
        }

        [TestMethod]
        public void AllEdgesAreFullyIntialized()
        {
            _sidewinderAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveFullyInitializedEdges(_smallGrid);
        }

        [TestMethod]
        public void AllBordersCreated()
        {
            _sidewinderAlgorithm.Generate(_smallGrid);
            CheckAllBordersExist(_smallGrid);
        }

        [TestMethod]
        public void AllCellsHaveAtLeastOnePassageWay()
        {
            _sidewinderAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveAtLeastOnePassageWay(_smallGrid);
        }
    }
}
