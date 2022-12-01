using System.Numerics;

namespace AdventOfCode.Y2021.Day14;

public class ExtendedPolymerization : AocSolution<(string startTemplate, Dictionary<string, char> pairInsertionRules)> {
    public override string Name => "Extended Polymerization";

    protected override (string startTemplate, Dictionary<string, char> pairInsertionRules) ProcessInput(string input) {
        string[] lines = input.SplitLines();

        string startTemplate = lines[0];
        Dictionary<string, char> pairInsertionRules = lines[2..].Select(x => x.Split("->", StringSplitOptions.TrimEntries)).ToDictionary(x => x[0], x => x[1][0]);
        return (startTemplate, pairInsertionRules);
    }

    protected override string Part1Implementation((string startTemplate, Dictionary<string, char> pairInsertionRules) input) {
        var polymerCount = GetCountOfEachPolymer(input.startTemplate, input.pairInsertionRules, 10);
        var sortedCount = polymerCount.OrderBy(x => x.Value).ToArray();

        return $"Difference between most common and least common: {sortedCount[^1].Value - sortedCount[0].Value}";
    }

    protected override string Part2Implementation((string startTemplate, Dictionary<string, char> pairInsertionRules) input) {
        var polymerCount = GetCountOfEachPolymer(input.startTemplate, input.pairInsertionRules, 40);
        var sortedCount = polymerCount.OrderBy(x => x.Value).ToArray();

        return $"Difference between most common and least common: {sortedCount[^1].Value - sortedCount[0].Value}";
    }

    private static Dictionary<char, BigInteger> GetCountOfEachPolymer(string input, Dictionary<string, char> pairInsertionRules, int repeats) {

        Dictionary<string, BigInteger> pairCount = pairInsertionRules.ToDictionary(x => x.Key, _ => BigInteger.Zero);
        Dictionary<char, BigInteger> itemCount = pairCount.SelectMany(x => x.Key).Distinct().ToDictionary(x => x, _ => BigInteger.Zero);
        for (int i = 0; i < input.Length - 1; i++) {
            pairCount[input[i..(i + 2)]]++;
        }
        foreach (char c in input) {
            itemCount[c]++;
        }

        for (int repetitions = 0; repetitions < repeats; repetitions++) {
            Dictionary<string, BigInteger> newPairCount = new();
            foreach (string pair in pairCount.Keys) {
                char newChar = pairInsertionRules[pair];
                BigInteger count = pairCount[pair];

                itemCount[newChar] += count;
                string new0 = $"{pair[0]}{newChar}";
                string new1 = $"{newChar}{pair[1]}";

                newPairCount[new0] = newPairCount.GetValueOrDefault(new0) + count;
                newPairCount[new1] = newPairCount.GetValueOrDefault(new1) + count;
            }

            pairCount = newPairCount;
        }

        return itemCount;
    }
}