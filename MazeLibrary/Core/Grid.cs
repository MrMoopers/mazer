using MazeLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary
{
    public class Grid
    {
        //Encapsulation of variables, methods and properties within a class shown here
        private string _name;
        private Random _random = new Random();
        private Size _size;
        private Cell[,] _cells;

        public Grid(string name, int width, int height)
        {
            _name = name;
            _size = new Size(width, height);
            _cells = new Cell[_size.Width, _size.Height];
        }

        public int Width
        {
            get { return _size.Width; }
        }

        public int Height
        {
            get { return _size.Height; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //Procedure for creating a passageway between two cells on the grid, from startCell to endCell in the direction given.
        //Notice it also creates the another passage in reverse from endCell to startCell in the opposite direction.
        public void CreatePassage(Cell startCell, Cell endCell, Direction direction)
        {
            CellPassage cellPassage1 = new CellPassage();
            cellPassage1.Initialize(startCell, endCell, direction);

            CellPassage cellPassage2 = new CellPassage();
            cellPassage2.Initialize(endCell, startCell, DirectionHelper.GetOpposite(direction));
        }

        //Procedure for creating a Wall between two cells on the grid, from startCell to endCell in the direction given.
        //Notice it also creates the another Wall in reverse from endCell to startCell in the opposite direction.
        public void CreateWall(Cell startCell, Cell endCell, Direction direction)
        {
            CellWall cellWall1 = new CellWall();
            cellWall1.Initialize(startCell, endCell, direction);
            if (endCell != null)
            {
                CellWall cellWall2 = new CellWall();
                cellWall2.Initialize(endCell, startCell, DirectionHelper.GetOpposite(direction));
            }
        }

        //Function to return true if a specified point is within the bounds of the grid's height and width
        public bool ContainsPoint(Point p)
        {
            if (p.X >= 0 && p.Y >= 0 && p.X < _size.Width && p.Y < _size.Height)
            {
                return true;
            }
            return false;
        }

        public bool ContainsPoint(int x, int y)
        {
            return ContainsPoint(new Point(x, y));
        }

        //function to return a random point on the grid within its' bounds
        public Point RandomPoint()
        {
            Point randomPoint = new Point(_random.Next(0, _size.Width), _random.Next(0, _size.Height));
            return randomPoint;
        }

        //Function for creating a cell on the grid with its coordinates as its name, for ease of debugging, at the specified width and height
        public Cell CreateCell(int i, int j)
        {
            string name = String.Format("({0}, {1})", i, j);
            Point point = new Point(i, j);
            Cell cell = new Cell(name, point);
            _cells[i, j] = cell;
            return cell;
        }

        //runs the previous CreateCell function, but allowing a point to be passed in instead two integers 
        public Cell CreateCell(Point p)
        {
            return CreateCell(p.X, p.Y);
        }

        //Function returns the cell at the specified x and y coordinates
        public Cell GetCell(int i, int j)
        {
            return _cells[i, j];
        }

        //function returns the cell at a specified point
        public Cell GetCell(Point p)
        {
            return GetCell(p.X, p.Y);
        }

        //This code relates only to outputting the maze as a string in console, to start testing if the WPF application would be possible to create
        public string ToString(int level = 0)
        {
            StringBuilder sb = new StringBuilder();
            string indent = new string(' ', 2 * level);
            sb.AppendLine(String.Format("{0}Grid: {1}", indent, _name));
            for (int i = 0; i < _size.Width; i++)
            {
                for (int j = 0; j < _size.Height; j++)
                {
                    Cell cell = GetCell(i, j);
                    if (cell != null)
                    {
                        sb.Append(cell.ToString(level + 1));
                    }
                }
            }
            return sb.ToString();
        }

        //Function for converting a textfile format into a maze it can use
        public static Grid GetGridFromFile(string filename)
        {

            using (StreamReader reader = new StreamReader(filename, true))
            {
                //Array stores the first line of the text file in three parts, where seporated by a verticle bar
                string[] splitter = reader.ReadLine().Split('|');

                //Creates the grid called the first part, width the second part, and height the third part
                Grid grid = new Grid(splitter[0], Convert.ToInt32(splitter[1]), Convert.ToInt32(splitter[2]));
                //e.g. "Maze1|5|6"

                //Prepare Grid Cells and Borders
                for (int y = 0; y < grid.Height; y++)
                {
                    for (int x = 0; x < grid.Width; x++)
                    {
                        //Creates a cell at every position on the grid and...
                        Cell currentCell = grid.CreateCell(x, y);

                        //fills its north direction in with a wall if it is at the north border
                        if (y == 0)
                        {
                            grid.CreateWall(currentCell, null, Direction.North);
                        }

                        //fills its south direction in with a wall if it is at the south border
                        if (y == grid.Height - 1)
                        {
                            grid.CreateWall(currentCell, null, Direction.South);
                        }

                        //fills its west direction in with a wall if it is at the west border
                        if (x == 0)
                        {
                            grid.CreateWall(currentCell, null, Direction.West);
                        }

                        //fills its east direction in with a wall if it is at the east border
                        if (x == grid.Width - 1)
                        {
                            grid.CreateWall(currentCell, null, Direction.East);
                        }
                    }
                }

                //line equals the next line in the file
                string line = reader.ReadLine();

                //steps through every cell in the grid
                for (int y = 0; y < grid.Height; y++)
                {
                    for (int x = 0; x < grid.Width; x++)
                    {
                        //Only gets the directions from the currentCell going North or East, this ensures at no point will it try to replace an already set edge
                        for (int i = 0; i < 4; i = i + 3)
                        {
                            Cell currentCell = grid.GetCell(x, y);
                            Direction direction = (Direction)i;
                            System.Drawing.Point offset = DirectionHelper.GetOffset(direction);
                            System.Drawing.Point point = new System.Drawing.Point(currentCell.Point.X + offset.X, currentCell.Point.Y + offset.Y);

                            //Only continues if the new neighbour point is on the grid, as off the grid values have already been done
                            if (grid.ContainsPoint(point))
                            {
                                //Note: the text file format lists every cell's edges in the order of the enum class Direction and also when saving
                                //files it uses only the char's 'P' and 'W'
                                if (line[i] == 'P')
                                {
                                    grid.CreatePassage(currentCell, grid.GetCell(point), direction);
                                }
                                else if (line[i] == 'W')
                                {
                                    grid.CreateWall(currentCell, grid.GetCell(point), direction);
                                }
                                else
                                {
                                    //Error thrown if an unexpected character is in the file
                                    throw new Exception("Uninitialized edge detected?");
                                }
                            }
                        }
                        //Gets the next line
                        line = reader.ReadLine();
                    }
                }
                //The grid will at this stage be complete and be a completed maze therefore it returns it
                return grid;
            }
        }
    }
}
