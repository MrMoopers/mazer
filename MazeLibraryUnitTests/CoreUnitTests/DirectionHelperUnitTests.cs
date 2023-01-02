using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeLibrary;
using System.Drawing;
using MazeLibrary.Helpers;

namespace MazeLibraryUnitTests
{
    [TestClass]
    public class DirectionHelperUnitTests : AlgorithmTestBase
    {
        [TestMethod]
        public void DirectionHelperCountReturnsIntegerFour()
        {
            Assert.AreEqual(4, DirectionHelper.Count);
        }

        [TestMethod]
        public void DirectionHelperReturnsCorrectOffset()
        {
            //The startCell is at: (0, 0)
            Point offset;
            Point point;

            offset = DirectionHelper.GetOffset(Direction.North);
            point = new Point(offset.X, offset.Y);
            Assert.IsTrue(new Point(0, -1) == point);

            offset = DirectionHelper.GetOffset(Direction.East);
            point = new Point(offset.X, offset.Y);
            Assert.IsTrue(new Point(1, 0) == point);

            offset = DirectionHelper.GetOffset(Direction.South);
            point = new Point(offset.X, offset.Y);
            Assert.IsTrue(new Point(0, 1) == point);

            offset = DirectionHelper.GetOffset(Direction.West);
            point = new Point(offset.X, offset.Y);
            Assert.IsTrue(new Point(-1, 0) == point);
        }

        [TestMethod]
        public void DirectionHelperReturnsOppositeDirection()
        {
            Assert.IsTrue(Direction.South == DirectionHelper.GetOpposite(Direction.North));
            Assert.IsTrue(Direction.North == DirectionHelper.GetOpposite(Direction.South));
            Assert.IsTrue(Direction.East == DirectionHelper.GetOpposite(Direction.West));
            Assert.IsTrue(Direction.West == DirectionHelper.GetOpposite(Direction.East));
        }
    }
}
