using VectorInt;

namespace AdventOfCode.Day05; 

static class HydrothermalVenture {

    private record Line(VectorInt2 P0, VectorInt2 P1) {
        public VectorInt2 Length => P1 - P0;
        
        public int Magnitude => Math.Max(Math.Abs(Length.X), Math.Abs(Length.Y));

        public VectorInt2 Direction => Length / new VectorInt2(Magnitude);
    }


    private static List<Line> GetAllLines(string[] input) {
        return input.Select(x => {
            int[] numbers = x.Split(new [] {",", "->"}, 
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(y => int.Parse(y)).ToArray();
            return new Line(new(numbers[0], numbers[1]), new(numbers[2], numbers[3]));
        }).ToList();
    }

    private static int GetMaxSize(List<Line> lines) {
        return lines.SelectMany(x => new[] {x.P0.X, x.P0.Y, x.P1.X, x.P1.Y} ).Max()+1;
    }


    public static void Run() {
        string[] input = File.ReadAllLines(Path.Join("Day05", "input.txt"));
        List<Line> lines = GetAllLines(input);
        int gridSize = GetMaxSize(lines);

        {
            Console.WriteLine("Hydrothermal Venture Part 1");
            int result = CalculateDangerAreas(lines, gridSize, x => x.Direction.IsUnit);

            Console.WriteLine($"Danger points: {result}\n");
        }
        {
            Console.WriteLine("Hydrothermal Venture Part 2");
            int result = CalculateDangerAreas(lines, gridSize, _ => true);
            Console.WriteLine($"Danger points: {result}\n");
        }

    }

    private static int CalculateDangerAreas(List<Line> lines, int gridSize, Func<Line, bool> shouldKeepLineFunc) {
        int[,] grid = new int[gridSize, gridSize];

        foreach (Line line in lines) {
            if (shouldKeepLineFunc(line) == false) {
                continue;
            }

            VectorInt2 direction = line.Direction;
            VectorInt2 p = line.P0;
            int magnitude = line.Magnitude;

            for (int i = 0; i <= magnitude; i++) {
                grid[p.X, p.Y]++;
                p += direction;
            }
        }

        return grid.Cast<int>().Count(number => number >= 2);
    }

}