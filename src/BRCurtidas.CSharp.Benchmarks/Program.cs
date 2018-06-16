using BenchmarkDotNet.Running;

namespace BRCurtidas.CSharp.Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<DomainBenchmarks>();
        }
    }
}