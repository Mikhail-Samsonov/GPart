using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
	[MemoryDiagnoser]
	public class ListBenchmark
    {
        private static readonly Random _rnd = new Random(DateTime.Now.Millisecond);
        private HashSet<int> _data = new HashSet<int>();

		[Params(10, 50, 100, 1000, 10000)]
		public int N;

        [GlobalSetup]
        public void Setup()
        {
            for (var i = 0; i < N; i++)
            {
                _data.Add(_rnd.Next());
            }
        }

        [Benchmark]
        public void Iteration()
        {
            foreach (var n in _data)
            {
            }
        }

        [Benchmark]
        public void Contains()
        {
            for (var i = 0; i < N; i++)
            {
                _data.Contains(_rnd.Next());
            }
        }

        [Benchmark]
        public void MaxByLinq()
        {
            foreach (var n in _data)
            {
                _data.MaxBy(x => x);
            }
        }

        [Benchmark]
        public void CustomMax()
        {
            foreach (var n in _data)
            {
                Max(_data);
            }
        }

        [Benchmark]
        public void Allocation()
        {
            for (var i = 0; i < N; i++)
            {
                _data.Add(_rnd.Next());
            }
        }

        private static int Max(HashSet<int> data)
        {
            if (data.Count == 0)
            {
                throw new Exception();
            }

            var max = -1;

            foreach (var n in data)
            {
                if (n > max)
                {
                    max = n;
                }
            }

            return max;
        }
    }
}

