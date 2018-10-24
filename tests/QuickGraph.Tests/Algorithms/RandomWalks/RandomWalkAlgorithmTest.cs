using QuickGraph.Algorithms.Observers;
using QuickGraph.Serialization;
using Xunit;

namespace QuickGraph.Algorithms.RandomWalks
{
    public class RandomWalkAlgorithmTest
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
        public void RoundRobinTest<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            if (g.VertexCount == 0)
                return;

            foreach (var root in g.Vertices)
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(g)
                {
                    EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge>()
                };
                walker.Generate(root);
            }
        }

        [Theory]
        [MemberData(nameof(AdjacencyGraphs))]
        public void RoundRobinTestWithVisitor<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            if (g.VertexCount == 0)
                return;

            foreach (var root in g.Vertices)
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(g)
                {
                    EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge>()
                };

                var vis = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis.Attach(walker))
                    walker.Generate(root);
            }
        }

    }
}
