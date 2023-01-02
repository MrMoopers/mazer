using GraphLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph g1 = new Graph("TestGraph");

            Node n1 = new Node("Node_1");
            Node n2 = new Node("Node_2");
            Node n3 = new Node("Node_3");
            Node n4 = new Node("Node_4");
            Node n5 = new Node("Node_5");

            g1.AddNode(n1);
            g1.AddNode(n2);
            g1.AddNode(n3);
            g1.AddNode(n4);
            g1.AddNode(n5);
            //foreach (Node n in g1.NodeList)
            //{
            //    Console.WriteLine("{0}", n.Name);
            //}
            //Console.ReadKey();
            //Edge e3 = new Edge("Edge_3", n2, n3, true);
            //g1.AddEdge(e3);

            g1.AddEdge("Edge_1", n1, n2, true);
            g1.AddEdge("Edge_2", n1, n3, true);
            g1.AddEdge("Edge_3", n2, n3, true);
            g1.AddEdge("Edge_4", n2, n4, false);
            g1.AddEdge("Edge_5", n5, n1, true);
        }
    }
}
