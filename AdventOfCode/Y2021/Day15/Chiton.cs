using VectorInt;

namespace AdventOfCode.Y2021.Day15;

public class Chiton : AocSolution<Grid<int>> {
    public override string Name => "Chiton";

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
        int lowestRiskValue = GetLowestRiskValue(input);

        return $"Lowest total risk: {lowestRiskValue}";
    }

    protected override string Part2Implementation(Grid<int> input) {
        Grid<int> biggerGrid = new Grid<int>(input.Size * new VectorInt2(5));

        for (int x = 0; x < biggerGrid.Width; x++) {
            for (int y = 0; y < biggerGrid.Height; y++) {
                // Rolls over on 10, adds 1, to keep it within 1-9
                biggerGrid[x, y] = 1 + (input[x % input.Width, y % input.Height] + x / input.Width + y / input.Height - 1) % 9;
            }
        }

        int lowestRiskValue = GetLowestRiskValue(biggerGrid);

        return $"Lowest total risk: {lowestRiskValue}";
    }

    private static int GetLowestRiskValue(Grid<int> data) {
        Grid<int> distanceGrid = new(data.Width, data.Height);
        distanceGrid.SetAll(-1);
        PriorityQueue<VectorInt2, int> remainingPositions = new();

        remainingPositions.Enqueue(VectorInt2.Zero, 0);
        distanceGrid[VectorInt2.Zero] = 0;

        while (remainingPositions.Count > 0) {
            VectorInt2 pos = remainingPositions.Dequeue();
            int preDistance = distanceGrid[pos];

            foreach (VectorInt2 neighbor in data.GetPositionNeighbors(pos)) {
                if (distanceGrid[neighbor] != -1) {
                    // Already visited
                    continue;
                }

                int distance = preDistance + data[neighbor];
                distanceGrid[neighbor] = distance;
                remainingPositions.Enqueue(neighbor, distance);
                if (neighbor == new VectorInt2(data.Width - 1, data.Height - 1)) {
                    return distance;
                }
            }
        }

        return -1;
    }
}