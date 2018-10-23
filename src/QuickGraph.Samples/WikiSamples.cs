using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph.Algorithms;
using System;
using System.Collections.Generic;

namespace QuickGraph.Samples
{
    [TestClass]
    public class WikiSamples
    {
        [TestMethod]
        public void ShortestPath()
        {
            var cities = new AdjacencyGraph<int, Edge<int>>(); // a graph of cities
            cities.AddVerticesAndEdge(new Edge<int>(0, 1));
            double cityDistances(Edge<int> e) => e.Target + e.Source; // a delegate that gives the distance between cities

            var sourceCity = 0; // starting city
            var targetCity = 1; // ending city

            // vis can create all the shortest path in the graph
            // and returns a delegate vertex -> path
            var tryGetPath = cities.ShortestPathsDijkstra(cityDistances, sourceCity);
            if (tryGetPath(targetCity, out IEnumerable<Edge<int>> path))
                foreach (var e in path)
                    Console.WriteLine(e);
        }
    }
}
