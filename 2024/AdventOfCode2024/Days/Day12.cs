using Pastel;
using System.Drawing;
using System.Numerics;
using VectorInt;

namespace AdventOfCode2024.Days;
public class Day12 : DayLineBase<Grid<char>> {
    public override Grid<char> Import(string[] input) {
        return Grid<char>.FromChars(input, x => x);
    }

    private static readonly VectorInt2[] _directions = [new(-1, 0), new(1, 0), new(0, -1), new(0, 1)];

    public override string Part1(Grid<char> grid) {
        var regions = GetRegions(grid);

        var totalPrice = 0;
        foreach (var region in regions) {
            var totalOpenSides = 0;
            foreach (var position in region) {
                // Need a fence for all sides that are exposed.
                var neighbors = _directions.Where(direction => region.Contains(position + direction)).Count();
                totalOpenSides += 4 - neighbors;
            }
            totalPrice += totalOpenSides * region.Count;
        }

        return $"Total price of fencing all regions: {totalPrice.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(Grid<char> grid) {
        var regions = GetRegions(grid);
        
        var totalPrice = 0;
        foreach (var region in regions) {
            var totalFences = 0;
            var alreadyChecked = new HashSet<(VectorInt2 Position, VectorInt2 Direction)>();

            foreach (var position in region) {
                // Look for fences in all directions
                foreach (var direction in _directions) {
                    if (CountFence(region, alreadyChecked, position, direction)) {
                        totalFences++;
                    }
                }
            }

            totalPrice += totalFences * region.Count;
        }


        return $"Total price of fencing all regions with discount: {totalPrice.ToString().Pastel(Color.Yellow)}";
    }

    private static HashSet<VectorInt2>[] GetRegions(Grid<char> grid) {
        grid = grid.Clone(); // Clone so we can modify it as we remove things
        var regions = new List<HashSet<VectorInt2>>();

        for (int y = 0; y < grid.Height; y++) {
            for (int x = 0; x < grid.Width; x++) {
                var @char = grid[x, y];
                // Already extracted
                if (@char == ' ') {
                    continue;
                }
                regions.Add(ExtractRegion(grid, @char, new(x, y)));
            }
        }

        return regions.ToArray();
    }

    private static HashSet<VectorInt2> ExtractRegion(Grid<char> grid, char @char, VectorInt2 startPosition) {
        var regionPositions = new HashSet<VectorInt2>();
        var positionsToCheck = new Stack<VectorInt2>();
        AddPosition(startPosition);
        while (positionsToCheck.TryPop(out var position)) {
            foreach (var neighbor in grid.GetPositionNeighbors(position)) {
                if (grid[neighbor] != @char) {
                    continue;
                }

                AddPosition(neighbor);
            }
        }
        return regionPositions;

        void AddPosition(VectorInt2 position) {
            grid[position] = ' ';
            positionsToCheck.Push(position);
            regionPositions.Add(position);
        }
    }

    /// <summary>
    /// Walk along a fence, adding every point to the alreadyChecked hashset parameter.
    /// </summary>
    /// <param name="region"></param>
    /// <param name="alreadyChecked"></param>
    /// <param name="position"></param>
    /// <param name="faceDirection"></param>
    /// <returns>true if its a a valid fence, otherwise false</returns>
    private static bool CountFence(HashSet<VectorInt2> region, HashSet<(VectorInt2 Position, VectorInt2 Direction)> alreadyChecked, VectorInt2 position, VectorInt2 faceDirection) {
        if (alreadyChecked.Contains((position, faceDirection))) {
            return false;
        }
        // Not a wall
        if (region.Contains(position + faceDirection)) {
            return false;
        }

        // Walk both ways
        GoDirection(new(-faceDirection.Y, faceDirection.X));
        GoDirection(new(faceDirection.Y, -faceDirection.X));

        void GoDirection(VectorInt2 walkDirection) {
            var pos = position;
            while (region.Contains(pos)) {
                // Not a wall anymore, thing in that way
                if (region.Contains(pos + faceDirection)) {
                    break;
                }

                alreadyChecked.Add((pos, faceDirection));
                pos += walkDirection;
            }
        }

        return true;
    }
}
