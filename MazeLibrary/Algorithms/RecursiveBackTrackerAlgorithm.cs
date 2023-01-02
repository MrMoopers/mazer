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
    public class RecursiveBackTrackerAlgorithm : AlgorithmBase
    {
        //http://www.jamisbuck.org/presentations/rubyconf2011/index.html#recursive-backtracker-demo

        public RecursiveBackTrackerAlgorithm(int delay)
        {
            _delay = delay;
        }

        public void Generate(Grid grid)
        {
            //The list activeCells functions as a stack in the running of this algorithm
            List<Cell> activeCells = new List<Cell>();
            Cell cell = grid.CreateCell(grid.RandomPoint());
            activeCells.Add(cell);
            //Debug.WriteLine(String.Format("First == ({0},{1}) ", cell.Point.X, cell.Point.Y));

            while (activeCells.Count > 0)
            {
                OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, activeCells));

                //currentCell is now equal to the latest value in the list 
                Cell currentCell = activeCells[activeCells.Count - 1];
                //Debug.Write(String.Format("Cell ({0},{1}): ", currentCell.Point.X, currentCell.Point.Y));

                if (currentCell.IsFullyInitialized)
                {
                    //Removes the currentCell if it is fully intialised, or more simply is at a dead end, etc.
                    activeCells.RemoveAt(activeCells.Count - 1);
                }
                else
                {
                    //If currentCell is not fully intialised then create an edge between the currentCell
                    //and one of it's neighbours
                    Direction direction = currentCell.GetRandomUninitializedDirection();
                    Point offset = DirectionHelper.GetOffset(direction);
                    Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);

                    if (grid.ContainsPoint(point))
                    {
                        //If neighbourCell doesn't exist make the cell and a passage otherwise make a wall 
                        Cell neighbourCell = grid.GetCell(point);
                        if (neighbourCell == null)
                        {
                            neighbourCell = grid.CreateCell(point);
                            activeCells.Add(neighbourCell);
                            //Debug.WriteLine(String.Format("Adding ({0},{1})", point.X, point.Y));
                            grid.CreatePassage(currentCell, neighbourCell, direction);
                            OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, direction));
                        }
                        else
                        {
                            //Debug.WriteLine(String.Format("Neighbour Exists ({0},{1}) ", point.X, point.Y));
                            grid.CreateWall(currentCell, neighbourCell, direction);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                        }
                    }
                    else
                    {
                        //Create the border wall
                        //Debug.WriteLine(String.Format("Off Grid ({0},{1}) ", point.X, point.Y));
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

        static bool deadendDetected;
        public void Solve(ref Grid grid, out List<Cell> activeCells, out List<Cell> visitedCells)
        {
            Cell startCell = grid.GetCell(0, 0);
            Cell endCell = grid.GetCell(grid.Width - 1, grid.Height - 1);
            Cell currentCell = startCell;

            //activeCells will hold the final path once completed and inactiveCells will hold all cells the program 
            //will not have to recheck
            activeCells = new List<Cell>();
            List<Cell> inactiveCells = new List<Cell>();
            visitedCells = new List<Cell>();
            activeCells.Add(currentCell);
            visitedCells.Add(currentCell);
            OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, activeCells));

            while (currentCell != endCell)
            {
                deadendDetected = false;



                //Function returning a direction from currenctCell which is on a null edge. Also this function checks
                //if the currentCells is now completed
                Direction direction = GetRandomPassage(grid, currentCell, inactiveCells);

                Point offset = DirectionHelper.GetOffset(direction);
                Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                Cell nextCell = grid.GetCell(point);
                if (!visitedCells.Contains(nextCell))
                {
                    visitedCells.Add(nextCell);
                    OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, visitedCells));
                }



                if (!activeCells.Contains(nextCell))
                {
                    //Adds the nextCell if it isn't already in the list
                    activeCells.Add(nextCell);
                    OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, activeCells));

                    if (_delay > 0)
                    {
                        Thread.Sleep(_delay);
                    }
                }
                else if (deadendDetected == true)
                {
                    //Stuck is a boolean for telling this part of code if the cell is stuck 
                    //in a deadend, if so it must add that cell to inactiveCells so it never tries to go 
                    //there again and remove it from activeCells as it doesn't lead to the endCell
                    inactiveCells.Add(activeCells[activeCells.Count - 1]);
                    activeCells.RemoveAt(activeCells.Count - 1);

                    OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, activeCells));
                    //Next it sets nextCell to the latest cell in activeCells, making it step backwards
                    nextCell = activeCells[activeCells.Count - 1];

                    if (_delay > 0)
                    {
                        Thread.Sleep(_delay);
                    }

                    //All in all this if statement follows the procedure of popping an item off the activeCell's stack
                    //adding it to the inactiveCell's stack, and then moves to the last cell
                }
                else
                {
                    //If activeCells does contain nextCell but it isn't stuck make nextCell equal the latest cell in activeCells.
                    //This makes sure the program does every path from a crossroads before backtracking, otherwise it could skip
                    //over a path
                    nextCell = activeCells[activeCells.Count - 1];
                }

                currentCell = nextCell;
            }


        }

        private Direction GetRandomPassage(Grid grid, Cell currentCell, List<Cell> inactiveCells)
        {
            Random random = new Random();
            List<Direction> directionList = new List<Direction>();

            //Collects all directions from currentCell...
            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                //...that are passageway edges and...
                if (currentCell.GetEdge((Direction)i) is CellPassage)
                {
                    Point offset = DirectionHelper.GetOffset((Direction)i);
                    Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                    Cell testCell = grid.GetCell(point);

                    //...arn't part of the cells it doesn't need to recheck, inactiveCells
                    if (!inactiveCells.Contains(testCell))
                    {
                        directionList.Add((Direction)i);
                    }
                }
            }

            if (directionList.Count == 1)
            {
                //The only direction it can go is where it came from as this is the only passageway, therefore it is at a deadend
                deadendDetected = true;
            }

            //Returns a random direction from the list
            int index = random.Next(0, directionList.Count);
            return directionList[index];
        }


       
    }
}
