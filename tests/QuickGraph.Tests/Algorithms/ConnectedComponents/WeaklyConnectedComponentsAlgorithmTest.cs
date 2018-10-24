using FluentAssertions;
using QuickGraph.Serialization;
using Xunit;

namespace QuickGraph.Algorithms.ConnectedComponents
{
    public class WeaklyConnectedComponentsAlgorithmTest
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
        public void Compute<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var dfs =
                new WeaklyConnectedComponentsAlgorithm<TVertex, TEdge>(g);
            dfs.Compute();
            if (g.VertexCount == 0)
            {
                dfs.ComponentCount.Should().Be(0);
                return;
            }

            dfs.ComponentCount.Should().BePositive();
            dfs.ComponentCount.Should().BeLessOrEqualTo(g.VertexCount);
            foreach (var kv in dfs.Components)
            {
                kv.Value.Should().BeGreaterOrEqualTo(0);
                kv.Value.Should().BeLessThan(dfs.ComponentCount);
            }

            foreach (var vertex in g.Vertices)
                foreach (var edge in g.OutEdges(vertex))
                {
                    dfs.Components[edge.Source].Should().Be(dfs.Components[edge.Target]);
                }
        }
    }
}
