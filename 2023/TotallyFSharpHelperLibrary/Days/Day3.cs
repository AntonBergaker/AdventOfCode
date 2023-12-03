namespace TotallyFSharpHelperLibrary.Days;

interface ICell;

record Blank : ICell;
record Symbol(char Character) : ICell;
record Number(int Digit) : ICell;

record EnginePart(int XEnd, int XStart, int Y, int Number);

public static class Day3 {
    public static void Run(string[] input) {

        var symbols = "@/%=*+-#$&";

        var grid = new Grid<ICell>(input[0].Length, input.Length, (x, y) =>
            input[y][x] switch {
                '.' => new Blank(),
                var c when symbols.Contains(c) => new Symbol(c),
                var a => new Number(int.Parse(a.ToString())),
            }

        );

        var engineParts = new List<EnginePart>();

        for (int y = 0; y < grid.Height; y++) {
            for (int x = 0; x < grid.Width; x++) {
                if (grid[x, y] is not Number) {
                    continue;
                }

                int startX = x;
                int num = 0;
                for (; x < grid.Width; x++) {
                    var cell = grid[x, y];
                    if (cell is Number number) {
                        num = num * 10 + number.Digit;
                    } else {
                        x--;
                        break;
                    }
                }

                engineParts.Add(new EnginePart(x, startX, y, num));

            }
        }

        int sum = 0;

        foreach (var part in engineParts) {
            bool hasNearbySymbol = false;
            for (int x = part.XStart - 1; x <= part.XEnd + 1; x++) {
                for (int y = part.Y - 1; y <= part.Y + 1; y++) {
                    if (grid.IsValidCoord(x, y) == false) {
                        continue;
                    }
                    if (grid[x, y] is not Symbol) {
                        continue;
                    }
                    hasNearbySymbol = true;
                    goto gigaBreak;
                }
            }
            gigaBreak:

            if (hasNearbySymbol) {
                sum += part.Number;
            }
        }

        Console.WriteLine($"Sum of all parts near symbols: {sum}");


        int gearRatioSums = 0;
        for (int y = 0; y < grid.Height; y++) {
            for (int x = 0; x < grid.Width; x++) {
                if (grid[x, y] is Symbol { Character: '*'}) {

                    var nearby = engineParts.Where(part => 
                        Math.Abs(part.Y - y) <= 1 &&
                        part.XStart-1 <= x && part.XEnd+1 >= x
                    ).ToArray();

                    if (nearby.Length == 2) {
                        gearRatioSums += nearby[0].Number * nearby[1].Number;
                    }
                }
            }
        }

        Console.WriteLine($"Sum of all gear ratios: {gearRatioSums}");
    }
}
