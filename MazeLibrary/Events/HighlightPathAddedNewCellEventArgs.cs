using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary.Events
{
    public class HighlightPathAddedNewCellEventArgs : EventArgs
    {
        private Grid _grid;
        private List<Cell> _highlightCells;

        public HighlightPathAddedNewCellEventArgs(Grid grid, List<Cell> highlightCells)
        {
            _grid = grid;
            _highlightCells = highlightCells;
        }

        public Grid Grid
        {
            get { return _grid; }
        }

        public List<Cell> HighlightCells
        {
            get { return _highlightCells; }
        }      
    }
}
