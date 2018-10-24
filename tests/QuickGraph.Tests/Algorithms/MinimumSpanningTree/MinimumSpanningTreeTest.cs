using FluentAssertions;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.MinimumSpanningTree;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Collections;
using QuickGraph.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Xunit;

namespace QuickGraph.Tests.Algorithms.MinimumSpanningTree
{
    public class MinimumSpanningTreeTest
    {
        private static UndirectedGraph<string, IEdge<string>> GetUndirectedFullGraph(int vert)
        {
            Console.WriteLine("Start");
            var usedEdge = new List<KeyValuePair<int, int>>();
            var random = new Random();
            var graph = new UndirectedGraph<string, IEdge<string>>();
            var trueGraph = new UndirectedGraph<string, IEdge<string>>();
            var ds = new ForestDisjointSet<string>(vert);
            for (int i = 0; i < vert; i++)
            {
                graph.AddVertex(i.ToString());
                trueGraph.AddVertex(i.ToString());
                ds.MakeSet(i.ToString());
            }
            for (int i = 0; i < vert; i++)
                for (int j = i + 1; j < vert; j++)
                    graph.AddEdge(new TaggedEdge<string, double>(i.ToString(), j.ToString(), random.Next(100)));
            return graph;
        }

        public static TheoryData<IUndirectedGraph<string, IEdge<string>>, Func<IEdge<string>, double>> UndirectedFullGraphs
        {
            get
            {
                Func<IEdge<string>, double> fn = x => (x as TaggedEdge<string, double>).Tag;
                var data = new TheoryData<IUndirectedGraph<string, IEdge<string>>, Func<IEdge<string>, double>>();
                data.Add(GetUndirectedFullGraph(50), fn);
                data.Add(GetUndirectedFullGraph(10), fn);
                data.Add(GetUndirectedFullGraph(100), fn);
                data.Add(GetUndirectedFullGraph(200), fn);
                data.Add(GetUndirectedFullGraph(300), fn);
                data.Add(GetUndirectedFullGraph(400), fn);
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(UndirectedFullGraphs))]
        public void MyPrim(IUndirectedGraph<string, IEdge<string>> g, Func<IEdge<string>, double> edge)
        {
            var m = "";
            m += DateTime.Now.ToString() + " " + DateTime.Now.Millisecond + "\n";
            var graph = GetUndirectedFullGraph(10);
            m += DateTime.Now.ToString() + " " + DateTime.Now.Millisecond + "\n";
            var ed = g.Edges.ToList();
            var distances = new Dictionary<IEdge<string>, double>();
            foreach (var e in g.Edges)
                distances[e] = edge(e);

            var prim = new PrimMinimumSpanningTreeAlgorithm<string, IEdge<string>>(g, e => distances[e]);
            AssertMinimumSpanningTree(g, prim);
            m += DateTime.Now.ToString() + " " + DateTime.Now.Millisecond + "\n";
            Console.Write(m);
        }

        [Theory]
        [MemberData(nameof(UndirectedFullGraphs))]
        public void MyKruskal(IUndirectedGraph<string, IEdge<string>> g, Func<IEdge<string>, double> edge)
        {
            var m = "";
            m += DateTime.Now.ToString() + " " + DateTime.Now.Millisecond + "\n";
            var graph = GetUndirectedFullGraph(10);
            m += DateTime.Now.ToString() + " " + DateTime.Now.Millisecond + "\n";
            var ed = g.Edges.ToList();
            var distances = new Dictionary<IEdge<string>, double>();
            foreach (var e in g.Edges)
                distances[e] = edge(e);

            var prim = new KruskalMinimumSpanningTreeAlgorithm<string, IEdge<string>>(g, e => distances[e]);
            AssertMinimumSpanningTree(g, prim);
            m += DateTime.Now.ToString() + " " + DateTime.Now.Millisecond + "\n";
            Console.Write(m);
        }

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
        public void Kruskal<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (var e in g.Edges)
                distances[e] = g.AdjacentDegree(e.Source) + 1;

            var kruskal = new KruskalMinimumSpanningTreeAlgorithm<TVertex, TEdge>(g, e => distances[e]);
            AssertMinimumSpanningTree(g, kruskal);
        }

        [Theory]
        [MemberData(nameof(UndirectedGraphs))]
        public void Prim<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<IEdge<TVertex>, double>();
            foreach (var e in g.Edges)
                distances[e] = g.AdjacentDegree(e.Source) + 1;

