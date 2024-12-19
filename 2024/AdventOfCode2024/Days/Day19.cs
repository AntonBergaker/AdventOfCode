using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;
public class Day19 : DayLineBase<(string[] Towels, string[] Designs)> {
    public override (string[] Towels, string[] Designs) Import(string[] input) {
        var towels = input[0].Split(",", StringSplitOptions.TrimEntries);
        var designs = input[2..];
        return (towels, designs);
    }

    public override string Part1((string[] Towels, string[] Designs) input) {
        var (towels, designs) = input;
        var total = 0;
        foreach (var design in designs) {
            if (GetNumberOfValidDesigns(design, towels) > 0) {
                total++;
            }
        }

        return $"Patterns we can build with towels: {total.ToString().Pastel(Color.Yellow)}";
    }

    private static long GetNumberOfValidDesigns(string design, string[] towels) {
        // Array of what parts of the design have valid combos
        var validCombinations = new long[design.Length + 1];
        // 0 length always has a valid combination, nothing
        validCombinations[0] = 1;
        for (int i = 1; i <= design.Length; i++) {
            var subDesign = design.AsSpan(0, i);
            long numberOfValidAdds = 0;

            foreach (var towel in towels) {
                // if it fits
                if (towel.Length > i) {
                    continue;
                }

                // check if adding length to a previous pattern is ok
                var validSoFar = validCombinations[i - towel.Length];
                if (validSoFar == 0) {
                    continue;
                }

                // check if pattern matches
                if (subDesign[^towel.Length..].SequenceEqual(towel)) {
                    continue;
                }

                numberOfValidAdds += validSoFar;
            }

            validCombinations[i] = numberOfValidAdds;
        }

        return validCombinations[^1];
    }

    public override string Part2((string[] Towels, string[] Designs) input) {
        var (towels, designs) = input;
        long total = 0;
        foreach (var design in designs) {
            total += GetNumberOfValidDesigns(design, towels);
        }

        return $"Sum of possible pattern combinations: {total.ToString().Pastel(Color.Yellow)}";
    }
}
