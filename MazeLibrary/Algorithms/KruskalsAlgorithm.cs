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
    public class KruskalsAlgorithm : AlgorithmBase
    {
        //http://www.jamisbuck.org/presentations/rubyconf2011/index.html#kruskal-demo
        //Works by making passages between any random cell and its neighbour then places thouse two in a new list,
        //if one of thouse cells belongs to another list it adds them both to that list.
        //If both cells belong to different lists it merges both lists into one list

        public KruskalsAlgorithm(int delay)
        {
            _delay = delay;
        }

        public void Generate(Grid grid)
        {
            List<List<Cell>> cellListList = new List<List<Cell>>();
            List<Cell> completedCellList = new List<Cell>();
            int numberOfCells = grid.Width * grid.Height;

            //Only stops the algorithm when the number of completed cells is equal to the maxium number of cells in the grid
            while (completedCellList.Count < numberOfCells)
            {
                Point randomPoint = grid.RandomPoint();
                Cell currentCell = grid.GetCell(randomPoint);
                if (currentCell == null)
                {
                    currentCell = grid.CreateCell(randomPoint);
                }

                //OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, new List<Cell>(new Cell[] { currentCell })));

                //Checks if the randomCell is part of a already made list
                List<Cell> currentCellList = GetCellList(currentCell, cellListList);
                if (currentCellList == null)
                {
                    //It isn't so make a new one
                    currentCellList = new List<Cell>();
                    currentCellList.Add(currentCell);
                    cellListList.Add(currentCellList);
                }
                

                if (!currentCell.IsFullyInitialized)
                {
                    Direction direction = currentCell.GetRandomUninitializedDirection();
                    Point offset = DirectionHelper.GetOffset(direction);
                    Point point = new Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);
                    if (grid.ContainsPoint(point))
                    {
                        //Simple part, if neighbour is null create the cell
                        Cell neighbourCell = grid.GetCell(point);

                        if (neighbourCell == null)
                        {
                            neighbourCell = grid.CreateCell(point);
                        }

                        OnRaiseHighlightPathAddedNewCellEvent(new HighlightPathAddedNewCellEventArgs(grid, new List<Cell>(new Cell[] { currentCell, neighbourCell })));

                        //NeighbourCell has already been added to the cellList so a wall must be created to ensure no loops are made later
                        if (currentCellList.Contains(neighbourCell))
                        {
                            grid.CreateWall(currentCell, neighbourCell, direction);
                            OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                        }
                        else
                        {
                            //It isn't part of the currentCells' list so it starts the process of adding neighbourCell to it
                            grid.CreatePassage(currentCell, neighbourCell, direction);
                            OnRaiseCellPassageCreatedEvent(new CellPassageCreatedEventArgs(grid, currentCell, neighbourCell, direction));

                            //Checks if the neighbourCell has its own list
                            List<Cell> neighborCellList = GetCellList(neighbourCell, cellListList);
                            if (neighborCellList == null)
                            {
                                //It doesn't so just add it
                                currentCellList.Add(neighbourCell);
                            }
                            else
                            {
                                //It does so add all cells in the neighbour's list to the currentCells list and delete the neighbours list, removing it from the list of lists
                                foreach (Cell cell in neighborCellList)
                                {
                                    currentCellList.Add(cell);
                                }
                                cellListList.Remove(neighborCellList);
                            }
                        }
                    }
                    else
                    {
                        //Edge becomes a border
                        grid.CreateWall(currentCell, null, direction);
                        OnRaiseCellWallCreatedEvent(new CellWallCreatedEventArgs(grid, currentCell, direction));
                    }
                }
                else
                {
                    //Adds the completed cell to the list of completed cells
                    if (!completedCellList.Contains(currentCell))
                    {
                        completedCellList.Add(currentCell);
                    }
                }
                
                if (_delay > 0)
                {
                    Thread.Sleep(_delay);
                }
            }
        }

        private List<Cell> GetCellList(Cell cell, List<List<Cell>> cellListList)
        {
            //function returning the cellList which contains the requested cell
            foreach (List<Cell> cellList in cellListList)
            {
                if (cellList.Contains(cell))
                {
                    return cellList;
                }
            }
            return null;
        }


        public List<Cell> Solve(Grid _grid)
        {
            throw new NotImplementedException();
        }
    }
}
