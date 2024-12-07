using InterpolatedParsing;
using Pastel;
using System.Drawing;
using BinaryOperation = System.Func<long, long, long>;

namespace AdventOfCode2024.Days;
public class Day07 : DayLineBase<Day07.OperatorLine[]> {
    public record OperatorLine(long Total, int[] Numbers);

    public override OperatorLine[] Import(string[] input) {
        var list = new List<OperatorLine>();
        foreach (var line in input) {
            long total = 0;
            int[] numbers = null!;

            InterpolatedParser.Parse(line, $"{total}: {numbers:' '}");
            list.Add(new(total, numbers));
        }
        return list.ToArray();
    }

    public override string Part1(OperatorLine[] input) {
        BinaryOperation[] operations = [
            (lhs, rhs) => lhs + rhs, 
            (lhs, rhs) => lhs * rhs
        ];

        long total = 0;
        foreach (var line in input) {
            if (TestOperationsOnLine(line, operations)) {
                total += line.Total;
            }
        }

        return $"Sum of lines that can be correctly assembled: {total.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(OperatorLine[] input) {
        BinaryOperation[] operations = [
            (lhs, rhs) => lhs + rhs, 
            (lhs, rhs) => lhs * rhs,
            // Concat numbers, raise lhs to add zeros to match the rhs number count.
            (lhs, rhs) => (int)Math.Pow(10, Math.Floor(Math.Log10(rhs)+1)) * lhs + rhs,
        ];

        long total = 0;
        foreach (var line in input) {
            if (TestOperationsOnLine(line, operations)) {
                total += line.Total;
            }
        }

        return $"Sum of lines that can be correctly assembled: {total.ToString().Pastel(Color.Yellow)}";
    }
    private static bool TestOperationsOnLine(OperatorLine line, BinaryOperation[] operations) {
        int operatorCount = line.Numbers.Length - 1;

        return TestRemainingOperations(1, line.Numbers[0]); ;

        bool TestRemainingOperations(int index, long total) {
            if (index > operatorCount) {
                return total == line.Total;
            }
            var number = line.Numbers[index];

            for (int i = 0; i < operations.Length; i++) {
                var newTotal = operations[i](total, number);

                if (newTotal > line.Total) {
                    continue;
                }

                if (TestRemainingOperations(index + 1, newTotal)) {
                    return true;
                }
            }
            return false;
        }
    }
}
