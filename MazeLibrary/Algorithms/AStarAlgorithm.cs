
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
    public class AStarAlgorithm : AlgorithmBase
    {
        //https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode

        public AStarAlgorithm(int delay)
        {
            //This constructor simply forces throughout the code a delay or pause for the chosen time from the user
            _delay = delay;
        }

        public void Solve(ref Grid grid, out List<Cell> activeCells, out List<Cell> closedSet, Point startPoint, Point endPoint)
        {
            {
                Cell startCell = grid.GetCell(startPoint);
                Cell endCell = grid.GetCell(endPoint);
                //Cell startCell = grid.GetCell(0, 0);
                //Cell endCell = grid.GetCell(grid.Width - 1, grid.Height - 1);

                //The set of cells already evaluated
               closedSet = new List<Cell>();

                //Set of currently discovered cells which are not evaluated yet.
                //Currently only the startCell is known
                List<Cell> openSet = new List<Cell>();
                openSet.Add(startCell);

                //For each cell, which cell it came from to get there, required to record the path to any cell in the closedSet
                Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();

                //To create a value for each cell in the grid, I set each cells cameFrom to be null
                for (int y = 0; y < grid.Height; y++)
                {
                    for (int x = 0; x < grid.Height; x++)
                    {
                        Cell cell = grid.GetCell(x, y);
                        cameFrom[cell] = null;
                    }
                }

                //For each cell the 'cost' of getting there from the startCell. Every cell starts with a default of infinity (largest number the integer can hold)
                Dictionary<Cell, int> gScore = new Dictionary<Cell, int>();
                for (int y = 0; y < grid.Height; y++)
                {
                    for (int x = 0; x < grid.Height; x++)
                    {
                        Cell cell = grid.GetCell(x, y);
                        gScore[cell] = Int32.MaxValue;
                    }
                }

                //Set the startCell's gScore to be zero as the cost to get from the start to itself is zero
                gScore[startCell] = 0;

                //For each cell the 'cost' of getting to it from the startCell added to the distance from it to the endCell.
                //Every cell starts with a default of infinity (largest number the integer can hold)
                Dictionary<Cell, int> fScore = new Dictionary<Cell, int>();
                for (int y = 0; y < grid.Height; y++)
                {
                    for (int x = 0; x < grid.Height; x++)
                    {
                        Cell cell = grid.GetCell(x, y);
                        fScore[cell] = Int32.MaxValue;
                    }
                }

                //The fScore for getting from the start to the end is calculated in the CalculateDistance function
                fScore[startCell] = CalculateDistance(startCell, endCell);

                //An indefinate loop used to run through every cell in openSet (as it increases and decreases), until there are no more cells in the grid to evaluate
                while (openSet.Count > 0)
                {
                    OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, closedSet));

                    //currentCell equals the cell in the currently unevaluated cells in openSet with the lowest distance from its self to the endCell.
                    Cell currentCell = GetCellWithLowestFScore(openSet, fScore);

                    //If true the algorithm has found the endCell. It must now reproduce the specific route that got to that endCell in the following function
                    if (currentCell == endCell)
                    {
                        activeCells = reconstructPath(grid, cameFrom, currentCell);
                        return;
                    }

                    //Because the algorithm is now evaluating the lowest fScore cell: currentCell, it must remove the cell from the openSet (unevaluated cells list) and add it to the closedSet (evaluated cells list)
                    openSet.Remove(currentCell);
                    closedSet.Add(currentCell);

                    //Function returning a list to the cells around the currentCell with a passageway between them
                    List<Cell> neighbours = GetTraversableNeighbours(grid, currentCell);

                    //Runs through each of these neighbour cells individually
                    foreach (Cell neighbour in neighbours)
                    {

                        //If this neighbour has already been evaluated; ignore it
                        if (closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        //A temporary gScore for this neighbour is recorded. This is calculated as the currentCell's gScore + 1
                        int tenativeGScore = gScore[currentCell] + 1;

                        //Should openSet not contain this neighbour cell it adds it as it is an unevaluated cell it now knows about
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);

                            //Raises an event to update the ui, namly the pen drawing the path. Delays after to allow a user to see the change
                            //  OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, openSet)); //...Ugly
                            if (_delay > 0)
                            {
                                Thread.Sleep(_delay);
                            }
                        }
                        //If the openSet contains this neighbour, but the neighbour's gScore is lower than or equal to the newly calculated gScore, it will be ignored
                        else if (tenativeGScore >= gScore[neighbour])
                        {
                            continue;
                        }

                        //As this neighbour is not in closedSet and has been added to openSet it will be the next cell used.
                        //cameFrom for this neighbour will be currentCell
                        cameFrom[neighbour] = currentCell;

                        //gScore for this neighbour is the new gScore 
                        gScore[neighbour] = tenativeGScore;

                        //fScore for this neighbour is it's gScore + the heuristic to the end
                        fScore[neighbour] = gScore[neighbour] + CalculateDistance(neighbour, endCell);
                    }
                }
                //Should the program have evaluated every cell, with no endCell being found it throws an error as there must be no way to reach the endCell
                throw new Exception(string.Format("Error: unable to find endCell, at: {0}", endCell.Name));
            }
        }



        private Cell GetCellWithLowestFScore(List<Cell> openSet, Dictionary<Cell, int> fScore)
        {
            //the self documenting code explains this function
            int lowestFScore = Int32.MaxValue;
            Cell cellWithLowestFScore = null;

            foreach (Cell cell in openSet)
            {
                int currentFScore = fScore[cell];
                if (currentFScore < lowestFScore)
                {
                    lowestFScore = currentFScore;
                    cellWithLowestFScore = cell;
                }
            }
            return cellWithLowestFScore;
        }

        private List<Cell> reconstructPath(Grid grid, Dictionary<Cell, Cell> cameFrom, Cell currentCell)
        {
            List<Cell> totalPath = new List<Cell>();

            //The while loop below will not add the endCell as no cameFrom leads from it, so it must be added first
            totalPath.Add(currentCell);

            //Ensures the dictionary contains the next cell, which should always be true
            while (cameFrom.ContainsKey(currentCell))
            {
                currentCell = cameFrom[currentCell];

                //If null, or is the first index, break out of the loop and return the path
                if (currentCell == null)
                    break;
                totalPath.Add(currentCell);

                //Updates ui canvas with route everytime a new cell is added to totalPath and delays so the user can see it clearly
                OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, totalPath));
                if (_delay > 0)
                {
                    Thread.Sleep(_delay);
                }
            }
            return totalPath;
        }


        private List<Cell> GetTraversableNeighbours(Grid grid, Cell currentCell) //check not backwards
        {
            //Simply checks each of the four cells around currentCell to see if they have a passageway to it and adds this cell to a list data structure to be returned.
            //It doesn't matter if the only cell returned is currentCell's cameFrom cell as the returned cell is check if it is in both openSet and closedSet later on
            List<Cell> neighbours = new List<Cell>();
            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                if (currentCell.GetEdge((Direction)i) is CellPassage)
                {
                    Point offset = DirectionHelper.GetOffset((Direction)i);
                    Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                    Cell neighbourCell = grid.GetCell(point);

                    neighbours.Add(neighbourCell);
                }
            }

            return neighbours;
        }

        private int CalculateDistance(Cell startCell, Cell endCell)
        {
            //Simply returns the Manhattan or rectilinear Distance between the two cells, calculated as is shown
            return Math.Abs(endCell.Point.X - startCell.Point.X) + Math.Abs(endCell.Point.Y - startCell.Point.Y);
        }


    }
}
