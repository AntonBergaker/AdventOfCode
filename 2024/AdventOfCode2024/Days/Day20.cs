using AdventOfCode2024.Util;
using Pastel;
using System.Drawing;
using static AdventOfCode2024.Days.Day20;

namespace AdventOfCode2024.Days;
internal class Day20 : DayTextBase<Grid<Cell>> {

    public enum Cell {
        Blank,
        Wall,
        Start,
        End,
    }

    public override Grid<Cell> Import(string input) {
        return Grid<Cell>.FromChars(input, x => x switch {
            '#' => Cell.Wall,
            'E' => Cell.End,
            'S' => Cell.Start,
            '.' => Cell.Blank,
            _ => throw new NotImplementedException()
        });
    }

    private int GetDistance(Vector2Int pos0, Vector2Int pos1) {
        return Math.Abs(pos0.X - pos1.X) + Math.Abs(pos0.Y - pos1.Y);
    }

    public override string Part1(Grid<Cell> grid) {
        var shortcutCount = GetShortcutCount(grid, 2);

        return $"Shortcuts of 2 picoseconds that shortens by at least 100 picoseconds: {shortcutCount.ToString().Pastel(Color.Yellow)}.";
    }

    public override string Part2(Grid<Cell> grid) {
        var shortcutCount = GetShortcutCount(grid, 20);

        return $"Shortcuts of 20 picoseconds that shortens by at least 100 picoseconds: {shortcutCount.ToString().Pastel(Color.Yellow)}.";
    }

    private int GetShortcutCount(Grid<Cell> grid, int travelDistance) {
        grid = grid.Clone();

        var startPosition = grid.PositionOf(Cell.Start);
        grid[startPosition] = Cell.Blank;
        var endPosition = grid.PositionOf(Cell.End);
        grid[endPosition] = Cell.Blank;

        var distancesFromStart = FloodFill(grid, startPosition);
        var distancesFromEnd = FloodFill(grid, endPosition);

        var queue = new Queue<Vector2Int>();
        queue.Enqueue(startPosition);
        var manhattanDistances = GetPointsInManhattanRange(travelDistance);

        var normalDistance = distancesFromStart[endPosition];
        var shortcutCount = 0;

        while (queue.TryDequeue(out var position)) {
            var distance = distancesFromStart[position];
            foreach (var neighbor in grid.GetPositionNeighbors(position)) {
                if (distancesFromStart[neighbor] < distance) {
                    continue;
                }
                queue.Enqueue(neighbor);
            }

            // Scan the nearest 20 grids
            foreach (var movement in manhattanDistances) {
                var newPosition = position + movement;
                if (grid.IsValidCoord(newPosition) == false || grid[newPosition] == Cell.Wall) {
                    continue;
                }

                var totalDistance = distancesFromStart[position] + distancesFromEnd[newPosition] + GetDistance(position, newPosition);
                if (normalDistance - totalDistance >= 100) {
                    shortcutCount++;
                }
            }
        }

        return shortcutCount;
    }

    List<Vector2Int> GetPointsInManhattanRange(int range) {
        var list = new List<Vector2Int>();

        for (int x = -range; x <= range; x++) {
            int yWidth = range - Math.Abs(x);

            for (int y = -yWidth; y <= yWidth; y++) {
                list.Add(new(x, y));
            }
        }

        return list;
    }

    private Grid<int> FloodFill(Grid<Cell> grid, Vector2Int fillStartPosition) {
        var distances = new Grid<int>(grid.Size);
        distances.SetAll(-1);

        var queue = new Queue<Vector2Int>();
        queue.Enqueue(fillStartPosition);
        distances[fillStartPosition] = 0;

        while (queue.TryDequeue(out var position)) {
            var myValue = distances[position];
            foreach (var neighbor in grid.GetPositionNeighbors(position)) {
                if (grid[neighbor] == Cell.Wall || distances[neighbor] >= 0) {
                    continue;
                }

                distances[neighbor] = myValue + 1;
                queue.Enqueue(neighbor);
            }
        }
        return distances;
    }
}
