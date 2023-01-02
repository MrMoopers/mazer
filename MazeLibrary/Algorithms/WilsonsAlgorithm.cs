using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary.Algorithms
{
    public class WilsonsAlgorithm
    {
        static private Random _random = new Random();
        public static void Generate(Grid grid)
        {
            PrepareGridCells(grid);
            int cx = _random.Next(0, grid.Width);
            int cy = _random.Next(0, grid.Height);
            Cell currentCell = grid.GetCell(cx,cy);
        }

        private static void PrepareGridCells(Grid grid)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    Cell currentCell = grid.CreateCell(x, y);

                    if (y == 0)
                    {
                        grid.CreateWall(currentCell, null, Direction.North);
                    }

                    if (y == grid.Height - 1)
                    {
                        grid.CreateWall(currentCell, null, Direction.South);
                    }

                    if (x == 0)
                    {
                        grid.CreateWall(currentCell, null, Direction.West);
                    }

                    if (x == grid.Width - 1)
                    {
                        grid.CreateWall(currentCell, null, Direction.East);
                    }
                }
            }

        }



        public static List<Cell> Solve(Grid _grid)
        {
            throw new NotImplementedException();
        }
    }
}
