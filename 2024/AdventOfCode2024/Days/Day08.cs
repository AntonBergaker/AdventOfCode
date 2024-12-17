using AdventOfCode2024.Util;
using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;
public class Day08 : DayLineBase<Day08.GridData> {
    public record GridData(Dictionary<char, List<Vector2Int>> Positions, Vector2Int Size);

    public override GridData Import(string[] input) {
        // Put every signal position in a dictionary with the type of signal as the key
        var dict = new Dictionary<char, List<Vector2Int>>();
        var height = input.Length;
        var width = input[0].Length;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var @char = input[y][x];
                if (@char == '.') {
                    continue;
                }
                if (dict.TryGetValue(@char, out var list) == false) {
                    dict[@char] = list = new();
                }
                list.Add(new(x, y));
            }
        }

        return new(dict, new(width, height));
    }

    public override string Part1(GridData input) {
        var antinodePositions = new HashSet<Vector2Int>();

        foreach (var (_, positions) in input.Positions) {
            CompareAntennas(positions, (antenna0, antenna1) => {
                var difference = antenna1 - antenna0;
                var pos = antenna0 - difference;
                if (Utils.IsInsideBounds(pos, input.Size)) {
                    antinodePositions.Add(pos);
                }
            });
        }

        return $"Total number of antinode positions: {antinodePositions.Count.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(GridData input) {
        var antinodePositions = new HashSet<Vector2Int>();

        foreach (var (_, positions) in input.Positions) {
            CompareAntennas(positions, (antenna0, antenna1) => {
                var pos = antenna0;
                var difference = antenna1 - antenna0;
                while (true) {
                    pos += difference;
                    if (Utils.IsInsideBounds(pos, input.Size) == false) {
                        break;
                    }
                    antinodePositions.Add(pos);
                }
            });
        }

        return $"Total number of antinode positions: {antinodePositions.Count.ToString().Pastel(Color.Yellow)}";
    }

    private void CompareAntennas(List<Vector2Int> positions, Action<Vector2Int, Vector2Int> func) {
        for (int i = 0; i < positions.Count; i++) {
            for (int j = i + 1; j < positions.Count; j++) {
                var position0 = positions[i];
                var position1 = positions[j];

                func(position0, position1);
                func(position1, position0);
            }
        }
    }
}
