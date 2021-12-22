using VectorInt;

namespace AdventOfCode.Y2021.Day05;

public class HydrothermalVenture : AocSolution<List<HydrothermalVenture.Line>> {
    public override string Name => "Hydrothermal Venture";

    public record Line(VectorInt2 P0, VectorInt2 P1) {
        public VectorInt2 Length => P1 - P0;

        public int Magnitude => Math.Max(Math.Abs(Length.X), Math.Abs(Length.Y));

        public VectorInt2 Direction => Length / new VectorInt2(Magnitude);
    }

    protected override List<Line> ProcessInput(string input) {
        return input.SplitLines().Select(x => {
            int[] numbers = x.Split(new[] { ",", "->" },
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(y => int.Parse(y)).ToArray();
            return new Line(new(numbers[0], numbers[1]), new(numbers[2], numbers[3]));
        }).ToList();
    }
    private static int GetMaxSize(List<Line> lines) {
        return lines.SelectMany(x => new[] { x.P0.X, x.P0.Y, x.P1.X, x.P1.Y }).Max() + 1;
    }

    protected override string Part1Implementation(List<Line> lines) {
        int result = CalculateDangerAreas(lines, x => x.Direction.IsUnit);
        return $"Danger points: {result}";
    }

    protected override string Part2Implementation(List<Line> lines) {
        int result = CalculateDangerAreas(lines, _ => true);
        return $"Danger points: {result}";
    }

    private static int CalculateDangerAreas(List<Line> lines, Func<Line, bool> shouldKeepLineFunc) {
        int gridSize = GetMaxSize(lines);
        Grid<int> grid = new(gridSize, gridSize);

        foreach (Line line in lines) {
            if (shouldKeepLineFunc(line) == false) {
                continue;
            }

            VectorInt2 direction = line.Direction;
            VectorInt2 p = line.P0;
            int magnitude = line.Magnitude;

            for (int i = 0; i <= magnitude; i++) {
                grid[p]++;
                p += direction;
            }
        }

        return grid.Count(number => number >= 2);
    }
}