            var edges = g.MinimumSpanningTreePrim(e => distances[e]);
            AssertSpanningTree(g, edges);
        }

        private static void AssertMinimumSpanningTree<TVertex, TEdge>(
            IUndirectedGraph<TVertex, TEdge> g,
            IMinimumSpanningTreeAlgorithm<TVertex, TEdge> algorithm)
            where TEdge : IEdge<TVertex>
        {
            var edgeRecorder = new EdgeRecorderObserver<TVertex, TEdge>();
            using (edgeRecorder.Attach(algorithm))
                algorithm.Compute();

            Console.WriteLine($"tree cost: {edgeRecorder.Edges.Count}");
            AssertSpanningTree(g, edgeRecorder.Edges);
        }

        private static void AssertSpanningTree<TVertex, TEdge>(
            IUndirectedGraph<TVertex, TEdge> g,
            IEnumerable<TEdge> tree)
            where TEdge : IEdge<TVertex>
        {
            var spanned = new Dictionary<TVertex, TEdge>();
            Console.WriteLine("tree:");
            foreach (var e in tree)
            {
                Console.WriteLine($"\t{e}");
                spanned[e.Source] = spanned[e.Target] = default(TEdge);
            }

            // find vertices that are connected to some edge
            var treeable = new Dictionary<TVertex, TEdge>();
            foreach (var e in g.Edges)
                treeable[e.Source] = treeable[e.Target] = e;

            // ensure they are in the tree
            foreach (var v in treeable.Keys)
                spanned.Should().ContainKey(v);
        }

        //private static double Cost<TVertex, TEdge>(IDictionary<TVertex, TEdge> tree)
        //{
        //    return tree.Count;
        //}

        //private static void AssertAreEqual<TVertex, TEdge>(
        //    IDictionary<TVertex, TEdge> left,
        //    IDictionary<TVertex, TEdge> right)
        //    where TEdge : IEdge<TVertex>
        //{
        //    try
        //    {
        //        Assert.AreEqual(left.Count, right.Count);
        //        foreach (var kv in left)
        //            Assert.AreEqual(kv.Value, right[kv.Key]);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Count: {left.Count} - {right.Count}");
        //        foreach (var kv in left)
        //        {
        //            Console.WriteLine($"{kv.Value} - {(right.TryGetValue(kv.Key, out TEdge e) ? e.ToString() : "missing")}");
        //        }

        //        throw new AssertFailedException("comparison failed", ex);
        //    }
        //}

        [Theory]
        [MemberData(nameof(UndirectedGraphs))]
        public double CompareRoot<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (var e in g.Edges)
                distances[e] = g.AdjacentDegree(e.Source) + 1;

            var prim = new List<TEdge>(g.MinimumSpanningTreePrim(e => distances[e]));
            var kruskal = new List<TEdge>(g.MinimumSpanningTreeKruskal(e => distances[e]));

            var primCost = prim.Sum(e => distances[e]);
            var kruskalCost = kruskal.Sum(e => distances[e]);
            Console.WriteLine($"prim cost: {primCost}");
            Console.WriteLine($"kruskal cost: {kruskalCost}");
            if (Math.Abs(primCost - kruskalCost) > .01)
            {
                GraphConsoleSerializer.DisplayGraph(g);
                Console.WriteLine($"prim: {string.Join(", ", Array.ConvertAll(prim.ToArray(), e => e.ToString() + ':' + distances[e]))}");
                Console.WriteLine($"krus: {string.Join(", ", Array.ConvertAll(kruskal.ToArray(), e => e.ToString() + ':' + distances[e]))}");
                Console.Write("cost do not match");
            }

            return kruskalCost;
        }

        [Fact]
        public void Prim12240()
        {
            var g = new UndirectedGraph<int, IEdge<int>>();
            g.AddVerticesAndEdge(new Edge<int>(1, 2));
            g.AddVerticesAndEdge(new Edge<int>(3, 2));
            g.AddVerticesAndEdge(new Edge<int>(3, 4));
            g.AddVerticesAndEdge(new Edge<int>(1, 4));

            var cost = CompareRoot(g);
            cost.Should().Be(9);
        }

        [Fact]
        public void Prim12240WithDelegate()
        {
            var vertices = new int[] { 1, 2, 3, 4 };
            var g = vertices.ToDelegateUndirectedGraph(
                delegate (int v, out IEnumerable<EquatableEdge<int>> ov)
                {
                    switch (v)
                    {
                        case 1: ov = new EquatableEdge<int>[] { new EquatableEdge<int>(1, 2), new EquatableEdge<int>(1, 4) }; break;
                        case 2: ov = new EquatableEdge<int>[] { new EquatableEdge<int>(1, 2), new EquatableEdge<int>(3, 1) }; break;
                        case 3: ov = new EquatableEdge<int>[] { new EquatableEdge<int>(3, 2), new EquatableEdge<int>(3, 4) }; break;
                        case 4: ov = new EquatableEdge<int>[] { new EquatableEdge<int>(1, 4), new EquatableEdge<int>(3, 4) }; break;
                        default: ov = null; break;
                    }
                    return ov != null;
                });
            var cost = CompareRoot(g);
            cost.Should().Be(9);
        }

        [Fact]
        public void Prim12273()
        {
            var ug = XmlReader.Create("GraphML/repro12273.xml").DeserializeFromXml(
                "graph", "node", "edge", "",
                reader => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                reader => reader.GetAttribute("id"),
                reader => new TaggedEdge<string, double>(
                    reader.GetAttribute("source"),
                    reader.GetAttribute("target"),
                    int.Parse(reader.GetAttribute("weight"))
                    )
                );

            var prim = ug.MinimumSpanningTreePrim(e => e.Tag).ToList();
            var pcost = prim.Sum(e => e.Tag);
            Console.WriteLine($"prim cost {pcost}");
            foreach (var e in prim)
                Console.WriteLine(e);

            var kruskal = ug.MinimumSpanningTreeKruskal(e => e.Tag).ToList();
            var kcost = kruskal.Sum(e => e.Tag);
            Console.WriteLine($"kruskal cost {kcost}");
            foreach (var e in kruskal)
                Console.WriteLine(e);

            pcost.Should().Be(63);
            pcost.Should().Be(kcost);
        }
    }
}
