using MazeLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Cell
    {
        private static Random _random = new Random();

        //Encapsulation: Private storage with public properties and functions.
        private string _name;
        private Point _point;
        private CellEdge[] _edges = new CellEdge[DirectionHelper.Count];
        private int _initialisedEdgeCount;

        public Cell(string name, Point point)
        {
            _name = name;
            _point = point;
        }

        public Cell(string name, int x, int y)
        {
            _name = name;
            _point = new Point(x, y);
        }

        //properties for getting the name or point at a cell 
        public string Name
        {
            get { return _name; }
        }

        public Point Point
        {
            get { return _point; }
        }

        //property for getting the name of a cell to be displayed in the output tab when debugging code
        private string DebuggerDisplay
        {
            get { return String.Format("Cell: {0}", Name); }
        }


        //Method for replacing whatever value was in that element of the _edges array with a new cellEdge and a direction.
        //_initialisedEdgeCount stores the number of edges that a cell has, it should never go above 4
        public void SetEdge(Direction direction, CellEdge cellEdge)
        {
            //Only increase the initialised edge count if the edge that is being set is currently null. If
            //it is not null then we are changing an edge that was already initialised so the count does not
            //need to be increased.
            if (_edges[(int)direction] == null)
            {
                _initialisedEdgeCount += 1;
            }

            _edges[(int)direction] = cellEdge;
        }

        //function for returning a specific edge name of a direction from a cell
        public CellEdge GetEdge(Direction direction)
        {
            return _edges[(int)direction];
        }

        //Debugging and testing for creating the maze in string form using the console
        public string ToString(int level = 0)
        {
            StringBuilder sb = new StringBuilder();
            string indent = new string(' ', 2 * level);
            sb.AppendLine(String.Format("{0}Cell: {1} ({2})", indent, _name, _point));
            foreach (CellEdge cellEdge in _edges)
            {
                if (cellEdge != null)
                {
                    //Polymorphic behaviour... the cellEdge calls the correct Display()
                    //routine depending on whether this particular cellEdge is a CellWall
                    //or CellPassage derived class.
                    sb.AppendLine(cellEdge.ToString(level + 1));
                }
            }
            return sb.ToString();
        }

        //returns if the current cell has exactly 4 edges, all of which arn't null
        public bool IsFullyInitialized
        {
            get { return _initialisedEdgeCount == DirectionHelper.Count; }
        }

        //Function returning all edges of a cell that are null, hence if there arn't any then a problem has occurred
        public Direction GetRandomUninitializedDirection()
        {
            List<Direction> directionList = new List<Direction>();
            for (int i = 0; i < DirectionHelper.Count; i++)
            {
                if (_edges[i] == null)
                {
                    directionList.Add((Direction)i);
                }
            }
            int index = _random.Next(0, directionList.Count);
            return directionList[index];
        }

        //returns the number of edges a cell has
        public int InitialisedEdgeCount
        {
            get { return _initialisedEdgeCount; }
        }
    }
}
