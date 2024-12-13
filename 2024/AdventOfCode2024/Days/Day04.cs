using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;
public class Day04 : DayLineBase<Grid<char>> {
    public override Grid<char> Import(string[] input) {
        return Grid<char>.FromChars(input, x => x);
    }

    public override string Part1(Grid<char> grid) {
        Vector2Int[] directions = [
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
        Vector2Int[] directions = [new(-1, -1), new(-1, 1)];

        var total = 0;

        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                if (grid[x, y] != 'A') {
                    continue;
                }
                var pos = new Vector2Int(x, y);

                // Check X shape by checking both directions
                if (directions.All(x =>
                    GridMatchesLetters(grid, pos - x, x, "MAS") ||
                    GridMatchesLetters(grid, pos - x, x, "SAM") // I'd love to reverse the angle instead of the letters, but this vector library doesn't have a unary negate.
                )) {
                    total++;
                }
            }
        }

        return $"Occurrences of X-MAS: {total.ToString().Pastel(Color.Yellow)}";
    }

    private bool GridMatchesLetters(Grid<char> grid, Vector2Int position, Vector2Int direction, string letters) {
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
