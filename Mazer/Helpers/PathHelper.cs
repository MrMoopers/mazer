using MazeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazer.Helpers
{
    public static class PathHelper
    {
        //M = "Move or translate", V = "Vertical translation", H = "Horizontal translation", L = "Line or both vertical and horizontal translations"
        public static string GetGuidePath(int gridWidth, int gridHeight, int wallSize)
        {
            //Creates the grid of dashed red lines over which walls and passageways are placed
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x <= gridWidth; x++)
            {
                //Vertical Line is started from the x position in the grid X-wallsize to get it to the correct place in terms of the rest of the grid
                sb.AppendFormat("M{0},{1}V{2}", x * wallSize, 0, gridHeight * wallSize);
            }

            for (int y = 0; y <= gridHeight; y++)
            {
                //Horizontal Line is started from the y position in the grid Y-wallsize to get it to the correct place in terms of the rest of the grid
                sb.AppendFormat("M{0},{1}H{2}", 0, y * wallSize, gridWidth * wallSize);
            }
            return sb.ToString();
        }

        internal static string GetMazeGeneratorPath(MazeLibrary.Grid grid, int wallSize)
        {
            //Starts forming the maze grid in string geometries where placement is decided on by x and y and distance to the next cell is wallsize, i.e. next point is px and py away
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    MazeLibrary.Cell cell = grid.GetCell(x, y);
                    int px = x * wallSize;
                    int py = y * wallSize;

                    //If statement required as a direction for a null cell, or a null edge, cannot be diplayed
                    if (cell != null)
                    {

                        //North Wall
                        CellEdge northWall = cell.GetEdge(Direction.North);
                        if (northWall is CellWall)
                        {
                            sb.AppendFormat("M{0},{1}H{2}", px, py, px + wallSize);
                        }

                        //East Wall
                        CellEdge eastWall = cell.GetEdge(Direction.East);
                        if (eastWall is CellWall)
                        {
                            sb.AppendFormat("M{0},{1}V{2}", px + wallSize, py, py + wallSize);
                        }

                        //South Wall
                        CellEdge southWall = cell.GetEdge(Direction.South);
                        if (southWall is CellWall)
                        {
                            sb.AppendFormat("M{0},{1}H{2}", px, py + wallSize, px + wallSize);
                        }

                        //West Wall
                        CellEdge westWall = cell.GetEdge(Direction.West);
                        if (westWall is CellWall)
                        {
                            sb.AppendFormat("M{0},{1}V{2}", px, py, py + wallSize);
                        }
                    }
                }
            }
            return sb.ToString();
        }

        internal static string GetMazeSolverPath(MazeLibrary.Grid grid, int wallSize, List<Cell> route)
        {
            //Converts the routePath into a string geometry
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < route.Count - 1; i++)
            {

                Cell currentCell = route[i];
                Cell neighbourCell = route[i + 1];

                //currentCell's position
                int currentX = (currentCell.Point.X * wallSize) + wallSize / 2;
                int currentY = (currentCell.Point.Y * wallSize) + wallSize / 2;

                //Distance to neighbourCells position
                int offsetX = (neighbourCell.Point.X - currentCell.Point.X) * wallSize;
                int offsetY = (neighbourCell.Point.Y - currentCell.Point.Y) * wallSize;

                //Addition of the two getting the next position
                int nextX = currentX + offsetX;
                int nextY = currentY + offsetY;

                //Creating this data in a geometry
                string statement = string.Format("M{0},{1}L{2},{3}", currentX, currentY, nextX, nextY);
                sb.AppendFormat(statement);

            }
            return sb.ToString();
        }


        internal static string GetHighlightCellPath(int wallSize, int wallThickness, List<Cell> highlightCells)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Cell cell in highlightCells)
            {
                int xOffset = cell.Point.X * wallSize;
                int yOffset = cell.Point.Y * wallSize;
                //sb.Append("M 100,100 V200 H200 V100 Z");
                sb.AppendFormat("M {0},{1} V{2} H{3} V{1} Z",
                    xOffset,
                    yOffset,
                    yOffset + wallSize,
                    xOffset + wallSize);
            }

            return sb.ToString();
        }
    }
}
