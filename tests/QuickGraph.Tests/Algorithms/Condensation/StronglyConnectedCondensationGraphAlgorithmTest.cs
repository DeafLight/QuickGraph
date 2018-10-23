﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph.Serialization;
using System.Collections.Generic;

namespace QuickGraph.Algorithms.Condensation
{
    [TestClass, PexClass]
    public partial class StronglyConnectedCondensationGraphAlgorithmTest
    {
        [TestMethod]
        public void StronglyConnectedCondensateAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                this.StronglyConnectedCondensate(g);
        }

        [PexMethod]
        public void StronglyConnectedCondensate<TVertex, TEdge>(
            [PexAssumeNotNull]IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var cg = g.CondensateStronglyConnected<TVertex, TEdge, AdjacencyGraph<TVertex,TEdge>>();

            CheckVertexCount(g, cg);
            CheckEdgeCount(g, cg);
            CheckComponentCount(g, cg);
            CheckDAG(g, cg);
        }

        private void CheckVertexCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            int count = 0;
            foreach (var vertices in cg.Vertices)
                count += vertices.VertexCount;
            Assert.AreEqual(g.VertexCount, count, "VertexCount does not match");
        }

        private void CheckEdgeCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            int count = 0;
            foreach (var edges in cg.Edges)
                count += edges.Edges.Count;
            foreach (var vertices in cg.Vertices)
                count += vertices.EdgeCount;
            Assert.AreEqual(g.EdgeCount, count, "EdgeCount does not match");
        }


        private void CheckComponentCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check number of vertices = number of storngly connected components
            IDictionary<TVertex, int> components;
            int componentCount = g.StronglyConnectedComponents(out components);
            Assert.AreEqual(componentCount, cg.VertexCount, "ComponentCount does not match");
        }

        private void CheckDAG<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check it's a dag
            try
            {
                cg.TopologicalSort();
            }
            catch (NonAcyclicGraphException)
            {
                Assert.Fail("Graph is not a DAG.");
            }

        }
    }
}
