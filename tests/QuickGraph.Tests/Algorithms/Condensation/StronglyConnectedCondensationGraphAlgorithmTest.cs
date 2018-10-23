using FluentAssertions;
using QuickGraph.Serialization;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QuickGraph.Algorithms.Condensation
{
    public class StronglyConnectedCondensationGraphAlgorithmTest
    {
        public static IEnumerable<object[]> AdjacencyGraphs =>
            TestGraphFactory.GetAdjacencyGraphs()
                .Select(x => new[] { x });

        [Theory]
        [MemberData(nameof(AdjacencyGraphs))]
        public void StronglyConnectedCondensate<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var cg = g.CondensateStronglyConnected<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>();

            CheckVertexCount(g, cg);
            CheckEdgeCount(g, cg);
            CheckComponentCount(g, cg);
            CheckDAG(cg);
        }

        private static void CheckVertexCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            var count = 0;
            foreach (var vertices in cg.Vertices)
                count += vertices.VertexCount;
            g.VertexCount.Should().Be(count);
        }

        private static void CheckEdgeCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            var count = 0;
            foreach (var edges in cg.Edges)
                count += edges.Edges.Count;
            foreach (var vertices in cg.Vertices)
                count += vertices.EdgeCount;
            g.EdgeCount.Should().Be(count);
        }


        private static void CheckComponentCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check number of vertices = number of storngly connected components
            var componentCount = g.StronglyConnectedComponents(out IDictionary<TVertex, int> components);
            componentCount.Should().Be(cg.VertexCount);
        }

        private static void CheckDAG<TVertex, TEdge>(
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            cg.Invoking(x => x.TopologicalSort()).Should().NotThrow<NonAcyclicGraphException>("Graph should be a DAG");
        }
    }
}
