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
    public class BinaryTreeAlgorithm : AlgorithmBase
    {
        //http://www.jamisbuck.org/presentations/rubyconf2011/index.html#binary-tree-demo

        public BinaryTreeAlgorithm(int delay)
        {
            _delay = delay;
        }

        public void Generate(Grid grid)
        {
            Random random = new Random();

            //two definate iteration loops for addressing every cell in the grid.
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    Cell currentCell = grid.CreateCell(x, y);

                    OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, new List<Cell>(new Cell[] { currentCell })));

                    //possiblePoints is an array storing every point adjecent to the currentCell
                    Point[] possiblePoints = new Point[] { new Point(currentCell.Point.X, currentCell.Point.Y - 1), //North
                        new Point(currentCell.Point.X + 1, currentCell.Point.Y),                                    //East
                        new Point(currentCell.Point.X, currentCell.Point.Y + 1),                                    //South 
                        new Point(currentCell.Point.X - 1, currentCell.Point.Y)};                                   //West

                    //Creates walls from the current cell to a null value if the endpoint is not on the grid
                    for (int i = 0; i < DirectionHelper.Count; i++)
                    {
                        if (grid.ContainsPoint(possiblePoints[i]) == false)
                        {
                            grid.CreateWall(currentCell, null, (Direction)i);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, (Direction)i));
                        }
                    }

                    //A random number to decide if:
                    //Case 0 (randSelected = 0); will create a passage north and a wall west, thus ending the current hallway 
                    //Case 1 (randSelected = 1); will create a wall north and a passage west, thus continueing the current hallway
                    int randSelected = random.Next(0, 2);

                    if (randSelected == 0 && grid.ContainsPoint(possiblePoints[0]) && grid.ContainsPoint(possiblePoints[3]))
                    {
                        grid.CreatePassage(currentCell, grid.GetCell(possiblePoints[0]), Direction.North);
                        OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, grid.GetCell(possiblePoints[0]), Direction.North));
                        grid.CreateWall(currentCell, grid.GetCell(possiblePoints[3]), Direction.West);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, Direction.West));
                    }
                    else if (randSelected == 1 && grid.ContainsPoint(possiblePoints[3]) && grid.ContainsPoint(possiblePoints[0]))
                    {
                        grid.CreatePassage(currentCell, grid.GetCell(possiblePoints[3]), Direction.West);
                        OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, grid.GetCell(possiblePoints[3]), Direction.West));
                        grid.CreateWall(currentCell, grid.GetCell(possiblePoints[0]), Direction.North);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, Direction.North));
                    }

                    if (_delay > 0)
                    {
                        Thread.Sleep(_delay);
                    }
                }
            }
            //Once every cell has been addressed as much as it can be, the remaining CellEdges must be filled otherwise the displaying algorithm in the Mazer>ViewModels
            //cannot display it
            FillRemainingEdges(grid);
        }

        private void FillRemainingEdges(Grid grid)
        {
            //another set of two definate iteration loops addressing every cell in the grid
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    Cell currentCell = grid.GetCell(x, y);

                    //addresses each CellEdge of the currentCell
                    for (int i = 0; i < 4; i++)
                    {
                        //Casts i as a Direction: direction
                        Direction direction = (Direction)i;
                        //Gets the edge corrasponding to that direction from the currentCell
                        CellEdge cellEdge = currentCell.GetEdge(direction);

                        //if this cellEdge is not a wall or a passage, i.e. is null it...
                        if (!(cellEdge is CellWall) && !(cellEdge is CellPassage))
                        {
                            Point offset = DirectionHelper.GetOffset(direction);
                            Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                            //...finds if the point in that direction is on the grid or not.
                            if (grid.ContainsPoint(point))
                            {
                                //if it is it creates a passage to it
                                Cell neighbourCell = grid.GetCell(point);

                                grid.CreatePassage(currentCell, neighbourCell, direction);
                                //OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, direction));
                            }
                            else
                            {
                                //if it isn't it creates a wall in its direction
                                grid.CreateWall(currentCell, null, direction);
                                //OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                            }
                        }
                    }
                }
            }
        }

        public List<Cell> Solve(Grid _grid)
        {
            throw new NotImplementedException();
        }
    }
}
