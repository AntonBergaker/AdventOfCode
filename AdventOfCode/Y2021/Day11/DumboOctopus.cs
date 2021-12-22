using VectorInt;

namespace AdventOfCode.Day11; 

static class DumboOctopus {
    public static void Run() {
        string[] input = File.ReadAllLines(Path.Join("Day11", "input.txt"));
        int width = input[0].Length;
        int height = input.Length;
        Grid<int> data = new(width, height);
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                data[x, y] = int.Parse(input[y][x].ToString());
            }
        }

        {
            Console.WriteLine("Dumbo Octopus Part 1");
            Grid<int> part1Data = data.Clone();
            int totalFlashes = 0;
            for (int i = 0; i < 100; i++) {
                totalFlashes += SimulateOctopuses(part1Data);
            }

            Console.WriteLine($"Total flashes: {totalFlashes}\n");
        }
        {
            Console.WriteLine("Dumbo Octopus Part 2");
            Grid<int> part2Data = data.Clone();

            // simulate a long time, hopefully breaks earlier than this
            int allFlashAt = -1;
            for (int i = 0; i < 100_000; i++) {
                SimulateOctopuses(part2Data);

                if (part2Data.All(x => x == 0)) {
                    allFlashAt = i + 1;
                    break;
                }
            }

            Console.WriteLine($"Round of total flash: {allFlashAt}\n");
        }
            
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
        int x0 = Math.Clamp(pos.X - 1, 0, width-1);
        int x1 = Math.Clamp(pos.X + 1, 0, width-1);
        int y0 = Math.Clamp(pos.Y - 1, 0, height-1);
        int y1 = Math.Clamp(pos.Y + 1, 0, height-1);

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