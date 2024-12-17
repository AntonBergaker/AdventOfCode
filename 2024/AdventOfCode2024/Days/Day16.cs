namespace AdventOfCode2024.Days;

using System.Drawing;
using AdventOfCode2024.Util;
using Pastel;

public class Day16 : DayLineBase<Grid<Day16.Cell>> {
    public enum Cell {
        Space,
        Wall,
        Start,
        End
    }

    public override Grid<Cell> Import(string[] input) {
        return Grid<Cell>.FromChars(input, c => c switch {
          '#' => Cell.Wall,
          'S' => Cell.Start,
          'E' => Cell.End,
          _ => Cell.Space
        });
    }

    public override string Part1(Grid<Cell> map) {
        var start = new PositionDirection(map.PositionOf(Cell.Start), Direction.East);
        var end = map.PositionOf(Cell.End);

        var distances = GenerateDistanceMap(map, start);
        var total = int.MaxValue;
        foreach (var direction in Direction.Directions) {
            if (distances.TryGetValue(new(end, direction), out var foundTotal) == false) {
                continue;
            }
            total = Math.Min(total, foundTotal);
        }

        return $"Score of the shortest path: {total.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(Grid<Cell> map) {
        var start = new PositionDirection(map.PositionOf(Cell.Start), Direction.East);
        var end = map.PositionOf(Cell.End);

        var distances = GenerateDistanceMap(map, start);

        var toCheckBackwards = new Stack<(PositionDirection PositionDirection, int Distance)>();
        var bestStartValue = int.MaxValue;

        foreach (var direction in Direction.Directions) {
            if (distances.TryGetValue(new(end, direction), out var foundTotal) == false) {
                continue;
            }
            if (foundTotal < bestStartValue) {
                bestStartValue = foundTotal;
                toCheckBackwards.Clear();
            }
            if (foundTotal == bestStartValue) {
                toCheckBackwards.Push((new(end, direction.Flip()), bestStartValue));
            }
        }

        var visited = new HashSet<PositionDirection>();

        // Go backwards through the maze
        while (toCheckBackwards.TryPop(out var positionDirection)) {
            var (pos, dir) = positionDirection.PositionDirection;
            var distance = positionDirection.Distance;

            // Try rotate cw
            TryAdd(new(pos, dir.RotateClockwise()), distance - 1000);
            // Try rotate ccw
            TryAdd(new(pos, dir.RotateCounterClockwise()), distance - 1000);
            // Try move forwards
            TryAdd(new(pos + dir, dir), distance - 1);
        }

        void TryAdd(PositionDirection positionDirection, int distance) {
            // Skip if already visited
            if (visited.Contains(positionDirection)) {
                return;
            }

            // Flip direction because we went the other way last time
            if (distances.TryGetValue(positionDirection with { Direction = positionDirection.Direction.Flip() }, out var targetDistance) == false) {
                return;
            }
            if (distance != targetDistance) {
                return;
            }

            visited.Add(positionDirection);
            toCheckBackwards.Push((positionDirection, distance));
        }

        var total = visited.Select(x => x.Position).Distinct().Count();
        
        return $"Total squares that are part of best paths: {total.ToString().Pastel(Color.Yellow)}";
    }

    private static Dictionary<PositionDirection, int> GenerateDistanceMap(Grid<Cell> map, PositionDirection start) {
        var distanceMap = new Dictionary<PositionDirection, int>();
        var queue = new PriorityQueue<PositionDirection, int>();

        TryAdd(start, 0);

        while (queue.TryDequeue(out var positionDirection, out var distance)) {
            var (pos, dir) = positionDirection;

            // Try rotate cw
            TryAdd(new(pos, dir.RotateClockwise()), distance + 1000);
            // Try rotate ccw
            TryAdd(new(pos, dir.RotateCounterClockwise()), distance + 1000);
            // Try move forward
            TryAdd(new(pos + dir, dir), distance + 1);
        }

        void TryAdd(PositionDirection positionDirection, int distance) {
            // Skip if we have one that's better already in the queue
            if (distanceMap.TryGetValue(positionDirection, out var previousDistance) && previousDistance <= distance) {
                return;
            }
            var pos = positionDirection.Position;
            if (map.IsValidCoord(pos) == false) {
                return;
            }
            if (map[pos] == Cell.Wall) {
                return;
            }
            distanceMap[positionDirection] = distance;
            queue.Enqueue(positionDirection, distance);
        }

        return distanceMap;
    }
}