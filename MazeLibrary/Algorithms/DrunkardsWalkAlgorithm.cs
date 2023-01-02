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
    public class DrunkardsWalkAlgorithm : AlgorithmBase
    {
        //http://www.jamisbuck.org/presentations/rubyconf2011/index.html#aldous-broder-demo

        private Random random = new Random();

        //Forces delay
        public DrunkardsWalkAlgorithm(int delay)
        {
            _delay = delay;
        }

        public void Generate(Grid grid)
        {
            List<Cell> activeCells = new List<Cell>();

            Cell currentCell = grid.CreateCell(grid.RandomPoint());
            activeCells.Add(currentCell);

            int cellCount = grid.Height * grid.Width;
            while (activeCells.Count < cellCount)
            {
                OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, new List<Cell>(new Cell[] { currentCell })));

                //Finds a random cell neighbouring currentCell
                Direction direction = DirectionHelper.RandomDirection();
                Point offset = DirectionHelper.GetOffset(direction);
                Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);

                //If the point is not on the grid, it cannot create a cell there and must instead create a wall in the direction variable's direction
                if (grid.ContainsPoint(point))
                {
                    //Attempts to get the cell where the point is...
                    Cell neighbourCell = grid.GetCell(point);

                    //If it is null (doesn't exist yet)...
                    if (neighbourCell == null)
                    {
                        //creates the cell and...
                        neighbourCell = grid.CreateCell(point);
                        CellEdge cellEdge = currentCell.GetEdge(direction);
                        //if there is no wall or passageway between currentCell and neighbourCell...
                        if (cellEdge == null)
                        {
                            //creates a passageway, then updating the ui
                            grid.CreatePassage(currentCell, neighbourCell, direction);
                            OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, direction));
                        }

                        //Uses neighbourCell as the next cell to build from
                        currentCell = neighbourCell;
                    }
                    else
                    {
                        //If neighbour cell already exists, check the edge between the two cells. If it is not set yet set it as a wall and update the ui
                        CellEdge cellEdge = currentCell.GetEdge(direction);
                        if (cellEdge == null)
                        {
                            grid.CreateWall(currentCell, neighbourCell, direction);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                        }

                        //Uses neighbourCell as the next cell to build from
                        currentCell = neighbourCell;
                    }
                }
                else
                {
                    //Creates a wall at the border, should a wall already be on that edge it it doesn't waste time replacing it with a new one
                    CellEdge cellEdge = currentCell.GetEdge(direction);
                    if (cellEdge == null)
                    {
                        grid.CreateWall(currentCell, null, direction);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                    }
                }

                //A list being used to keep track of all cells with all their edges set
                if (!activeCells.Contains(currentCell) && currentCell.IsFullyInitialized)
                {
                    activeCells.Add(currentCell);
                }

                //Delaying the ui updates so a user can view them
                if (_delay > 0)
                {
                    Thread.Sleep(_delay);
                }
            }
        }

        public void Solve(ref Grid grid, out List<Cell> activeCells, out List<Cell> visitedCells)
        {
            //Sets startCell and endCell as the top left and bottom right respectfully. The currentCell is startCell
            Cell startCell = grid.GetCell(0, 0);
            Cell endCell = grid.GetCell(grid.Width - 1, grid.Height - 1);
            Cell currentCell = startCell;

            //Adding currentCell to list of explored cells
            activeCells = new List<Cell>();
            visitedCells = new List<Cell>();
            activeCells.Add(currentCell);
            visitedCells.Add(currentCell);
            OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, activeCells));

            //Stops searching when the goal is found
            while (currentCell != endCell)
            {
                // OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, activeCells));

                //Gets a random direction and the cell that direction moves it to from currentCell. 
                //This is done by the following function which ensures that the returned direction is through a passageway
                Direction direction = GetRandomPassage(grid, currentCell);

                Point offset = DirectionHelper.GetOffset(direction);
                Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                Cell nextCell = grid.GetCell(point);

                if (!visitedCells.Contains(nextCell))
                {
                    visitedCells.Add(nextCell);
                    OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, visitedCells));
                }

                //If the list doesn't contain the cell it adds it and updates the ui, otherwise it removes the latest addition to the list.
                //(Stops multiples of the same cell being added)
                if (!activeCells.Contains(nextCell))
                {
                    activeCells.Add(nextCell);

                    OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, activeCells));
                }
                else
                {
                    activeCells.RemoveAt(activeCells.Count() - 1);
                    OnRaiseRoutePathAddedNewCellEvent(new RoutePathAddedNewCellEventArgs(grid, activeCells));
                }

                //Delays the ui after any update
                if (_delay > 0)
                {
                    Thread.Sleep(_delay);
                }

                currentCell = nextCell;
            }

        }

        private Direction GetRandomPassage(Grid grid, Cell currentCell)
        {
            //Creates a list of all passageways from currentCell to it's neighbours
            List<Direction> directionList = new List<Direction>();

            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                if (currentCell.GetEdge((Direction)i) is CellPassage)
                {
                    directionList.Add((Direction)i);
                }
            }
            //chooses one cell from that list and returns the direction to it.
            int index = random.Next(0, directionList.Count);
            return directionList[index];
        }
    }
}