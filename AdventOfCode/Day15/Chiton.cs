using VectorInt;

namespace AdventOfCode.Day15;

static class Chiton {

    public static void Run() {
        string[] input = File.ReadAllLines(Path.Join("Day15", "input.txt"));
        int width = input[0].Length;
        int height = input.Length;
        Grid<int> data = new(width, height);
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                data[x, y] = int.Parse(input[y][x].ToString());
            }
        }

        {
            Console.WriteLine("Chiton Part 1");

            int lowestRiskValue = GetLowestRiskValue(data);

            Console.WriteLine($"Lowest total risk: {lowestRiskValue}\n");
        }

        {
            Console.WriteLine("Chiton Part 2");

            Grid<int> biggerGrid = new Grid<int>(data.Size * new VectorInt2(5));

            for (int x = 0; x < biggerGrid.Width; x++) {
                for (int y = 0; y < biggerGrid.Height; y++) {
                    // Rolls over on 10, adds 1, to keep it within 1-9
                    biggerGrid[x, y] = 1 + (data[x % data.Width, y % data.Height] + x / data.Width + y/data.Height - 1) % 9;
                }
            }

            int lowestRiskValue = GetLowestRiskValue(biggerGrid);

            Console.WriteLine($"Lowest total risk: {lowestRiskValue}\n");
        }
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