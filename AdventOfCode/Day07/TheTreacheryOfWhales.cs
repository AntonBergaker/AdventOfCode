using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day07; 

static class TheTreacheryOfWhales {
    public static void Run() {
        int[] input = File.ReadAllText(Path.Join("Day07", "input.txt")).Split(',').Select(x => int.Parse(x)).ToArray();
        {
            Console.WriteLine("The Treachery of Whales Part 1");

            var bestFuelUsage = GetBestFuelUsage(input, (x0, x1) => Math.Abs(x0 - x1));

            Console.WriteLine($"Min Fuel Usage: {bestFuelUsage}\n");
        }
        {
            Console.WriteLine("The Treachery of Whales Part 2");

            var bestFuelUsage = GetBestFuelUsage(input, (x0, x1) => {
                    int diff = Math.Abs(x0 - x1);
                    return (diff * (diff + 1)) / 2;
                }
            );

            Console.WriteLine($"Min Fuel Usage: {bestFuelUsage}\n");
        }
    }

    private static int GetBestFuelUsage(int[] input, Func<int,int,int> calculateFuelUsage) {
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