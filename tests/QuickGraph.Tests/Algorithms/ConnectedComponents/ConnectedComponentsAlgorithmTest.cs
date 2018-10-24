using FluentAssertions;
using QuickGraph.Serialization;
using System.Linq;
using Xunit;

namespace QuickGraph.Algorithms.ConnectedComponents
{
    public class ConnectedComponentsAlgorithmTest
    {
        public static TheoryData<UndirectedGraph<string, IEdge<string>>> UndirectedGraphs
        {
            get
            {
                var data = new TheoryData<UndirectedGraph<string, IEdge<string>>>();
                foreach (var g in TestGraphFactory.GetUndirectedGraphs())
                    data.Add(g);

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(UndirectedGraphs))]
        [Trait(TestCategories.Category, TestCategories.LongRunning)]
        public void Compute<TVertex, TEdge>(UndirectedGraph<TVertex, TEdge> g)
             where TEdge : IEdge<TVertex>
        {
            while (g.EdgeCount > 0)
            {
                var dfs = new ConnectedComponentsAlgorithm<TVertex, TEdge>(g);
                dfs.Compute();
                if (g.VertexCount == 0)
                {
                    dfs.ComponentCount.Should().Be(0);
                    return;
                }

                dfs.ComponentCount.Should().BeGreaterOrEqualTo(0);
                dfs.ComponentCount.Should().BeLessOrEqualTo(g.VertexCount);
                foreach (var kv in dfs.Components)
                {
                    kv.Value.Should().BeGreaterOrEqualTo(0);
                    kv.Value.Should().BeLessThan(dfs.ComponentCount);
                }

                foreach (var vertex in g.Vertices)
                    foreach (var edge in g.AdjacentEdges(vertex))
                    {
                        dfs.Components[edge.Source].Should().Be(dfs.Components[edge.Target]);
                    }

                g.RemoveEdge(g.Edges.First());
            }
        }
    }
}
