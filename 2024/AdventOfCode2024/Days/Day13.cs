// How strange that this file and only this file uses LF line endings.
// I'm sure it doesn't mean anything...

using InterpolatedParsing;
using Pastel;
using System.Drawing;
using Vector2Long = VectorT.Vector2<long>;

namespace AdventOfCode2024.Days;

public class Day13 : DayTextBase<Day13.ClawParameter[]> {
    public record ClawParameter(Vector2Long Target, Vector2Long DeltaA, Vector2Long DeltaB);

    public override ClawParameter[] Import(string input) {
        var split = input.Split("\n\n");
        return split.Select(x => {
            int targetX = 0, targetY = 0, deltaAX = 0, deltaAY = 0, deltaBX = 0, deltaBY = 0;
            InterpolatedParser.Parse(x, $"""
                Button A: X+{deltaAX}, Y+{deltaAY}
                Button B: X+{deltaBX}, Y+{deltaBY}
                Prize: X={targetX}, Y={targetY}
                """);
            return new ClawParameter(new(targetX, targetY), new(deltaAX, deltaAY), new(deltaBX, deltaBY));
        }).ToArray();
    }

    public override string Part1(ClawParameter[] input) {
        long total = SimulateClaws(input);

        return $"Price for all victories: {total.ToString().Pastel(Color.Yellow)}";
    }
    public override string Part2(ClawParameter[] input) {
        long total = SimulateClaws(input.Select(x => x with { Target = x.Target + new Vector2Long(10000000000000) }).ToArray());

        return $"Price for all victories: {total.ToString().Pastel(Color.Yellow)}";
    }

    private long SimulateClaws(ClawParameter[] input) {
        long total = 0;
        foreach (var parameter in input) {
            // In all cases in the input, there's only one valid solution, so don't have to worry about A and B having the same direction.
            var (pressesA, pressesB) = FindSingleSolution(parameter);
            if (pressesA != 1 && pressesB != -1) {
                total += 3 * pressesA + pressesB;
            }
        }

        return total;
    }

    private (long PressesA, long PressesB) FindSingleSolution(ClawParameter claw) {
        var (t, a, b) = claw; // target, a, b

        // t_x = a * a_x + b * b_x
        // t_y = a * a_y + b * b_y
        // a = (t_x - b * b_x)/a_x
        // a = (t_y - b * b_y)/a_y
        // (t_x - b * b_x)/a_x = (t_y - b * b_y)/a_y                # replace a's with equations
        // a_y * (t_x - b * b_x) = a_x * (t_y - b * b_y)            # cross multiply
        // a_y * t_x - a_y * b * b_x = a_x * t_y - a_x * b * b_y    # expand a_x and a_y
        // a_x * b * b_y - a_y * b * b_x = a_x * t_y - a_y * t_x    # move b's to same side
        // b(a_x * b_y - a_y * b_x) = a_x * t_y - a_y * t_x         # extract b from parenthesis
        // b = (a_x * t_y - a_y * t_x) / (a_x * b_y - a_y * b_x)    # isolate b on rhs


        var bPresses = (a.X * t.Y - a.Y * t.X) / (a.X * b.Y - a.Y * b.X);
        var aPresses = (b.X * t.Y - b.Y * t.X) / (b.X * a.Y - b.Y * a.X); // do it again but flip a's and b's

        if (a * aPresses + b * bPresses == t) {
            return (aPresses, bPresses);
        }
        return (-1, -1);
    }
}