using MazeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary.Events
{
    //Event for getting the ui canvas to update with the latest information, namely the grid its working on, 
    //the cell the wall came from and the direction
    public class CellWallCreatedEventArgs : EventArgs
    {
        private Grid _grid;
        private Cell _cell;
        private Direction _direction;

        public CellWallCreatedEventArgs(Grid grid, Cell cell, Direction direction)
        {
            _grid = grid;
            _cell = cell;
            _direction = direction;
        }

        public Grid Grid
        {
            get { return _grid; }
        }

        public Cell Cell
        {
            get { return _cell; }
        }

        public Direction Direction
        {
            get { return _direction; }
        }
    }
}
