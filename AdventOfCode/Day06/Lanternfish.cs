using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day06; 

static class Lanternfish {

    public static void Run() {
        int[] input = File.ReadAllText(Path.Join("Day06", "input.txt")).Split(',').Select(x => int.Parse(x)).ToArray();
        {
            Console.WriteLine("Lanternfish Part 1");
            BigInteger fishCount = SimulateFishes(input, 80);
            Console.WriteLine($"Fishies: {fishCount}\n");
        }
        {
            Console.WriteLine("Lanternfish Part 2");
            BigInteger fishCount = SimulateFishes(input, 256);
            Console.WriteLine($"Fishies: {fishCount}\n");
        }
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

        return fishByAge.Aggregate(BigInteger.Zero, (i, x) => i+x);
    }
}