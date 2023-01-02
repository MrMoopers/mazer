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
    public class PrimsAlgorithm : AlgorithmBase
    {
        //http://www.jamisbuck.org/presentations/rubyconf2011/index.html#prim-demo

        public PrimsAlgorithm(int delay)
        {
            _delay = delay;
        }
        private Random _random = new Random();

        public void Generate(Grid grid)
        {
            //Discovered cells where at least one edge from it is null
            List<Cell> activeCells = new List<Cell>();

            //Discovered cells where all edges from  it are completed
            List<Cell> completedCells = new List<Cell>();

            Cell currentCell = grid.CreateCell(grid.RandomPoint());
            activeCells.Add(currentCell);

            int cellCount = grid.Height * grid.Width;

            //Only stops the algorithm when the number of completed cells is equal to the maxium number of cells in the grid
            while (completedCells.Count < cellCount)
            {
                //The next cell it will prgress from is chosen randomly from the list of active cells
                currentCell = activeCells[_random.Next(0, activeCells.Count)];//check

                OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, activeCells));

                //Gets a null direction from the currentCell to get a neighbour cell to it later, if necessary
                Direction direction = currentCell.GetRandomUninitializedDirection();
                Point offset = DirectionHelper.GetOffset(direction);
                Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);


                if (grid.ContainsPoint(point))
                {
                    Cell neighbourCell = grid.GetCell(point);
                    if (neighbourCell == null)
                    {
                        neighbourCell = grid.CreateCell(point);
                        //As the cell is on the grid and as it hasn't been created yet, a passage can be made to it
                        grid.CreatePassage(currentCell, neighbourCell, direction);
                        OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, direction));
                    }
                    else
                    {
                        //As the cell has been made already, a wall must be placed to prevent creation of a loop
                        grid.CreateWall(currentCell, neighbourCell, direction);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                    }

                    //Checks if the currentCell is fully intialised, if so removes it from the activeCells and then adds it to the completedCells list
                    if (currentCell.IsFullyInitialized)
                    {
                        activeCells.Remove(currentCell);
                        completedCells.Add(currentCell);
                    }


                    currentCell = neighbourCell;
                }
                else
                {
                    //As the cell is off the grid a border wall must be placed
                    grid.CreateWall(currentCell, null, direction);
                    OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                }

                //If the new cell hasn't been added to the known cells and isn't fully intialised then add it
                if (!activeCells.Contains(currentCell) && !currentCell.IsFullyInitialized)
                {
                    activeCells.Add(currentCell);
                }

                //Checks if the new currentCell is fully intialised, if so removes it from the activeCells and then adds it to the completedCells list
                if (currentCell.IsFullyInitialized)
                {
                    activeCells.Remove(currentCell);
                    completedCells.Add(currentCell);
                }

                //Delays so the UI can update and user can see any changes
                if (_delay > 0)
                {
                    Thread.Sleep(_delay);
                }

            }
        }



        public List<Cell> Solve(Grid _grid)
        {
            throw new NotImplementedException();
        }
    }
}
