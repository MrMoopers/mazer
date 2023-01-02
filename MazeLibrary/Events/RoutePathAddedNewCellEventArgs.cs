using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary.Events
{
    //Event for getting the ui canvas to update with the latest information, namely the grid its working on and the latest version of the route path
    public class RoutePathAddedNewCellEventArgs : EventArgs
    {
        private Grid _grid;
        private List<Cell> _routeCells;

        public RoutePathAddedNewCellEventArgs(Grid grid, List<Cell> routeCells)
        {
            _grid = grid;
            _routeCells = routeCells;
        }

        public Grid Grid
        {
            get { return _grid; }
        }

        public List<Cell> RouteCells
        {
            get { return _routeCells; }
        }
    }
}
