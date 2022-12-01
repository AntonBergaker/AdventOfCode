using VectorInt;

namespace AdventOfCode.Y2021.Day09;

public class SmokeBasin : AocSolution<Grid<int>> {
    public override string Name => "Smoke Basin";

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

    protected override string Part1Implementation(Grid<int> data) {
        int total = GetLowestPoints(data).Select(x => data[x] + 1).Sum();
        return $"Sum of risk level: {total}";
    }

    protected override string Part2Implementation(Grid<int> data) {
        var lowPoints = GetLowestPoints(data);

        // Make an array of bools to mark where we've been
        Grid<bool> visited = new(data.Width, data.Height);
        List<int> basinSizes = new List<int>();

        foreach (VectorInt2 lowPoint in lowPoints) {
            int size = 0;

            Stack<VectorInt2> nextPositions = new();
            nextPositions.Push(lowPoint);

            while (nextPositions.Count > 0) {
                VectorInt2 pos = nextPositions.Pop();
                var nearby = data.GetPositionNeighbors(pos);
                foreach (VectorInt2 near in nearby) {
                    if (visited[near]) {
                        continue;
                    }

                    if (data[near] == 9) {
                        continue;
                    }

                    visited[near] = true;
                    size++;
                    nextPositions.Push(near);
                }
            }

            basinSizes.Add(size);
        }

        basinSizes.Sort();
        int productOfBasinSizes = basinSizes.Take(^3..).Aggregate(1, (i, x) => i * x);

        return $"Product of largest basin sizes: {productOfBasinSizes}";
    }

    private static List<VectorInt2> GetLowestPoints(Grid<int> heightMap) {
        int width = heightMap.Width;
        int height = heightMap.Height;
        List<VectorInt2> points = new List<VectorInt2>();

        // Lol I hate it
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var nearby = heightMap.GetPositionNeighbors(new(x, y));
                int min = nearby.Select(near => heightMap[near]).Min();

                if (heightMap[x, y] < min) {
                    points.Add(new(x, y));
                }
            }
        }

        return points;
    }
}