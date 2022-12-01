namespace AdventOfCode.Y2021.Day07;

public class TheTreacheryOfWhales : AocSolution<int[]> {
    public override string Name => "The Treachery of Whales";

    protected override int[] ProcessInput(string input) => input.Split(',').Select(x => int.Parse(x)).ToArray();

    protected override string Part1Implementation(int[] input) {
        var bestFuelUsage = GetBestFuelUsage(input, (x0, x1) => Math.Abs(x0 - x1));

        return $"Min Fuel Usage: {bestFuelUsage}";
    }

    protected override string Part2Implementation(int[] input) {
        var bestFuelUsage = GetBestFuelUsage(input, (x0, x1) => {
            int diff = Math.Abs(x0 - x1);
            return (diff * (diff + 1)) / 2;
        });

        return $"Min Fuel Usage: {bestFuelUsage}";
    }

    private static int GetBestFuelUsage(int[] input, Func<int, int, int> calculateFuelUsage) {
        int minW = input.Min();
        int maxW = input.Max();
        int bestFuelUsage = int.MaxValue;

        for (int x = minW; x <= maxW; x++) {
            int fuelUsage = input.Sum(value => calculateFuelUsage(x, value));

            if (fuelUsage < bestFuelUsage) {
                bestFuelUsage = fuelUsage;
            }
        }

        return bestFuelUsage;
    }
}