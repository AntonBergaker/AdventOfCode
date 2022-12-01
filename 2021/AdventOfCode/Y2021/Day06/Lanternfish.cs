using System.Numerics;

namespace AdventOfCode.Y2021.Day06;

public class Lanternfish : AocSolution<int[]> {
    public override string Name => "Lanternfish";

    protected override int[] ProcessInput(string input) => input.Split(',').Select(x => int.Parse(x)).ToArray();

    protected override string Part1Implementation(int[] input) {
        BigInteger fishCount = SimulateFishes(input, 80);
        return $"Fishies: {fishCount}";
    }

    protected override string Part2Implementation(int[] input) {
        BigInteger fishCount = SimulateFishes(input, 256);
        return $"Fishies: {fishCount}";
    }


    private static BigInteger SimulateFishes(int[] input, int days) {
        int maxAge = 9;
        BigInteger[] fishByAge = new BigInteger[maxAge];
        foreach (int age in input) {
            fishByAge[age]++;
        }

        for (int repetitions = 0; repetitions < days; repetitions++) {
            BigInteger birthingFish = fishByAge[0];
            // Deage all fish
            for (int i = 1; i < maxAge; i++) {
                fishByAge[i - 1] = fishByAge[i];
            }

            // It's the ciiiiiiircle of life
            fishByAge[6] += birthingFish;
            fishByAge[8] = birthingFish;
        }

        return fishByAge.Aggregate(BigInteger.Zero, (i, x) => i + x);
    }
}