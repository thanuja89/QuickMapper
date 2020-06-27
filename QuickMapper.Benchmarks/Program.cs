using BenchmarkDotNet.Running;

namespace QuickMapper.Benchmarks
{
    class Program
    {
        private static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
