using MazeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MazeLibraryUnitTests.Helpers
{
    public static class GridFactory
    {

        public static Grid GetGrid1()
        {
            string filePath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data Files\Test Maze 17X13 (RecursiveBacktracker).maz"));
            Grid grid = MazeLibrary.Grid.GetGridFromFile(filePath);
            return grid;
        }

        public static List<Point> ActualRouteGrid1()
        {
            List<Point> actualRoute = new List<Point>(new Point[]
            {
                new Point(0,0), new Point(1,0), new Point(2,0), new Point(2,1), new Point(1,1), new Point(0,1), new Point(0,2), new Point(1,2), new Point(1,3),
                new Point(2,3), new Point(2,4), new Point(3,4), new Point(4,4), new Point(4,5), new Point(4,6), new Point(5,6), new Point(6,6), new Point(7,6),
                new Point(7,7), new Point(7,8), new Point(6,8), new Point(6,7), new Point(5,7), new Point(4,7), new Point(3,7), new Point(3,8), new Point(2,8), 
                new Point(1,8), new Point(1,7), new Point(0,7), new Point(0,8), new Point(0,9), new Point(0,10), new Point(0,11), new Point(0,12), new Point(1,12),
                new Point(1,11), new Point(2,11), new Point(3,11), new Point(3,12), new Point(4,12), new Point(5,12), new Point(6,12), new Point(7,12), new Point(8,12),
                new Point(9,12), new Point(9,11), new Point(10,11), new Point(11,11), new Point(12,11), new Point(12,10), new Point(12,9), new Point(13,9),
                new Point(13,10), new Point(14,10), new Point(14,11), new Point(15,11), new Point(15,10), new Point(16,10), new Point(16,11), new Point(16,12)  
            });

            return actualRoute;
        }

        public static Grid GetGrid2()
        {
            string filePath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data Files\Test Maze 25X20 (RecursiveDivision).maz"));
            Grid grid = MazeLibrary.Grid.GetGridFromFile(filePath);
            return grid;
        }

        public static List<Point> ActualRouteGrid2()
        {
            List<Point> actualRoute = new List<Point>(new Point[]
            {
               new Point(0,0), new Point(0,1), new Point(0,2), new Point(0,3), new Point(1,3), new Point(1,2), new Point(2,2), new Point(3,2), new Point(3,3),
               new Point(4,3), new Point(4,2), new Point(5,2), new Point(6,2), new Point(6,3), new Point(6,4), new Point(7,4), new Point(7,5), new Point(8,5), 
               new Point(8,6), new Point(8,7), new Point(9,7), new Point(9,8), new Point(10,8), new Point(11,8), new Point(11,7), new Point(12,7), new Point(13,7),
               new Point(13,6), new Point(13,5), new Point(13,4), new Point(14,4), new Point(14,5), new Point(15,5), new Point(15,4), new Point(16,4), new Point(17,4),
               new Point(17,3), new Point(18,3), new Point(19,3), new Point(19,4), new Point(20,4), new Point(20,3), new Point(21,3), new Point(21,4), new Point(21,5),
               new Point(20,5), new Point(20,6), new Point(19,6), new Point(19,7), new Point(20,7), new Point(21,7), new Point(21,8), new Point(22,8), new Point(22,7),
               new Point(23,7), new Point(23,8), new Point(24,8), new Point(24,9), new Point(23,9), new Point(23,10), new Point(24,10), new Point(24,11),
               new Point(24,12), new Point(24,13), new Point(24,14), new Point(24,15), new Point(23,15), new Point(23,16), new Point(24,16), new Point(24,17), 
               new Point(24,18), new Point(24,19), 
            });

            return actualRoute;
        }
    }
}
