using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary.Algorithms;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class HuntAndKillAlgorithmUnitTest : AlgorithmTestBase
    {
        private HuntAndKillAlgorithm _huntAndKillAlgorithm = new HuntAndKillAlgorithm(0);

        [TestMethod]
        public void GridHasCorrectNumberOfCells()
        {
            _huntAndKillAlgorithm.Generate(_smallGrid);
            Assert.AreEqual(30, _smallGrid.Width * _smallGrid.Height);
        }

        [TestMethod]
        public void AllCellsExist()
        {
            _huntAndKillAlgorithm.Generate(_smallGrid);
            CheckAllCellsExist(_smallGrid);
        }

        [TestMethod]
        public void AllEdgesAreFullyIntialized()
        {
            _huntAndKillAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveFullyInitializedEdges(_smallGrid);
        }

        [TestMethod]
        public void AllBordersCreated()
        {
            _huntAndKillAlgorithm.Generate(_smallGrid);
            CheckAllBordersExist(_smallGrid);
        }

        [TestMethod]
        public void AllCellsHaveAtLeastOnePassageWay()
        {
            _huntAndKillAlgorithm.Generate(_smallGrid);
            CheckAllCellsHaveAtLeastOnePassageWay(_smallGrid);
        }
    }
}
