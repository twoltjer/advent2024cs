using AdventOfCode2024.Day1;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode2024Benchmarks
{
    [MemoryDiagnoser]
    public class DayOneBenchmarks
    {
        private List<int> _leftList = null!;
        private List<int> _rightList = null!;
        private AoCDayOne _baseline = null!;
        private AoCDayOneOptimized _vectorized = null!;
        private AoCDayOneOptimized _parallelized = null!;

        [Params(100, 1000, 10_000, 100_000, 1_000_000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(42); // Fixed seed for reproducibility
            _leftList = Enumerable.Range(0, N)
                .Select(_ => random.Next(16384))
                .ToList();
            _rightList = Enumerable.Range(0, N)
                .Select(_ => random.Next(16384))
                .ToList();

            _baseline = new AoCDayOne();
            _vectorized = new AoCDayOneOptimized(forceVectorized: true);
            _parallelized = new AoCDayOneOptimized(forceVectorized: false);
        }

        [Benchmark(Baseline = true)]
        public async Task<int> Baseline() =>
            await _baseline.GetTotalDistanceAsync(_leftList, _rightList);

        [Benchmark]
        public async Task<int> Vectorized() =>
            await _vectorized.GetTotalDistanceAsync(_leftList, _rightList);

        [Benchmark]
        public async Task<int> Parallelized() =>
            await _parallelized.GetTotalDistanceAsync(_leftList, _rightList);
    }

}