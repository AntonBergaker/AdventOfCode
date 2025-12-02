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

    private int GetShortestDistance(Grid<Cell> grid, Vector2Int start, Vector2Int end) {
        // Get the shortest distance based on A*
        Grid<bool> visited = new(grid.Size);

        PriorityQueue<Vector2Int, int> queue = new();
        queue.Enqueue(start, 0);

        while (queue.TryDequeue(out var pos, out var priority)) {
            priority -= GetHeuristic(pos);
            if (pos == end) {
                return priority;
            }

            foreach (var neighbour in grid.GetPositionNeighbors(pos)) {
                if (grid[neighbour] == Cell.Wall || visited[neighbour]) {
                    continue;
                }

                queue.Enqueue(neighbour, priority + 1 + GetHeuristic(neighbour));
                visited[neighbour] = true;
            }
        }

        int GetHeuristic(Vector2Int pos) {
            return Math.Abs(pos.X - end.X) + Math.Abs(pos.Y - end.Y);
        }

        return -1;
    }

    public override string Part1(Grid<Cell> grid) {
        grid = grid.Clone();

        var startPosition = grid.PositionOf(Cell.Start);
        grid[startPosition] = Cell.Blank;
        var endPosition = grid.PositionOf(Cell.End);
        grid[endPosition] = Cell.Blank;

        var normalShortest = GetShortestDistance(grid, startPosition, endPosition);
        var numberShortenedBy100 = 0;

        foreach (var position in grid.GetPositions()) {
            if (grid[position] != Cell.Wall) {
                continue;
            }

            grid[position] = Cell.Blank;
            var shortest = GetShortestDistance(grid, startPosition, endPosition);
            grid[position] = Cell.Wall;

            if (normalShortest - shortest >= 100) {
                numberShortenedBy100++;
            }
        }

        return $"Walls removed that shortens by at least 100 picoseconds: {numberShortenedBy100.ToString().Pastel(Color.Yellow)}.";
    }

    public override string Part2(Grid<Cell> grid) {
        grid = grid.Clone();

        var startPosition = grid.PositionOf(Cell.Start);
        grid[startPosition] = Cell.Blank;
        var endPosition = grid.PositionOf(Cell.End);
        grid[endPosition] = Cell.Blank;

        return $"Walls removed that shortens by at least 100 picoseconds: {numberShortenedBy100.ToString().Pastel(Color.Yellow)}.";
    }
}
