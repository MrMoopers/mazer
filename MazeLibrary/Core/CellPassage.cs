using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
// Object Orientation is PIE in the sky!
//
// P = Polymorphism
// I = Inheritance
// E = Encapsulation
//

namespace MazeLibrary
{
    //Inheritance: The ':' means this CellPassage class is derived from a base class CellEdge. Or said
    //another way... CellPassage is inherited from CellEdge.
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class CellPassage : CellEdge
    {
        public override string ToString(int level)
        {
            string indent = new string(' ', 2 * level);
            return String.Format("{0}Passage: {1} {2} ({3})", indent, _startCell.Name, _endCell.Name, _direction);
        }

        private string DebuggerDisplay
        {
            get { return String.Format("Passage: {0} {1} {2}", _startCell.Point, _endCell.Point, _direction); }
        }
    }
}
