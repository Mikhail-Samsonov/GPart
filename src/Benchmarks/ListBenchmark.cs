using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
	[MemoryDiagnoser]
	public class HashSetBenchmark
	{
        private static readonly Random _rnd = new Random(DateTime.Now.Millisecond);
        private List<int> _data = new List<int>();

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
            for (var i = 0; i < _data.Count; i++)
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
        public void CustomMax()
        {
            for (var i = 0; i < _data.Count; i++)
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

        private static int Max(List<int> list)
        {
            if (list.Count == 0)
            {
                throw new Exception();
            }

            var max = list[0];

            if (list.Count > 1)
            {
                for (var i = 1; i < list.Count; i++)
                {
                    if (list[i] > max)
                    {
                        max = list[i];
                    }
                }
            }

            return max;
        }
    }
}

