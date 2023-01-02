using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    //The ':' means this CellWall class is derived from a base class CellEdge
    public class CellWall : CellEdge
    {
        private string DebuggerDisplay
        {
            get { return String.Format("Wall: {0} {1} {2}", _startCell.Point, _endCell == null ? "[Border]" : _endCell.Point.ToString(), _direction); }
        }

        //The 'override' means we are overriding the function in the base class that
        //has the same name.
        public override string ToString(int level)
        {
            string indent = new string(' ', 2 * level);
            return String.Format("{0}Wall: {1} {2} ({3})",
                indent,
                _startCell.Name,
                _endCell == null ? "Border" : _endCell.Name,
                _direction);
        }
    }
}
