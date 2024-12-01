using AdventOfCode2024.Day1;
using FluentAssertions;

namespace AdventOfCode2024Tests;

public class DayOneTests
{
    [Fact]
    public void TestDayOne()
    {
        var leftInput = new int[] { 3, 4, 2, 1, 3, 3 };
        var rightInput = new int[] { 4, 3, 5, 3, 9, 3 };

        var subject = new AoCDayOne();
        var result = subject.GetTotalDistance(leftInput, rightInput);
        result.Should().Be(11);
    }

    [Fact]
    public void TestDayOnePartTwo()
    {
        var leftInput = new int[] { 3, 4, 2, 1, 3, 3 };
        var rightInput = new int[] { 4, 3, 5, 3, 9, 3 };

        var subject = new AoCDayOne();
        var result = subject.GetSimilarityScore(leftInput, rightInput);
        result.Should().Be(31);
    }
}