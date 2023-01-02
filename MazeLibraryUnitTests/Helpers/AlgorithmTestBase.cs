using MazeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using MazeLibraryUnitTests.Helpers;
using MazeLibrary.Helpers;

namespace MazeLibraryUnitTests
{
    public class AlgorithmTestBase
    {
        protected Grid _tinyGrid = new Grid("tinyGrid", 1, 1);
        protected Grid _smallGrid = new Grid("smallGrid", 5, 6);
        protected Grid _mediumGrid = new Grid("mediumGrid", 11, 13);
        protected Grid _largeGrid = new Grid("largeGrid", 31, 54);
        protected Grid _veryLargeGrid = new Grid("veryLargeGrid", 102, 74);
        protected Grid _extremelyLargeGrid = new Grid("extremelyLargeGrid", 210, 190);

        public void CheckAllInRouteAreSameGrid1(List<Cell> route)
        {
            List<Point> routePoints = new List<Point>();
            foreach (Cell c in route)
            {
                routePoints.Add(new Point(c.Point.X, c.Point.Y));
            }

            List<Point> actualRoutePoints = GridFactory.ActualRouteGrid1();

            for (int i = 0; i < route.Count - 1; i++)
            {
                Assert.IsTrue(actualRoutePoints[i] == routePoints[i]);
            }
        }

        public void CheckAllInRouteAreSameGrid2(List<Cell> route)
        {
            List<Point> routePoints = new List<Point>();
            foreach (Cell c in route)
            {
                routePoints.Add(new Point(c.Point.X, c.Point.Y));
            }

            List<Point> actualRoutePoints = GridFactory.ActualRouteGrid2();

            for (int i = 0; i < route.Count - 1; i++)
            {
                Assert.IsTrue(actualRoutePoints[i] == routePoints[i]);
            }
        }


        public void CheckAllCellsHaveAtLeastOnePassageWay(Grid g)
        {
            for (int y = 0; y < g.Height; y++)
            {
                for (int x = 0; x < g.Width; x++)
                {
                    Cell cell = g.GetCell(x, y);
                    int count = 0;
                    for (int i = 0; i < DirectionHelper.Count; i++)
                    {
                        CellEdge cellEdge = cell.GetEdge((Direction)i);
                        if (cellEdge is CellPassage) count++;
                    }
                    Assert.IsTrue(count >= 1);
                }
            }
        }

        public void CheckAllCellsExist(Grid g)
        {
            for (int y = 0; y < g.Height; y++)
            {
                for (int x = 0; x < g.Width; x++)
                {
                    Assert.IsNotNull(g.GetCell(x, y));
                }
            }
        }

        public void CheckAllCellsHaveFullyInitializedEdges(Grid g)
        {
            for (int y = 0; y < g.Height; y++)
            {
                for (int x = 0; x < g.Width; x++)
                {
                    Cell cell = g.GetCell(x, y);
                    bool b = cell.IsFullyInitialized;
                    Assert.IsTrue(cell.IsFullyInitialized == true);
                }
            }
        }

        public void CheckAllBordersExist(Grid g)
        {
            for (int y = 0; y < g.Height; y++)
            {
                for (int x = 0; x < g.Width; x++)
                {
                    Cell currentCell = g.GetCell(x, y);
                    if (y == 0)
                    {
                        CellEdge ce = g.GetCell(x, y).GetEdge(Direction.North);
                        Assert.IsTrue(g.GetCell(x, y).GetEdge(Direction.North) is CellWall);
                    }

                    if (y == g.Height - 1)
                    {
                        Assert.IsTrue(g.GetCell(x, y).GetEdge(Direction.South) is CellWall);
                    }

                    if (x == 0)
                    {
                        Assert.IsTrue(g.GetCell(x, y).GetEdge(Direction.West) is CellWall);
                    }

                    if (x == g.Width - 1)
                    {
                        Assert.IsTrue(g.GetCell(x, y).GetEdge(Direction.East) is CellWall);
                    }
                }
            }
        }
    }
}
