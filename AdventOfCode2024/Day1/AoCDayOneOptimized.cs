using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day1;

public class AoCDayOneOptimized : IAoCDayOne
{
    private readonly bool? _forceVectorized;

    public AoCDayOneOptimized(bool? forceVectorized = null)
    {
        _forceVectorized = forceVectorized;
    }
    
    public static void RunFromConsole()
    {
        var input = Console.ReadLine();
        var lines = new List<string>();
        while (!string.IsNullOrEmpty(input))
        {
            lines.Add(input);
            input = Console.ReadLine();
        }

        var leftList = new List<int>(lines.Count);
        var rightList = new List<int>(lines.Count);

        foreach (var line in lines)
        {
            var match = Regex.Match(line, @"(?<left>\d+)\s+(?<right>\d+)");
            var left = int.Parse(match.Groups["left"].Value);
            var right = int.Parse(match.Groups["right"].Value);
            leftList.Add(left);
            rightList.Add(right);
        }

        var obj = new AoCDayOne();
        Console.WriteLine("Distance:");
        var totalDist = obj.GetTotalDistance(leftList, rightList);
        Console.WriteLine(totalDist);
        
        Console.WriteLine("Similarity score:");
        var totalSim = obj.GetSimilarityScore(leftList, rightList);
        Console.WriteLine(totalSim);
    }

    public async Task<int> GetTotalDistanceAsync(IReadOnlyList<int> leftList, IReadOnlyList<int> rightList)
    {
        var leftSortTask = Task.Run(() =>
        {
            var sortedLeft = new List<int>(leftList);
            sortedLeft.Sort();
            return sortedLeft;
        });
        var rightSortTask = Task.Run(() =>
        {
            var sortedRight = new List<int>(rightList);
            sortedRight.Sort();
            return sortedRight;
        });
        
        await Task.WhenAll(leftSortTask, rightSortTask);
        var sortedLeft = leftSortTask.Result;
        var sortedRight = rightSortTask.Result;

        if (_forceVectorized ?? Vector512.IsHardwareAccelerated)
        {
            return VectorizedTotalDistance(sortedLeft, sortedRight);
        }
        else
        {
            return ParallelizedTotalDistance(sortedLeft, sortedRight);
        }
    }


    private static int VectorizedTotalDistance(List<int> sortedLeft, List<int> sortedRight)
    {
        var intsPerVector = Vector512<int>.Count;
        var leftSpan = CollectionsMarshal.AsSpan(sortedLeft);
        var rightSpan = CollectionsMarshal.AsSpan(sortedRight);
    
        var totalVector = Vector512<int>.Zero;

        var vectorCount = sortedLeft.Count / intsPerVector;
        for (int i = 0; i < vectorCount; i++)
        {
            var leftVector = Vector512.LoadUnsafe(ref leftSpan[i * intsPerVector]);
            var rightVector = Vector512.LoadUnsafe(ref rightSpan[i * intsPerVector]);
        
            var diff = Vector512.Subtract(leftVector, rightVector);
            var abs = Vector512.Abs(diff);
        
            totalVector = Vector512.Add(totalVector, abs);
        }

        var totalDist = Vector512.Sum(totalVector);

        var resumeIndex = vectorCount * intsPerVector;
        for (int i = resumeIndex; i < sortedLeft.Count; i++)
        {
            totalDist += Math.Abs(sortedLeft[i] - sortedRight[i]);
        }

        return totalDist;
    }

    private static int ParallelizedTotalDistance(List<int> sortedLeft, List<int> sortedRight)
    {
        var total = 0;
        Parallel.For(0, sortedLeft.Count, 
            // Initialize each thread's local counter
            () => 0,
            // Accumulate in the local counter
            (i, loop, localTotal) => 
                localTotal + Math.Abs(sortedLeft[i] - sortedRight[i]),
            // Final step: add local counter to total
            localTotal => Interlocked.Add(ref total, localTotal)
        );
        return total;
    }

    public int GetSimilarityScore(IReadOnlyList<int> leftList, IReadOnlyList<int> rightList)
    {
        var rightCounts = new Dictionary<int, int>();
        foreach (var value in rightList)
        {
            rightCounts.TryAdd(value, 0);
            rightCounts[value]++;
        }
        
        var similarity = 0;
        foreach (var value in leftList)
        {
            if (!rightCounts.TryGetValue(value, out var count))
                continue;

            similarity += value * count;
        }

        return similarity;
    }
}