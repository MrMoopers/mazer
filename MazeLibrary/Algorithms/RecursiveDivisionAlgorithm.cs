using MazeLibrary.Events;
using MazeLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeLibrary.Algorithms
{
    //http://www.jamisbuck.org/presentations/rubyconf2011/index.html#recursive-division-demo
    public class RecursiveDivisionAlgorithm : AlgorithmBase
    {
        private static Random _random = new Random();

        public RecursiveDivisionAlgorithm(int delay)
        {
            _delay = delay;
        }

        public void Generate(Grid grid)
        {
            CreateAllCells(grid);
            DrawBorderWalls(grid);

            //Recursive procedure splitting the grid into a new portion over and over until it is finished
            DoSplit(grid, 0, grid.Width, 0, grid.Height);

            //Due to the algorithm having some complications, which will be adressed in the evaluation, 
            //some edges arn't set they can however all be set to the same edge type in this function
            FillRemaindingEdges(grid);
        }

        private void CreateAllCells(Grid grid)
        {
            //Does as the name suggests
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    grid.CreateCell(x, y);
                }
            }
        }

        private void DrawBorderWalls(Grid grid)
        {
            int width = grid.Width;
            int height = grid.Height;

            //procedures filling in all border cell's border edges with walls, thus filling the entire row or column at once
            CreateWalls(grid, 0, width, 0, Direction.East);
            CreateWalls(grid, 0, width, height, Direction.East);
            CreateWalls(grid, 0, height, 0, Direction.South);
            CreateWalls(grid, 0, height, width, Direction.South);
        }

        private void DoSplit(Grid grid, int x0, int x1, int y0, int y1)
        {
            int width = x1 - x0;
            int height = y1 - y0;

            if (width >= height)
            {
                //The space is only recursively divided if there is room, i.e. more than one cell's distance
                if (x1 - x0 > 1)
                {
                    //Width is greater than or equal to the height - Divide Vertically

                    int cut = _random.Next(x0 + 1, x1);

                    //forms all the walls vertically, (in terms of a grid makes walls east or west but
                    //we perceive them as being vertical in the ui)
                    CreateWalls(grid, y0, y1, cut, Direction.South);
                    if (cut > 0 & cut < grid.Width)
                    {
                        //Must create one way through the wall which will be this point:
                        CreatePassage(grid, cut, _random.Next(y0, y1), Direction.West);
                    }

                    if (_delay > 0)
                    {
                        Thread.Sleep(_delay);
                    }

                    if (cut - x0 > 1)
                    {
                        //Divide region left of the cut as there is room on this side
                        DoSplit(grid, x0, cut, y0, y1);
                    }

                    if (x1 - cut > 1)
                    {
                        //Divide region right of the cut as there is room on this side
                        DoSplit(grid, cut, x1, y0, y1);
                    }
                }
            }
            else
            {
                //The space is only recursively divided if there is room, i.e. more than one cell's distance
                if (y1 - y0 > 1)
                {
                    //Height is greater than or equal to the width - Divide horizontally

                    int cut = _random.Next(y0 + 1, y1);

                    //forms all the walls vertically, (in terms of a grid makes walls east or west but
                    //we perceive them as being vertical in the ui)

                    CreateWalls(grid, x0, x1, cut, Direction.East);
                    if (cut > 0 & cut < grid.Height)
                    {
                        //Must create one way through the wall which will be this point:
                        CreatePassage(grid, _random.Next(x0, x1), cut, Direction.North);
                    }

                    if (_delay > 0)
                    {
                        Thread.Sleep(_delay);
                    }

                    if (cut - y0 > 1)
                    {
                        //Divide region above the cut as there is room on this side
                        DoSplit(grid, x0, x1, y0, cut);
                    }

                    if (y1 - cut > 1)
                    {
                        //Divide region below the cut as there is room on this side
                        DoSplit(grid, x0, x1, cut, y1);
                    }
                }
            }
        }

        private void FillRemaindingEdges(Grid grid)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    Cell currentCell = grid.GetCell(x, y);

                    //Checks every direction from currentCell
                    for (int i = 0; i < 4; i++)
                    {
                        Direction direction = (Direction)i;
                        CellEdge cellEdge = currentCell.GetEdge(direction);
                        if (cellEdge == null)
                        {
                            Point offset = DirectionHelper.GetOffset(direction);
                            Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                            if (grid.ContainsPoint(point))
                            {
                                Cell neighbourCell = grid.GetCell(point);

                                //As the cellEdge is not set and is on th grid it can be replaced with a passageway
                                grid.CreatePassage(currentCell, neighbourCell, direction);
                                OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, direction));
                            }
                        }
                    }
                }
            }
        }

        private void CreatePassage(Grid grid, int x, int y, Direction direction)
        {
            //cell equals one of the cells on the wall section just created
            Cell cell = grid.GetCell(x, y);
            Point offset = DirectionHelper.GetOffset(direction);
            Point point = new Point(cell.Point.X + offset.X, cell.Point.Y + offset.Y);
            if (grid.ContainsPoint(point))
            {
                Cell neighbour = grid.GetCell(point);
                if (neighbour != null)
                {
                    //Overrides one of the walls with a passageway, doublechecking that the neighbour is indeed existant
                    grid.CreatePassage(cell, neighbour, direction);
                    OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, cell, neighbour, direction));
                }
            }
        }

        private void CreateWalls(Grid grid, int start, int end, int offset, Direction direction)
        {
            Cell cell = null;
            Cell neighbour = null;
            //Steps through every cell in the perpendicular to the variable "direction"
            //filling in walls in the "direction" variable's direction
            for (int i = start; i < end; i++)
            {
                switch (direction)
                {
                    //As direction is either east or west cellEdges are being filled in with walls going up or down,
                    //thus making a straight vertical wall
                    case Direction.East:
                    case Direction.West:
                        if (offset >= 0 && offset < grid.Height)
                        {
                            cell = grid.GetCell(i, offset);
                        }

                        if (offset > 0 && offset <= grid.Height)
                        {
                            neighbour = grid.GetCell(i, offset - 1);
                        }

                        if (cell != null)
                        {
                            grid.CreateWall(cell, neighbour, Direction.North);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, cell, Direction.North));
                        }
                        else
                        {
                            grid.CreateWall(neighbour, cell, Direction.South);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, neighbour, Direction.South));
                        }

                        if (_delay > 0)
                        {
                            Thread.Sleep(_delay);
                        }
                        break;

                    //As direction is either north or south cellEdges are being filled in with walls going leaft or right,
                    //thus making a straight horizontal wall
                    case Direction.North:
                    case Direction.South:
                        if (offset >= 0 && offset < grid.Width)
                        {
                            cell = grid.GetCell(offset, i);
                        }

                        if (offset > 0 && offset <= grid.Width)
                        {
                            neighbour = grid.GetCell(offset - 1, i);
                        }

                        if (cell != null)
                        {
                            grid.CreateWall(cell, neighbour, Direction.West);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, cell, Direction.West));
                        }
                        else
                        {
                            grid.CreateWall(neighbour, cell, Direction.East);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, neighbour, Direction.East));
                        }

                        if (_delay > 0)
                        {
                            Thread.Sleep(_delay);
                        }
                        break;
                }
            }
        }

        public List<Cell> Solve(Grid _grid)
        {
            throw new NotImplementedException();
        }
    }
}

