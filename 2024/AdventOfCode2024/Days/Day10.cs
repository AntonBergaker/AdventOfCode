﻿using AdventOfCode2024.Util;
using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;
public class Day10 : DayLineBase<Grid<int>> {
    // Linq is fun

    public override Grid<int> Import(string[] input) {
        return Grid<int>.FromChars(input, x => x.ToInt());
    }

    public override string Part1(Grid<int> grid) {
        var total = grid.GetPositions()
            .Where(position => grid[position] == 0) // Get all trailhead start positions
            .Select(position => FindTops(grid, position, 0).Distinct().Count()) // Filter identical tops
            .Sum(); // Add it all together

        return $"Total trailhead scores: {total.ToString().Pastel(Color.Yellow)}";
    }


    public override string Part2(Grid<int> grid) {
        var total = grid.GetPositions()
            .Where(position => grid[position] == 0) // Get all trailhead start positions
            .Select(position => FindTops(grid, position, 0).Count()) // Get number of tops
            .Sum(); // Add it all together

        return $"Total trailhead rankings: {total.ToString().Pastel(Color.Yellow)}";
    }

    private static IEnumerable<Vector2Int> FindTops(Grid<int> grid, Vector2Int position, int currentHeight) {
        if (currentHeight >= 9) {
            return [position];
        }
        return grid
            .GetPositionNeighbors(position) // Get surrounding cells
            .Where(neighbor => grid[neighbor] == currentHeight + 1) // Filter to max change of 1 height
            .SelectMany(neighbor => FindTops(grid, neighbor, currentHeight + 1)); // Run FindPath:s recursively
        
    }
}
