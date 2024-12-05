using InterpolatedParsing;
using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;
public class Day05 : DayLineBase<Day05.PageRules> {
    
    // OrderRules contains an array of everything that is not allowed to be printed after the key
    public record PageRules(Dictionary<int, int[]> OrderRules, int[][] Lines);

    public override PageRules Import(string[] input) {
        var dict = new Dictionary<int, List<int>>();
        var blankLineIndex = Array.FindIndex(input, x => x == "");

        foreach (var line in input.Take(blankLineIndex)) {
            int lhs = 0, rhs = 0;
            InterpolatedParser.Parse(line, $"{lhs}|{rhs}");

            // Get list or create if doesn't exist
            if (dict.TryGetValue(lhs, out var list) == false) {
                dict[lhs] = list = new();
            }
            list.Add(rhs);
        }


        var lines = input.Skip(blankLineIndex + 1).Select(
            line => line.Split(',').Select(
                number => int.Parse(number)
            ).ToArray()
        ).ToArray();

        return new(dict.ToDictionary(x => x.Key, x => x.Value.ToArray()), lines);
    }

    public override string Part1(PageRules input) {
        var total = 0;
        foreach (var line in input.Lines) {
            if (LineIsCorrect(input.OrderRules, line)) {
                total += line[line.Length / 2];
            }
        }

        return $"Sum of middle number on correctly ordered lists: {total.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(PageRules input) {
        var total = 0;
        foreach (var sourceLine in input.Lines) {
            if (LineIsCorrect(input.OrderRules, sourceLine)) {
                continue;
            }

            // Make a copy of the array, dont want to taint the input
            var line = sourceLine.ToArray();

            // Flip until it is correct
            while (LineIsCorrect(input.OrderRules, line) == false) {
                // key: number, value: position
                var dict = new Dictionary<int, int>();

                for (int i = 0; i < line.Length; i++) {
                    var number = line[i];

                    var cantBeBefore = input.OrderRules.GetValueOrDefault(number, []);
                    foreach (var otherNumber in cantBeBefore) {
                        if (dict.TryGetValue(otherNumber, out int othersIndex) == false) {
                            continue;
                        }

                        // Flip their positions if match
                        (line[i], line[othersIndex]) = (line[othersIndex], line[i]);

                        // Could probably continue without using an outer break with proper cleanup in the dict, but this is good enough
                        goto outer_break;
                    }

                    dict.Add(number, i);
                }
                outer_break: ;
            }

            total += line[line.Length / 2];
        }

        return $"Sum of middle number on fixed lists: {total.ToString().Pastel(Color.Yellow)}";
    }

    private static bool LineIsCorrect(Dictionary<int, int[]> orderRules, int[] line) {
        var alreadyPrinted = new HashSet<int>();

        foreach (var number in line) {
            var cantBeBefore = orderRules.GetValueOrDefault(number, []);
            if (cantBeBefore.Any(x => alreadyPrinted.Contains(x))) {
                return false;
            }
            alreadyPrinted.Add(number);
        }

        return true;
    }
}
