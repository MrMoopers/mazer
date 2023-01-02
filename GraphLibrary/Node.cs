using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Node
    {
        private string _name;
        private List<Edge> _edgeList = new List<Edge>();

        public Node(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void AddEdge(Edge e)
        {
            _edgeList.Add(e);
        }

        private string DebuggerDisplay
        {
            get { return String.Format("Node: {0}", Name); }
        }
    }
}
