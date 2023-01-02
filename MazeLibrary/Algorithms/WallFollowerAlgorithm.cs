using MazeLibrary.Events;
using MazeLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeLibrary.Algorithms
{
    public class WallFollowerAlgorithm : AlgorithmBase
    {
        public WallFollowerAlgorithm(int delay)
        {
            _delay = delay;
        }

        public void SolveLeft(ref Grid grid, out List<Cell> activeCells, out List<Cell> visitedCells)
        {
            activeCells = new List<Cell>();
            visitedCells = new List<Cell>();

            Cell startCell = grid.GetCell(0, 0);
            Cell endCell = grid.GetCell(grid.Width - 1, grid.Height - 1);

            Cell currentCell = startCell;
            activeCells.Add(currentCell);
            visitedCells.Add(currentCell);

            //Must assume that enterance  from the north
            Direction cameFrom = Direction.North;

            while (currentCell != endCell)
            {
                Direction lefty = FindLeft(grid, currentCell, cameFrom);

                Point offset = DirectionHelper.GetOffset(lefty);
                Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                Cell neighbourCell = grid.GetCell(point);

                currentCell = neighbourCell;

                if (!activeCells.Contains(currentCell))
                {
                    activeCells.Add(currentCell);
                }
                else
                {
                    //evilCells.Add(currentCell);
                    activeCells.RemoveAt(activeCells.Count - 1);

                }


                if (!visitedCells.Contains(currentCell))
                {
                    visitedCells.Add(currentCell);
                    OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, visitedCells));
                }


                OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, activeCells));

                if (_delay > 0)
                {
                    Thread.Sleep(_delay);
                }

                cameFrom = DirectionHelper.GetOpposite(lefty);
            }

        }

        private static Direction FindLeft(Grid grid, Cell currentCell, Direction direction)
        {


            CellEdge testEdge;
            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                if (direction == Direction.North)
                {
                    direction = Direction.East;
                    testEdge = currentCell.GetEdge(direction);
                    if (testEdge is CellPassage)
                    {
                        break;
                    }
                }
                if (direction == Direction.East)
                {
                    direction = Direction.South;
                    testEdge = currentCell.GetEdge(direction);
                    if (testEdge is CellPassage)
                    {
                        break;
                    }
                }
                if (direction == Direction.South)
                {
                    direction = Direction.West;
                    testEdge = currentCell.GetEdge(direction);
                    if (testEdge is CellPassage)
                    {
                        break;
                    }
                }
                if (direction == Direction.West)
                {
                    direction = Direction.North;
                    testEdge = currentCell.GetEdge(direction);
                    if (testEdge is CellPassage)
                    {
                        break;
                    }
                }
            }

            return direction;

            //Direction is now first left!

        }



        public void SolveRight(ref Grid grid, out List<Cell> activeCells, out List<Cell> visitedCells)
        { 
            activeCells = new List<Cell>();
          visitedCells = new List<Cell>();
          

            Cell startCell = grid.GetCell(0, 0);
            Cell endCell = grid.GetCell(grid.Width - 1, grid.Height - 1);

            Cell currentCell = startCell;
            activeCells.Add(currentCell);
            visitedCells.Add(currentCell);

            //Must assume that enterance  from the north
            Direction cameFrom = Direction.North;

            while (currentCell != endCell)
            {
                Direction righty = FindRight(grid, currentCell, cameFrom);

                Point offset = DirectionHelper.GetOffset(righty);
                Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                Cell neighbourCell = grid.GetCell(point);


                currentCell = neighbourCell;

                if (!activeCells.Contains(currentCell))
                {
                    activeCells.Add(currentCell);
                }
                else
                {
                    //evilCells.Add(currentCell);
                    activeCells.RemoveAt(activeCells.Count - 1);

                }

                if (!visitedCells.Contains(currentCell))
                {
                    visitedCells.Add(currentCell);
                    OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, visitedCells));
                }

                OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, activeCells));

                if (_delay > 0)
                {
                    Thread.Sleep(_delay);
                }

                cameFrom = DirectionHelper.GetOpposite(righty);
            }


        }


        private static Direction FindRight(Grid grid, Cell currentCell, Direction direction)
        {

            CellEdge testEdge;
            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                if (direction == Direction.North)
                {
                    direction = Direction.West;
                    testEdge = currentCell.GetEdge(direction);
                    if (testEdge is CellPassage)
                    {
                        break;
                    }
                }
                if (direction == Direction.West)
                {
                    direction = Direction.South;
                    testEdge = currentCell.GetEdge(direction);
                    if (testEdge is CellPassage)
                    {
                        break;
                    }
                }
                if (direction == Direction.South)
                {
                    direction = Direction.East;
                    testEdge = currentCell.GetEdge(direction);
                    if (testEdge is CellPassage)
                    {
                        break;
                    }
                }
                if (direction == Direction.East)
                {
                    direction = Direction.North;
                    testEdge = currentCell.GetEdge(direction);
                    if (testEdge is CellPassage)
                    {
                        break;
                    }
                }
            }

            return direction;

            //Direction is now first left!

        }
    }
}
