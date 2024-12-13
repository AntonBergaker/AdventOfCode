﻿using Pastel;
using System.Drawing;
using System.Numerics;

namespace AdventOfCode2024.Days;
public class Day12 : DayLineBase<Grid<char>> {
    public override Grid<char> Import(string[] input) {
        return Grid<char>.FromChars(input, x => x);
    }

    private static readonly Vector2Int[] _directions = [new(-1, 0), new(1, 0), new(0, -1), new(0, 1)];

    public override string Part1(Grid<char> grid) {
        var regions = GetRegions(grid);

        var totalPrice = 0;
        foreach (var region in regions) {
            var totalOpenSides = 0;
            foreach (var position in region) {
                // Need a fence for all sides that are exposed.
                var neighbors = _directions.Count(direction => region.Contains(position + direction));
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
            var alreadyChecked = new HashSet<(Vector2Int Position, Vector2Int Direction)>();

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

    private static HashSet<Vector2Int>[] GetRegions(Grid<char> grid) {
        grid = grid.Clone(); // Clone so we can modify it as we remove things
        var regions = new List<HashSet<Vector2Int>>();

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

    private static HashSet<Vector2Int> ExtractRegion(Grid<char> grid, char @char, Vector2Int startPosition) {
        var regionPositions = new HashSet<Vector2Int>();
        var positionsToCheck = new Stack<Vector2Int>();
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

        void AddPosition(Vector2Int position) {
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
    private static bool CountFence(HashSet<Vector2Int> region, HashSet<(Vector2Int Position, Vector2Int Direction)> alreadyChecked, Vector2Int position, Vector2Int faceDirection) {
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

        void GoDirection(Vector2Int walkDirection) {
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
