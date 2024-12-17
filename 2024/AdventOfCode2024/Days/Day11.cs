using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;
internal class Day11 : DayTextBase<long[]> {
    public override long[] Import(string input) {
        return input.Split(' ').Select(x => long.Parse(x)).ToArray();
    }

    public override string Part1(long[] input) {
        return $"Total number of stones after 25 blinks: {SimulateStones(input, 25).ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(long[] input) {
        return $"Total number of stones after 75 blinks: {SimulateStones(input, 75).ToString().Pastel(Color.Yellow)}";
    }

    /// <summary>
    /// Run the stone simulation a certain number of times
    /// </summary>
    /// <param name="input"></param>
    /// <param name="numberOfSimulations"></param>
    /// <returns>Number of stones at end of simulation</returns>
    private static long SimulateStones(long[] input, int numberOfSimulations) {
        var stones = new Dictionary<long, long>(); // Key is number on stone, value is number of stones
        foreach (var value in input) {
            AddToDictionary(stones, value, 1);
        }

        for (int i = 0; i < numberOfSimulations; i++) {
            var newStones = new Dictionary<long, long>();
            foreach (var (value, count) in stones) {
                if (value == 0) {
                    AddToDictionary(newStones, 1, count);
                    continue;
                }
                var numOfDigits = GetNumberOfDigits(value);
                if (numOfDigits % 2 == 0) {
                    var exponent = (long)Math.Floor(Math.Pow(10, numOfDigits / 2));
                    AddToDictionary(newStones, value / exponent, count);
                    AddToDictionary(newStones, value % exponent, count);
                    continue;
                }
                AddToDictionary(newStones, value*2024, count);
            }
            stones = newStones;
        }

        static void AddToDictionary(Dictionary<long, long> dictionary, long key, long value) {
            dictionary[key] = value + dictionary.GetValueOrDefault(key);
        }

        return stones.Values.Sum();
    }

    private static int GetNumberOfDigits(long number) {
        return (int)Math.Floor(Math.Log10(number) + 1);
    }
}
