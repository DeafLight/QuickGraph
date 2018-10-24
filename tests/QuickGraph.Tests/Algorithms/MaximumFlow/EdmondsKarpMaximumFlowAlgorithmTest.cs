using QuickGraph.Algorithms;
using QuickGraph.Serialization;
using System.Linq;
using Xunit;

namespace QuickGraph.Tests.Algorithms.MaximumFlow
{
    public class EdmondsKarpMaximumFlowAlgorithmTest
    {
        private static readonly EdgeFactory<string, Edge<string>> _edgeFactory = (source, target) => new Edge<string>(source, target);

        public static TheoryData<IVertexAndEdgeListGraph<string, Edge<string>>, EdgeFactory<string, Edge<string>>> NonEmptyAdjacencyGraphs
        {
            get
            {
                var data = new TheoryData<IVertexAndEdgeListGraph<string, Edge<string>>, EdgeFactory<string, Edge<string>>>();
                foreach (var g in TestGraphFactory.GetAdjacencyGraphs()
                    .Where(x => x.VertexCount > 0))
                    data.Add(g, _edgeFactory);

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(NonEmptyAdjacencyGraphs))]
        public void EdmondsKarpMaxFlow(
            AdjacencyGraph<string, Edge<string>> g,
            EdgeFactory<string, Edge<string>> edgeFactory)
        {
            foreach (var source in g.Vertices)
                foreach (var sink in g.Vertices.Where(x => !x.Equals(source)))
                    RunMaxFlowAlgorithm(g, edgeFactory, source, sink);
        }

        private static double RunMaxFlowAlgorithm(IMutableVertexAndEdgeListGraph<string, Edge<string>> g, EdgeFactory<string, Edge<string>> edgeFactory, string source, string sink)
        {
            var flow = g.MaximumFlowEdmondsKarp(
                e => 1,
                source, sink,
                out TryFunc<string, Edge<string>> flowPredecessors,
                edgeFactory
                );

            return flow;
        }

    }
}
