using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace AdventOfCode2024Benchmarks;

public class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<DayOneBenchmarks>();
    }
}