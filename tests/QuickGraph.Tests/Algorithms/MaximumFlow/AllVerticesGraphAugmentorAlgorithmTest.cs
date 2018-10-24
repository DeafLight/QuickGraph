using FluentAssertions;
using QuickGraph.Serialization;
using Xunit;

namespace QuickGraph.Algorithms.MaximumFlow
{
    public class AllVerticesGraphAugmentorAlgorithmTest
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
        public void Augment(IMutableVertexAndEdgeListGraph<string, Edge<string>> g)
        {
            var vertexCount = g.VertexCount;
            var edgeCount = g.EdgeCount;
            var vertexId = g.VertexCount + 1;
            var edgeID = g.EdgeCount + 1;
            using (var augmentor = new AllVerticesGraphAugmentorAlgorithm<string, Edge<string>>(
                g,
                () => (vertexId++).ToString(),
                (s, t) => new Edge<string>(s, t)
                ))
            {
                augmentor.Compute();
                VerifyCount(g, augmentor, vertexCount);
                VerifySourceConnector(g, augmentor);
                VerifySinkConnector(g, augmentor);
            }
            g.VertexCount.Should().Be(vertexCount);
            g.EdgeCount.Should().Be(edgeCount);
        }

        private static void VerifyCount<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            int vertexCount)
            where TEdge : IEdge<TVertex>
        {
            g.VertexCount.Should().Be(vertexCount + 2);
            g.ContainsVertex(augmentor.SuperSource).Should().BeTrue();
            g.ContainsVertex(augmentor.SuperSink).Should().BeTrue();
        }

        private static void VerifySourceConnector<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                if (v.Equals(augmentor.SuperSource))
                    continue;
                if (v.Equals(augmentor.SuperSink))
                    continue;
                g.ContainsEdge(augmentor.SuperSource, v).Should().BeTrue();
            }
        }

        private static void VerifySinkConnector<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                if (v.Equals(augmentor.SuperSink))
                    continue;
                if (v.Equals(augmentor.SuperSink))
                    continue;
                g.ContainsEdge(v, augmentor.SuperSink).Should().BeTrue();
            }
        }
    }

    public sealed class StringVertexFactory
    {
        private int id;

        public StringVertexFactory()
            : this("Super")
        { }

        public StringVertexFactory(string prefix)
        {
            Prefix = prefix;
        }

        public string Prefix { get; set; }

        public string CreateVertex()
        {
            return Prefix + (++id);
        }
    }
}
