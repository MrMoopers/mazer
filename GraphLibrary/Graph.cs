using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Graph
    {
        private string _name;
        private List<Node> _nodeList = new List<Node>();
        private List<Edge> _edgeList = new List<Edge>();

        public Graph()
        {
        }

        public Graph(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<Node> NodeList
        {
            get { return _nodeList; }
        }

        public List<Edge> EdgeList
        {
            get { return _edgeList; }
        }

        public void AddNode(Node n)
        {
            _nodeList.Add(n);
        }

        public void AddEdge(Edge e)
        {
            _edgeList.Add(e);
        }

        public Edge AddEdge(string name, Node startNode, Node endNode, bool isTraversable)
        {
            Edge e = new Edge(name, startNode, endNode, isTraversable);
            AddEdge(e);
            return e;
        }

        private string DebuggerDisplay
        {
            get { return String.Format("Graph: {0}", Name); }
        }
    }
}
