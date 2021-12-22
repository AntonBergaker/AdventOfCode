namespace AdventOfCode.Y2021.Day01;

public class SonarSweep : AocSolution<int[]> {
    public override string Name => "Sonar Sweep";

    protected override int[] ProcessInput(string input) => input.SplitLines().Select(x => int.Parse(x)).ToArray();

    protected override string Part1Implementation(int[] input) {
        int previous = input[0];
        int timesIncreased = 0;
        for (int i = 1; i < input.Length; i++) {
            int number = input[i];
            if (number > previous) {
                timesIncreased++;
            }

            previous = number;
        }

        return $"Times increased: {timesIncreased}";
    }

    protected override string Part2Implementation(int[] input) {
        int previous = input[0];
        int timesIncreased = 0;
        for (int i = 1; i < input.Length - 2; i++) {
            int number = input[i] + input[i + 1] + input[i + 2];
            if (number > previous) {
                timesIncreased++;
            }

            previous = number;
        }

        return $"Times increased: {timesIncreased}";
    }
}

