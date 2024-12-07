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
            if (TestOperationsOnLine(line, operations, out var value)) {
                total += value;
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
            if (TestOperationsOnLine(line, operations, out var value)) {
                total += value;
            }
        }

        return $"Sum of lines that can be correctly assembled: {total.ToString().Pastel(Color.Yellow)}";
    }
    private static bool TestOperationsOnLine(OperatorLine line, BinaryOperation[] operations, out long value) {
        int operatorCount = line.Numbers.Length - 1;
        var iterationCount = (int)Math.Pow(operations.Length, operatorCount);

        for (int iteration = 0; iteration < iterationCount; iteration++) {
            long testValue = line.Numbers[0];
            for (int i = 0; i < operatorCount; i++) {
                // Get the operator to use at this index.
                int operatorIndex = (iteration / (int)Math.Pow(operations.Length, i)) % operations.Length;
                testValue = operations[operatorIndex](testValue, line.Numbers[i + 1]);
            }

            if (testValue == line.Total) {
                value = line.Total;
                return true;
            }
        }

        value = default;
        return false;
    }
}
