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
    public class HuntAndKillAlgorithm : AlgorithmBase
    {
        //http://www.jamisbuck.org/presentations/rubyconf2011/index.html#hunt-and-kill-demo

        public HuntAndKillAlgorithm(int delay)
        {
            _delay = delay;
        }

        ////Global 'stuck' variable used to tell the main function if the algorithm has reached a dead end
        //static bool stuck;
        public void Generate(Grid grid)
        {
            Cell currentCell = grid.CreateCell(grid.RandomPoint());
            List<Cell> tempListCells = new List<Cell>();

            while (currentCell != null)
            {
                //OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, new List<Cell>(new Cell[] { currentCell })));



                if (!tempListCells.Contains(currentCell))
                {
                    tempListCells.Add(currentCell);
                    OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, tempListCells));
                }


                //If currentCell is fully intialised i.e has all its edges as non null values then the algorithm must find a new cell to process next.
                //Otherwise if it isn't it can process this one
                if (!currentCell.IsFullyInitialized)
                {
                    //In order to ensure no edge with a value is being replace with a new value, direction is given a null value from currentCell
                    Direction direction = currentCell.GetRandomUninitializedDirection();
                    Point offset = DirectionHelper.GetOffset(direction);
                    Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);

                    if (grid.ContainsPoint(point))
                    {
                        Cell neighbourCell = grid.GetCell(point);

                        //As the last if statement filtered the possiblity of the neighbour being off the grid, it is safe to now ask if that cell is null
                        if (neighbourCell == null)
                        {
                            //Creates the edge between the old cell and the new neighbour cell
                            neighbourCell = grid.CreateCell(point);
                            grid.CreatePassage(currentCell, neighbourCell, direction);
                            OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, direction));

                            //procedure to complete any other edges, if possible, for the currentCell
                            CompleteAnyWalls(currentCell, grid);

                            currentCell = neighbourCell;
                        }
                        else
                        {
                            //Neighbour is already made, so a wall must seporate them, or a loop may be made
                            grid.CreateWall(currentCell, neighbourCell, direction);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                        }
                    }
                    else
                    {
                        //Neighbour is off the grid
                        grid.CreateWall(currentCell, null, direction);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                    }
                }
                else
                {
                    //currentCell is done, such as if it has reached a deadend of 3 walls and one passageway
                    currentCell = Hunt(grid);
                    //Debug.WriteLine("New hunted at {0}", currentCell.Name);
                    //tempListCells.Clear();
                }

                if (_delay > 0)
                {
                    Thread.Sleep(_delay);
                }
            }
        }

        private Cell Hunt(Grid grid)
        {
            //Checks every cell, from the top-left to the bottom-right
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    Cell currentCell = grid.GetCell(x, y);

                    if (currentCell == null)
                    {
                        //Tests every direction from the null cell
                        for (int i = 0; i < DirectionHelper.Count; i++)
                        {
                            Direction direction = (Direction)i;
                            Point offset = DirectionHelper.GetOffset(direction);
                            Point point = new Point(x + offset.X, y + offset.Y);
                            if (grid.ContainsPoint(point))
                            {
                                Cell neighbourCell = grid.GetCell(point);
                                //The next if statement forces the program to only use currentCell if it's neighbour is part of the already made maze structure.
                                //After this, if it isn't null, the generation wilol continue from this cell next time round
                                if (neighbourCell != null)
                                {
                                    //Creates currentCell and 
                                    currentCell = grid.CreateCell(x, y);

                                    grid.CreatePassage(currentCell, neighbourCell, direction);
                                    OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, direction));
                                    if (_delay > 0)
                                    {
                                        Thread.Sleep(_delay);
                                    }

                                    CompleteAnyWalls(currentCell, grid);

                                    return currentCell;
                                }
                            }
                        }
                    }
                }
            }
            //There are no more non-null cells or all remaining cells are off the grid it returns null to say 'there are no more cells left to use'
            return null;
        }

        private void CompleteAnyWalls(Cell currentCell, Grid grid)
        {
            //Checks every egde from the currentCell
            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                Direction direction = (Direction)i;

                //Ensures the edge is not already set before it tries to replace it
                if (currentCell.GetEdge(direction) == null)
                {
                    Point offset = DirectionHelper.GetOffset(direction);
                    Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                    if (grid.ContainsPoint(point))
                    {
                        Cell neighbourCell = grid.GetCell(point);

                        //If that cell is on the grid and the cell isn't null, so is part of the maze structure a wall is created to stop any loops being made later
                        if (neighbourCell != null)
                        {
                            grid.CreateWall(currentCell, neighbourCell, direction);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                        }
                    }
                    else
                    {
                        //If the cell isn't on the grid a border wall must be made
                        grid.CreateWall(currentCell, null, direction);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                    }
                }
                if (_delay > 0)
                {
                    Thread.Sleep(_delay);
                }
            }
        }

        public List<Cell> Solve(Grid grid)
        {
            throw new NotImplementedException();
            //           Cell startCell = grid.GetCell(0, 0);
            //           Cell endCell = grid.GetCell(grid.Width - 1, grid.Height - 1);

            //           Cell currentCell = startCell;

            //           List<Cell> activeCells = new List<Cell>();
            //           activeCells.Add(currentCell);
            //           List<Cell> evilCells = new List<Cell>();

            //           while (currentCell != endCell)
            //           {
            //stuck = false;

            //               Direction direction = GetRandomPassage(grid, currentCell, evilCells);

            //               Point offset = DirectionHelper.GetOffset(direction);
            //               Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
            //               Cell nextCell = grid.GetCell(point);






            //               if (!activeCells.Contains(nextCell))
            //               {
            //                   activeCells.Add(nextCell);
            //                   currentCell = nextCell;
            //               }
            //               else
            //               {
            //                   if (stuck == true)
            //                   {
            //                       for (int y = 0; y < grid.Height - 1; y++)
            //                       {
            //                           for (int x = 0; x < grid.Width - 1; x++)
            //                           {
            //                               Cell cell = grid.GetCell(x, y);
            //                               if (!activeCells.Contains(cell) && !evilCells.Contains(cell))
            //                               {
            //                                   for (int i = 0; i < DirectionHelper.Count; i++)
            //                                   {
            //                                       direction = (Direction)i;
            //                                       offset = DirectionHelper.GetOffset(direction);
            //                                       point = new Point(x + offset.X, y + offset.Y);
            //                                       if (grid.ContainsPoint(point))
            //                                       {
            //                                           Cell neighbourCell = grid.GetCell(point);
            //                                           if (activeCells.Contains(neighbourCell))
            //                                           {
            //                                               int j = activeCells.Count - 1;
            //                                               while (activeCells[j] != neighbourCell)
            //                                               {
            //                                                   evilCells.Add(activeCells[j]);
            //                                                   activeCells.RemoveAt(j);
            //                                               }
            //                                               currentCell = cell;
            //                                               //start removing from active cells until neighbour cell reached
            //                                               //add removed to evil cells
            //                                               //currentCell = grid.getCell(x,y)
            //                                           }
            //                                       }
            //                                   }
            //                               }
            //                           }
            //                       }
            //                   }

            //               }


            //               foreach (Cell c in activeCells)
            //               {
            //                   Debug.Write(c.Name);
            //               }
            //               return activeCells;
            //           }
        }

        //private Direction GetRandomPassage(Grid grid, Cell currentCell, List<Cell> evilCells)
        //{
        //    Random random = new Random();

        //    List<Direction> directionList = new List<Direction>();
        //    for (int i = 0; i < DirectionHelper.Count; i++)
        //    {
        //        if (currentCell.GetEdge((Direction)i) is CellPassage)
        //        {
        //            Point offset = DirectionHelper.GetOffset((Direction)i);
        //            Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
        //            Cell testCell = grid.GetCell(point);

        //            if (!evilCells.Contains(testCell))
        //            {
        //                directionList.Add((Direction)i);
        //            }
        //        }
        //    }

        //    if (directionList.Count <= 1)
        //    {
        //        stuck = true;
        //    }
        //    else
        //    {
        //        stuck = false;
        //    }

        //    int index = random.Next(0, directionList.Count);
        //    return directionList[index];
        //}

    }
}
