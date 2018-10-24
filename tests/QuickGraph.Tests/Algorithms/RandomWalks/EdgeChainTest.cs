using QuickGraph.Algorithms.Observers;
using QuickGraph.Serialization;
using Xunit;

namespace QuickGraph.Algorithms.RandomWalks
{
    public class EdgeChainTest
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
        public void Generate<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(g);
                var vis = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis.Attach(walker))
                    walker.Generate(v);
            }
        }
    }
}
