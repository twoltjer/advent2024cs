using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day1;

public interface IAoCDayOne
{
    Task<int> GetTotalDistanceAsync(IReadOnlyList<int> leftList, IReadOnlyList<int> rightList);
}

public class AoCDayOne : IAoCDayOne
{
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

    public int GetTotalDistance(IReadOnlyList<int> leftList, IReadOnlyList<int> rightList)
    {
        var sortedLeft = new List<int>(leftList);
        sortedLeft.Sort();
        var sortedRight = new List<int>(rightList);
        sortedRight.Sort();
        var totalDist = 0;
        for (int i = 0; i < sortedLeft.Count; i++)
        {
            totalDist += Math.Abs(sortedLeft[i] - sortedRight[i]);
        }

        return totalDist;
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

    public Task<int> GetTotalDistanceAsync(IReadOnlyList<int> leftList, IReadOnlyList<int> rightList)
    {
        var result = GetTotalDistance(leftList, rightList);
        return Task.FromResult(result);
    }
}