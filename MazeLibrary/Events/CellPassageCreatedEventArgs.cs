using MazeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary.Events
{
    //Event for getting the ui canvas to update with the latest information, namely the grid its working on, 
    //the cells the passage came from and is going and the direction
    public class CellPassageCreatedEventArgs : EventArgs
    {
        private Grid _grid;
        private Cell _fromCell;
        private Cell _toCell;
        private Direction _direction;

        public CellPassageCreatedEventArgs(Grid grid, Cell fromCell, Cell toCell, Direction direction)
        {
            _grid = grid;
            _fromCell = fromCell;
            _toCell = toCell;
            _direction = direction;
        }

        public Grid Grid
        {
            get { return _grid; }
        }

        public Cell FromCell
        {
            get { return _fromCell; }
        }

        public Cell ToCell
        {
            get { return _toCell; }
        }

        public Direction Direction
        {
            get { return _direction; }
        }
    }
}
