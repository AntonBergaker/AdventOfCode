
using Pastel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;
public class Day03 : DayTextBase<string> {
    public override string Import(string input) {
        return input;
    }

    public override string Part1(string input) {
        var regex = new Regex("""mul\(\d+\,\d+\)""");

        var total = 0;
        foreach (Match match in regex.Matches(input)) {
            total += EvaluateMultiplication(match.Value);
        }

        return $"Sum of multiplications: {total.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(string input) {
        var regex = new Regex("""(mul\(\d+\,\d+\))|(don't\(\))|(do\(\))""");

        var total = 0;
        bool enabled = true;
        foreach (Match match in regex.Matches(input)) {
            var text = match.Value;
            if (text.StartsWith("mul")) {
                if (enabled) {
                    total += EvaluateMultiplication(text);
                }
            } else if (text.StartsWith("don't")) {
                enabled = false;
            } else if (text.StartsWith("do")) {
                enabled = true;
            }
        }

        return $"Sum of enabled multiplications: {total.ToString().Pastel(Color.Yellow)}";
    }

    private static int EvaluateMultiplication(string text) {
        int lhs = 0, rhs = 0;
        InterpolatedParsing.InterpolatedParser.Parse(text, $"mul({lhs},{rhs})");
        var sum = lhs * rhs;
        return sum;
    }
}
