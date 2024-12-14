using InterpolatedParsing;
using Pastel;
using System.Drawing;
using static AdventOfCode2024.Days.Day14;

namespace AdventOfCode2024.Days;

public class Day14 : DayLineBase<(Vector2Int Size, RobotData[] Robots)> {
    public record RobotData {
        public Vector2Int Position;
        public Vector2Int Velocity;

        public RobotData(Vector2Int position, Vector2Int velocity) {
            Position = position;
            Velocity = velocity;
        }
    }

    public override (Vector2Int Size, RobotData[] Robots) Import(string[] input) {
        Vector2Int size = input.Length > 20 ? new(101, 103) : new(11, 7); // Room size based on input size
        
        return (size, input.Select(line => {
            int x = 0, y = 0, vX = 0, vY = 0;
            InterpolatedParser.Parse(line, $"p={x},{y} v={vX},{vY}");
            return new RobotData(new(x, y), new(vX, vY));
        }).ToArray());
    }

    public override string Part1((Vector2Int Size, RobotData[] Robots) input) {
        var robots = input.Robots.Select(x => x with { }).ToArray(); // Clone input so we can modify

        MoveRobots(robots, input.Size, 100);

        var middle = input.Size / 2;
        var inQuadrantNW = robots.Count(robot => robot.Position.X < middle.X && robot.Position.Y < middle.Y);
        var inQuadrantNE = robots.Count(robot => robot.Position.X > middle.X && robot.Position.Y < middle.Y);
        var inQuadrantSW = robots.Count(robot => robot.Position.X < middle.X && robot.Position.Y > middle.Y);
        var inQuadrantSE = robots.Count(robot => robot.Position.X > middle.X && robot.Position.Y > middle.Y);

        var safetyFactor = inQuadrantNW * inQuadrantNE * inQuadrantSW * inQuadrantSE;
        return $"Product of quadrant sums: {safetyFactor.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2((Vector2Int Size, RobotData[] Robots) input) {
        var robots = input.Robots.Select(x => x with { }).ToArray(); // Clone input so we can modify
        var grid = new Grid<int>(input.Size);
        int seconds = 0;

        while (true) {
            seconds++;
            MoveRobots(robots, input.Size, 1);

            grid.SetAll(0);
            foreach (var robot in robots) {
                grid[robot.Position]++;
            }

            if (seconds >= 7093) { // number learned from divine intervention
                PrintGrid(grid);
                break;
            }
        }

        return $"Seconds until pattern emerged: {seconds.ToString().Pastel(Color.Yellow)}";
    }

    private static void PrintGrid(Grid<int> grid) {
        Console.WriteLine(grid.ToGridString(x => x switch {
            0 => ".".Pastel(Color.Gray),
            1 => "1".Pastel(Color.White),
            2 => "2".Pastel(Color.Green),
            3 => "3".Pastel(Color.Red),
            4 => "4".Pastel(Color.Yellow),
            _ => x.ToString().Pastel(Color.Pink)
        }));
    }

    /*
    private static void ExportAsImage(Grid<int> grid, int seconds) {
        // Download SkiaSharp to output the image
        var image = new SKBitmap(grid.Size.X, grid.Size.Y, true);
        for (int y = 0; y < grid.Height; y++) {
            for (int x = 0; x < grid.Width; x++) {
                if (grid[x, y] == 0) {
                    continue;
                }

                image.SetPixel(x, y, grid[x, y] switch {
                    1 => new SKColor(0xFFFFFFFF),
                    2 => new SKColor(0x00FF00FF),
                    3 => new SKColor(0xFF0000FF),
                    4 => new SKColor(0xF5E633FF),
                    _ => new SKColor(0xFF359BFF)
                });
            }
        }
        File.WriteAllBytes(seconds.ToString("0000.") + ".png", image.Encode(SKEncodedImageFormat.Png, 10).ToArray());
    }
    */

    private void MoveRobots(RobotData[] robots, Vector2Int size, int modifier) {
        foreach (var robot in robots) {
            var pos = robot.Position + robot.Velocity * modifier;
            pos.X = pos.X.Wrap(size.X);
            pos.Y = pos.Y.Wrap(size.Y);
            robot.Position = pos;
        }
    }
}
