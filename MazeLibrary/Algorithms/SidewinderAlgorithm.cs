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
    public class SidewinderAlgorithm : AlgorithmBase
    {
        public SidewinderAlgorithm(int delay)
        {
            _delay = delay;
        }

        private Random _random = new Random();

        public void Generate(Grid grid)
        {
            PrepareGridCells(grid);

            Point neighbourPoint;
            //activeGroup is a list of cells in the current group it will be adding to
            List<Cell> activeGroup = new List<Cell>();

            for (int y = 0; y < grid.Height; y++)
            {
                //Removes all cells from the current group
                activeGroup.Clear();

                for (int x = 0; x < grid.Width; x++)
                {
                    Cell currentCell = grid.GetCell(x, y);
                    activeGroup.Add(currentCell);

                    OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, activeGroup));

                    //Random boolean decides if the next run should be a new group or a continuation of the current group
                    bool endCurrentRun = _random.Next(0, 2) == 0;
                    if (y > 0 && (x == grid.Width - 1 || endCurrentRun))
                    {
                        //Ends the current run and carves north
                        //Choose a random cell from the activeGroup and carve north from it
                        //activeGroup needs to be restarted

                        //Puts a wall east
                        neighbourPoint = new Point(currentCell.Point.X + 1, currentCell.Point.Y);
                        if (grid.ContainsPoint(neighbourPoint))
                        {
                            Cell neighbourCell = grid.GetCell(neighbourPoint);
                            grid.CreateWall(currentCell, neighbourCell, Direction.East);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, Direction.East));

                            if (_delay > 0)
                            {
                                Thread.Sleep(_delay);
                            }
                        }

                        int r = _random.Next(0, activeGroup.Count);
                        for (int i = 0; i < activeGroup.Count; i++)
                        {
                            Cell groupCell = activeGroup[i];

                            neighbourPoint = new Point(groupCell.Point.X, groupCell.Point.Y - 1);
                            Cell neighbourCell = grid.GetCell(neighbourPoint);

                            //Only if i is equal to the one place for a passage does it make it, so no loops can be made
                            if (i == r)
                            {
                                //Make a passage North
                                grid.CreatePassage(groupCell, neighbourCell, Direction.North);
                                OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, Direction.North));
                            }
                            else
                            {
                                //Make a wall North
                                grid.CreateWall(groupCell, neighbourCell, Direction.North);
                                OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, Direction.North));
                            }

                            if (_delay > 0)
                            {
                                Thread.Sleep(_delay);
                            }
                        }

                        activeGroup.Clear();
                    }
                    else
                    {
                        //Carve East
                        //Add the cell to the activeGroup
                        //Carve east from the previous cell
                        neighbourPoint = new Point(x + 1, y);
                        if (grid.ContainsPoint(neighbourPoint))
                        {
                            Cell neighbourCell = grid.GetCell(neighbourPoint);
                            grid.CreatePassage(currentCell, neighbourCell, Direction.East);
                            OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, Direction.East));
                        }
                        else
                        {
                            grid.CreateWall(currentCell, null, Direction.East);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, Direction.East));
                        }
                        //activeGroup.Add(currentCell);

                        if (_delay > 0)
                        {
                            Thread.Sleep(_delay);
                        }
                    }
                }
            }
        }

        private void PrepareGridCells(Grid grid)
        {
            //Creates every cell in the grid and also sets all the border edges to be walls
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    Cell currentCell = grid.CreateCell(x, y);

                    if (y == 0)
                    {
                        //Left border
                        grid.CreateWall(currentCell, null, Direction.North);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, Direction.North));

                        if (_delay > 0)
                        {
                            Thread.Sleep(_delay);
                        }
                    }

                    if (y == grid.Height - 1)
                    {
                        //Right border
                        grid.CreateWall(currentCell, null, Direction.South);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, Direction.South));

                        if (_delay > 0)
                        {
                            Thread.Sleep(_delay);
                        }
                    }

                    if (x == 0)
                    {
                        //Top border
                        grid.CreateWall(currentCell, null, Direction.West);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, Direction.West));

                        if (_delay > 0)
                        {
                            Thread.Sleep(_delay);
                        }
                    }

                    if (x == grid.Width - 1)
                    {
                        //Bottom border
                        grid.CreateWall(currentCell, null, Direction.East);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, Direction.East));

                        if (_delay > 0)
                        {
                            Thread.Sleep(_delay);
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