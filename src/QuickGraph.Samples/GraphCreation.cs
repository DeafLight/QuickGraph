using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph.Algorithms;
using System;
using System.Linq;

namespace QuickGraph.Samples
{
    [TestClass]
    public class GraphCreation
    {
        [TestMethod]
        public void EdgeArrayToAdjacencyGraph()
        {
            var edges = new SEdge<int>[] {
                new SEdge<int>(1, 2),
                new SEdge<int>(0, 1)
            };
            var graph = edges.ToAdjacencyGraph<int, SEdge<int>>();
        }

        [TestMethod]
        public void DelegateGraph()
        {
            // a simple adjacency graph representation
            var graph = new int[][]{
                new int[] { 1 },
                new int[] { 2, 3 },
                new int[] { 3, 4 },
                new int[] { 4 },
                Array.Empty<int>()
            };

            // interoping with quickgraph
            var g = Enumerable.Range(0, graph.Length).ToDelegateVertexAndEdgeListGraph(
                v => Array.ConvertAll(graph[v],
                w => new SEquatableEdge<int>(v, w))
            );

            // it's ready to use!
            foreach (var v in g.TopologicalSort())
                Console.WriteLine(v);
        }
    }
}
