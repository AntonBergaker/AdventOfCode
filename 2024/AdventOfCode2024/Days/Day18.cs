using AdventOfCode2024.Util;
using InterpolatedParsing;
using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;
public class Day18 : DayLineBase<Vector2Int[]> {
    public override Vector2Int[] Import(string[] input) {
        return input.Select(line => {
            int x = 0, y = 0;
            InterpolatedParser.Parse(line, $"{x},{y}");
            return new Vector2Int(x, y);
        }).ToArray();
        
    }

    public override string Part1(Vector2Int[] positions) {
        (Vector2Int gridSize, int inputLength) = positions.Length <= 25 ? (new Vector2Int(7, 7), 12) : (new Vector2Int(71, 71), 1024);

        var grid = new Grid<bool>(gridSize);
        foreach (var position in positions.Take(inputLength)) {
            grid[position] = true;
        }

        var start = new Vector2Int(0, 0);
        var end = grid.Size - new Vector2Int(1);

        int bestDistance = Pathfind(grid, start, end);

        return $"Distance to exit after {inputLength} bytes: {bestDistance.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(Vector2Int[] positions) {
        Vector2Int gridSize = positions.Length <= 25 ? new(7, 7) : new(71, 71);
        var grid = new Grid<bool>(gridSize);

        var start = new Vector2Int(0, 0);
        var end = grid.Size - new Vector2Int(1);

        var last = new Vector2Int(-1);

        foreach (var position in positions) {
            grid[position] = true;

            int bestDistance = Pathfind(grid, start, end);

            if (bestDistance == -1) {
                last = position;
                break;
            }
        }

        return $"Position that prevented escape: {last.ToString().Pastel(Color.Yellow)}";
    }

    private static int Pathfind(Grid<bool> grid, Vector2Int start, Vector2Int end) {
        var distances = new Dictionary<Vector2Int, int>();
        var queue = new PriorityQueue<Vector2Int, int>();

        TryAdd(start, 0);

        while (queue.TryDequeue(out var position, out var distance)) {
            foreach (var neighbor in grid.GetPositionNeighbors(position)) {
                if (neighbor == end) {
                    return distance + 1;
                }
                TryAdd(neighbor, distance + 1);
            }
        }

        return -1;

        void TryAdd(Vector2Int position, int distance) {
            // Skip if we have one that's better already in the queue
            if (distances.ContainsKey(position)) {
                return;
            }
            if (grid.IsValidCoord(position) == false) {
                return;
            }
            if (grid[position]) {
                return;
            }

            distances[position] = distance;
            queue.Enqueue(position, distance);
        }
    }
}
