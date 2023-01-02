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
    public  class SimpleRectangleAlgorithm : AlgorithmBase
    {
        //Algorithm has no source and will not generate a maze. This algorithm is only included
        //as it was the starting point to testing if maze generation would be possible with the approach here:
        public SimpleRectangleAlgorithm(int delay) 
        {
            _delay = delay;
        }

        public  void Generate(Grid grid)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    //Creates a cell at every point on the grid, after all the passes through the loop
                    Cell currentCell = grid.CreateCell(x, y);
                    OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, new List<Cell>(new Cell[] { currentCell })));

                    //An array is used to store all the points around the currentCell, ergo its neighbours
                    Point[] possiblePoints = new Point[] { new Point(currentCell.Point.X, currentCell.Point.Y - 1), //North
                        new Point(currentCell.Point.X + 1, currentCell.Point.Y),                                    //East
                        new Point(currentCell.Point.X, currentCell.Point.Y + 1),                                    //South 
                        new Point(currentCell.Point.X - 1, currentCell.Point.Y)};                                   //West

                    //Fills any border cell's edges, which lead off the grid, with walls
                    for (int i = 0; i < DirectionHelper.Count; i++)
                    {
                        if (grid.ContainsPoint(possiblePoints[i]) == false)
                        {
                            grid.CreateWall(currentCell, null, (Direction)i);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, (Direction)i));

                            if (_delay > 0)
                            {
                                Thread.Sleep(_delay);
                            }
                        }
                    }

                    //Only checks points north and east of the currentCell. It fills all edges in the grid, bar border edges, with passages
                    if (grid.ContainsPoint(possiblePoints[0]))
                    {
                        grid.CreatePassage(currentCell, grid.GetCell(possiblePoints[0]), Direction.North);
                        OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, grid.GetCell(possiblePoints[0]), Direction.North));
                    }
                    if (grid.ContainsPoint(possiblePoints[3]))
                    {
                        grid.CreatePassage(currentCell, grid.GetCell(possiblePoints[3]), Direction.West);
                        OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, grid.GetCell(possiblePoints[3]), Direction.West));
                    }

                    //By setting edges to both walls and borders I have tested that they both work as intended

                    if (_delay > 0)
                    {
                        Thread.Sleep(_delay);
                    }
                    
                }
            }
        }

        public  List<Cell> Solve(Grid _grid)
        {
            throw new NotImplementedException();
        }
    }
}
