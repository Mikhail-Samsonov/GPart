using BenchmarkDotNet.Running;

namespace Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(typeof(Program).Assembly);
            switcher.Run(args);
        }
    }
}