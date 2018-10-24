using FluentAssertions;
using QuickGraph.Algorithms;
using Xunit;

namespace QuickGraph.Tests.Algorithms.ConnectedComponents
{
    public class IncrementalConnectedComponentsAlgorithmTest
    {
        [Fact]
        public void IncrementalConnectedComponent()
        {
            var g = new AdjacencyGraph<int, SEquatableEdge<int>>();
            g.AddVertexRange(new int[] { 0, 1, 2, 3 });
            var components = g.IncrementalConnectedComponents();

            var current = components();
            current.Key.Should().Be(4);

            g.AddEdge(new SEquatableEdge<int>(0, 1));
            current = components();
            current.Key.Should().Be(3);

            g.AddEdge(new SEquatableEdge<int>(2, 3));
            current = components();
            current.Key.Should().Be(2);

            g.AddEdge(new SEquatableEdge<int>(1, 3));
            current = components();
            current.Key.Should().Be(1);
        }
    }
}
