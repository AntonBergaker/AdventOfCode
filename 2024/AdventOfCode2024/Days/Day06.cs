using Pastel;
using System.Drawing;
using System.Numerics;
using VectorInt;
using PositionDirection = (VectorInt.VectorInt2 Position, VectorInt.VectorInt2 Direction);

namespace AdventOfCode2024.Days;
public class Day06 : DayLineBase<Day06.InputData> {
    public enum Cell {
        Empty,
        Wall
    }

    public record InputData(VectorInt2 StartPosition, Grid<Cell> Cells);

    public override InputData Import(string[] input) {
        var height = input.Length;
        var width = input[0].Length;

        var grid = new Grid<Cell>(width, height);
        var startPosition = new VectorInt2(-1, -1);

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var @char = input[y][x];

                if (@char == '#') {
                    grid[x, y] = Cell.Wall;
                } else if (@char == '.') {
                    grid[x, y] = Cell.Empty;
                } else if (@char == '^') {
                    grid[x, y] = Cell.Empty;
                    startPosition = new(x, y);
                } else {
                    throw new Exception($"Unexpected character in input {@char}");
                }
            }
        }

        return new(startPosition, grid);
    }

    public override string Part1(InputData input) {
        var total = GetGuardWalkingPositions(input).Length;
        return $"Total number of cells visited: {total.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(InputData input) {
        var placesToBlock = GetGuardWalkingPositions(input)[1..];
        var total = 0;
        var cells = input.Cells.Clone(); // Clone to not taint input. For uhm multithreading safety or something.

        foreach (var placeToBlock in placesToBlock) {
            cells[placeToBlock] = Cell.Wall;
            
            if (CanExitMaze(new(input.StartPosition, cells)) == false) {
                total++;
            }

            cells[placeToBlock] = Cell.Empty; // Return to before
        }

        return $"Number of positions where we can create loops: {total.ToString().Pastel(Color.Yellow)}";
    }

    private bool CanExitMaze(InputData input) {
        var hasVisited = new HashSet<PositionDirection>();

        foreach (var position in IterateMaze(input)) {
            if (hasVisited.Contains(position)) {
                return false;
            }
            hasVisited.Add(position);
        }

        return true;
    }

    private VectorInt2[] GetGuardWalkingPositions(InputData input) {
        return IterateMaze(input).Select(x => x.Position).Distinct().ToArray();
    }

    private static IEnumerable<PositionDirection> IterateMaze(InputData input) {
        var (position, cells) = input;
        var direction = new VectorInt2(0, -1);

        while (true) {
            Vector2 next = position + direction;

            // You're free
            if (cells.IsValidCoord(next) == false) {
                break;
            }
            // Rotate right if wall
            if (cells[next] == Cell.Wall) {
                direction = new(-direction.Y, direction.X);
                continue;
            }

            yield return (next, direction);
            position = next;
        }
    }
}
