using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Edge
    {
        private string _name;
        private Node _startNode;
        private Node _endNode;
        private bool _isTraversable;

        public Edge(string name, Node startNode, Node endNode, bool isTraversable)
        {
            _name = name;
            _startNode = startNode;
            _endNode = endNode;
            _isTraversable = isTraversable;

            startNode.AddEdge(this);
            endNode.AddEdge(this);
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Node StartNode
        {
            get { return _startNode; }
            set { _startNode = value; }
        }

        public Node EndNode
        {
            get { return _endNode; }
            set { _endNode = value; }
        }

        public bool IsTraversable
        {
            get { return _isTraversable; }
            set { _isTraversable = value; }
        }

        private string DebuggerDisplay
        {
            get { return String.Format("Edge: {0} ({1} -> {2})", Name, StartNode.Name, EndNode.Name); }
        }
    }
}
