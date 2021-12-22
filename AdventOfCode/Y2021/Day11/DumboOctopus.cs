using VectorInt;
namespace AdventOfCode.Y2021.Day11;

public class DumboOctopus : AocSolution<Grid<int>> {
    public override string Name => "Dumbo Octopus";

    protected override Grid<int> ProcessInput(string input) {
        string[] lines = input.SplitLines();
        int width = lines[0].Length;
        int height = lines.Length;
        Grid<int> data = new(width, height);
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                data[x, y] = int.Parse(lines[y][x].ToString());
            }
        }

        return data;
    }

    protected override string Part1Implementation(Grid<int> input) {
        int totalFlashes = 0;
        for (int i = 0; i < 100; i++) {
            totalFlashes += SimulateOctopuses(input);
        }

        return $"Total flashes: {totalFlashes}";
    }

    protected override string Part2Implementation(Grid<int> input) {
        // simulate a long time, hopefully breaks earlier than this
        int allFlashAt = -1;
        for (int i = 0; i < 100_000; i++) {
            SimulateOctopuses(input);

            if (input.All(x => x == 0)) {
                allFlashAt = i + 1;
                break;
            }
        }

        return $"Round of total flash: {allFlashAt}";
    }

    private static int SimulateOctopuses(Grid<int> data) {
        int totalFlashes = 0;

        HashSet<VectorInt2> flashedThisStep = new();

        void IncrementOctopusAt(VectorInt2 pos) {
            data[pos] += 1;
            if (data[pos] == 10) {
                totalFlashes++;
                flashedThisStep.Add(pos);
                var nearby = GetNearbyCoords(pos, data.Width, data.Height);
                foreach (VectorInt2 near in nearby) {
                    IncrementOctopusAt(near);
                }
            }
        }


        flashedThisStep.Clear();
        for (int y = 0; y < data.Height; y++) {
            for (int x = 0; x < data.Width; x++) {
                IncrementOctopusAt(new(x, y));
            }
        }

        foreach (VectorInt2 pos in flashedThisStep) {
            data[pos] = 0;
        }

        return totalFlashes;
    }

    private static List<VectorInt2> GetNearbyCoords(VectorInt2 pos, int width, int height) {
        List<VectorInt2> list = new();
        int x0 = Math.Clamp(pos.X - 1, 0, width - 1);
        int x1 = Math.Clamp(pos.X + 1, 0, width - 1);
        int y0 = Math.Clamp(pos.Y - 1, 0, height - 1);
        int y1 = Math.Clamp(pos.Y + 1, 0, height - 1);

        for (int x = x0; x <= x1; x++) {
            for (int y = y0; y <= y1; y++) {
                if (x == pos.X && y == width) {
                    continue;
                }
                list.Add(new(x, y));
            }
        }

        return list;
    }
}