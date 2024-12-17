using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;
internal class Day02 : DayLineBase<int[][]> {
    public override int[][] Import(string[] input) {
        return input.Select(x => x.Split(" ").Select(x => int.Parse(x)).ToArray()).ToArray();
    }

    public override string Part1(int[][] input) {
        var safeLineCount = input.Count(x => IsSafe(x, 0));
        return $"Number of safe reports: {safeLineCount.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(int[][] input) {
        var safeLineCount = 0;
        foreach (var line in input) {
            var safe =
                IsSafe(line, 1) || // Check if removing any number except first is okay 
                IsSafe(line[1..], 0); // Check if explicitly removing first number is okay.

            if (safe) {
                safeLineCount++;
            }
            //Console.WriteLine(string.Join(" ", line) + (safe ? " safe".Pastel(Color.Green) : " unsafe".Pastel(Color.Red)));
        }
        return $"Number of safe reports with dampener: {safeLineCount.ToString().Pastel(Color.Yellow)}";
    }

    /// <summary>
    /// Checks if a line is safe. The allowedRemovals parameter can be used to allow numbers to be skipped, except the first number.
    /// </summary>
    /// <param name="line"></param>
    /// <param name="allowedRemovals">Amount of number removals to pass as a safe number. Does not respect the first number.</param>
    /// <returns></returns>
    private static bool IsSafe(int[] line, int allowedRemovals) {
        int growthDirection = Math.Sign(line[0] - line[1]);

        int last = line[0];
        for (int i = 1; i < line.Length; i++) {
            var value = line[i];
            var direction = Math.Sign(last - value);
            if (direction != growthDirection) {
                allowedRemovals--;
                continue;
            }
            var difference = Math.Abs(last - value);
            if (difference > 3) {
                allowedRemovals--;
                continue;
            }
            last = value;
        }

        return allowedRemovals >= 0;
    }
}
