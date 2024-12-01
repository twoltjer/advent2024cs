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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task TestDayOneAsync(bool vectorized)
    {
        var leftInput = new int[] { 3, 4, 2, 1, 3, 3 };
        var rightInput = new int[] { 4, 3, 5, 3, 9, 3 };

        var subject = new AoCDayOneOptimized(forceVectorized: vectorized);
        var result = await subject.GetTotalDistanceAsync(leftInput, rightInput);
        result.Should().Be(11); 
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public async Task TestDayOneWithLargeDataset(bool optimized, bool vectorized)
    {
        var random = new Random(0);
        var leftInput = Enumerable.Repeat(0, 20_000).Select(_ => random.Next(16384)).ToList();
        var rightInput = Enumerable.Repeat(0, 20_000).Select(_ => random.Next(16384)).ToList();
        IAoCDayOne subject = optimized
            ? new AoCDayOneOptimized(forceVectorized: vectorized)
            : new AoCDayOne();
        
        var result = await subject.GetTotalDistanceAsync(leftInput, rightInput);
        result.Should().Be(2122216);
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