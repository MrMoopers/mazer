using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary
{
    public abstract class CellEdge
    {
        //protected because these variables can be used by this class and those inherited from it
        //Another case of Encapsulation here as well
        protected Cell _startCell;
        protected Cell _endCell;
        protected Direction _direction;

        public Cell StartCell
        {
            get { return _startCell; }
            set { _startCell = value; }
        }

        public Cell EndCell
        {
            get { return _endCell; }
            set { _endCell = value; }
        }

        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public void Initialize(Cell startCell, Cell endCell, Direction direction)
        {
            _startCell = startCell;
            _endCell = endCell;
            _direction = direction;

            startCell.SetEdge(direction, this);
        }

        //virtual lets you override this function in the derived classes
        public virtual string ToString(int level)
        {
            return null;
        }
    }
}
