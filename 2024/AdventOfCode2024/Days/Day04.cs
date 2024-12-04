using Pastel;
using System.Drawing;
using VectorInt;

namespace AdventOfCode2024.Days;
public class Day04 : DayLineBase<Grid<char>> {
    public override Grid<char> Import(string[] input) {
        var height = input.Length;
        var width = input[0].Length;
        
        var grid = new Grid<char>(width, height);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                grid[x, y] = input[y][x];
            }
        }
        return grid;
    }

    public override string Part1(Grid<char> grid) {
        VectorInt2[] directions = [
            new(-1, -1), new(0, -1), new(1, -1),
            new(-1,  0),             new(1,  0),
            new(-1,  1), new(0,  1), new(1,  1),
        ];

        var total = 0;

        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                if (grid[x, y] != 'X') {
                    continue;
                }
                foreach (var direction in directions) {
                    if (GridMatchesLetters(grid, new(x, y), direction, "XMAS")) {
                        total++;
                    }
                }
            }
        }

        return $"Occurrences of XMAS: {total.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(Grid<char> grid) {
        VectorInt2[] directions = [new(-1, -1), new(-1, 1)];

        var total = 0;

        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                if (grid[x, y] != 'A') {
                    continue;
                }
                var pos = new VectorInt2(x, y);
                var hits = 0;
                foreach (var direction in directions) {
                    if (GridMatchesLetters(grid, pos - direction, direction, "MAS") ||
                        GridMatchesLetters(grid, pos - direction, direction, "SAM")) {
                        hits++;
                    }
                }
                if (hits >= 2) {
                    total++;
                }
            }
        }

        return $"Occurrences of X-MAS: {total.ToString().Pastel(Color.Yellow)}";
    }

    private bool GridMatchesLetters(Grid<char> grid, VectorInt2 position, VectorInt2 direction, string letters) {
        for (int i = 0; i < letters.Length; i++) {
            if (grid.IsValidCoord(position) == false) {
                return false;
            }
            if (grid[position] != letters[i]) {
                return false;
            }
            position += direction;
        }
        return true;
    }
}
