using FluentAssertions;
using QuickGraph.Algorithms.AssigmentProblem;
using Xunit;

namespace QuickGraph.Tests.Algorithms.AssigmentProblem
{
    public class HungarianAlgorithmTest
    {
        [Fact]
        public void RunCheck()
        {
            var matrix = new[,] { { 1, 2, 3 }, { 3, 3, 3 }, { 3, 3, 2 } };
            var algorithm = new HungarianAlgorithm(matrix);
            var res = algorithm.Run();
            res.Should().HaveElementAt(0, 0);
            res.Should().HaveElementAt(1, 1);
            res.Should().HaveElementAt(2, 2);
        }

        [Fact]
        public void IterationsCheck()
        {
            var matrix = new[,] { { 1, 2, 3 }, { 3, 3, 3 }, { 3, 3, 2 } };
            var algorithm = new HungarianAlgorithm(matrix);
            var iterations = algorithm.GetIterations();
            var res = algorithm.AgentsTasks;
            res.Should().HaveElementAt(0, 0);
            res.Should().HaveElementAt(1, 1);
            res.Should().HaveElementAt(2, 2);
            iterations.Should().HaveCount(3);
        }
    }
}
