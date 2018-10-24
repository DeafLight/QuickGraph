using FluentAssertions;
using QuickGraph.Serialization;
using System.Collections.Generic;
using Xunit;

namespace QuickGraph.Algorithms.Condensation
{
    public class WeaklyConnectedCondensationGraphAlgorithmTest
    {
        public static TheoryData<IVertexAndEdgeListGraph<string, Edge<string>>> AdjacencyGraphs
        {
            get
            {
                var data = new TheoryData<IVertexAndEdgeListGraph<string, Edge<string>>>();
                foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                    data.Add(g);

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(AdjacencyGraphs))]
        public void WeaklyConnectedCondensate<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var algo = new CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>(g)
            {
                StronglyConnected = false
            };
            algo.Compute();
            CheckVertexCount(g, algo);
            CheckEdgeCount(g, algo);
            CheckComponentCount(g, algo);
        }

        private static void CheckVertexCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
            CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            var count = 0;
            foreach (var vertices in algo.CondensedGraph.Vertices)
                count += vertices.VertexCount;
            g.VertexCount.Should().Be(count);
        }

        private static void CheckEdgeCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
            CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            var count = 0;
            foreach (var edges in algo.CondensedGraph.Edges)
                count += edges.Edges.Count;
            foreach (var vertices in algo.CondensedGraph.Vertices)
                count += vertices.EdgeCount;
            g.EdgeCount.Should().Be(count);
        }


        private static void CheckComponentCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
            CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            // check number of vertices = number of storngly connected components
            var components = g.WeaklyConnectedComponents<TVertex, TEdge>(new Dictionary<TVertex, int>());
            components.Should().Be(algo.CondensedGraph.VertexCount);
        }
    }
}
