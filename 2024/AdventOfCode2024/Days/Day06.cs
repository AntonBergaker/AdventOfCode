using Pastel;
using System.Drawing;
using System.Numerics;
using PositionDirection = (VectorT.Vector2<int> Position, VectorT.Vector2<int> Direction);

namespace AdventOfCode2024.Days;
public class Day06 : DayLineBase<Day06.InputData> {
    public enum Cell {
        Empty,
        StartPosition,
        Wall
    }

    public record InputData(Vector2Int StartPosition, Grid<Cell> Cells);

    public override InputData Import(string[] input) {
        var grid = Grid<Cell>.FromChars(input, @char => {
            return @char switch {
                '#' => Cell.Wall,
                '.' => Cell.Empty,
                '^' => Cell.StartPosition,
                _ => throw new Exception($"Unexpected character in input {@char}")
            };
        });
        var startPosition = grid.PositionOf(Cell.StartPosition);
        grid[startPosition] = Cell.Empty;
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

    private Vector2Int[] GetGuardWalkingPositions(InputData input) {
        return IterateMaze(input).Select(x => x.Position).Distinct().ToArray();
    }

    private static IEnumerable<PositionDirection> IterateMaze(InputData input) {
        var (position, cells) = input;
        var direction = new Vector2Int(0, -1);

        while (true) {
            Vector2Int next = position + direction;

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
