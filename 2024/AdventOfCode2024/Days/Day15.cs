using AdventOfCode2024.Util;
using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;

public class Day15 : DayTextBase<Day15.InputData> {
    public enum Cell {
        Empty,
        Box,
        BoxL,
        BoxR,
        Wall
    }

    public record InputData(Grid<Cell> Map, Vector2Int StartPosition, Direction[] Movements);

    public override InputData Import(string input) {
        var parts = input.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.None);

        var startPosition = Grid<char>.FromChars(parts[0], x => x).PositionOf('@'); // Making a whole grid just to find the start position :cool:
        var map = Grid<Cell>.FromChars(parts[0], x => x switch {
            '#' => Cell.Wall,
            'O' => Cell.Box,
            _ => Cell.Empty
        });

        var movements = parts[1].ReplaceLineEndings("").Select(x => x switch {
            '<' => Direction.West,
            '^' => Direction.North,
            '>' => Direction.East,
            'v' => Direction.South,
            _ => throw new NotImplementedException()
        }).ToArray();

        return new(map, startPosition, movements);
    }



    public override string Part1(InputData input) {
        var map = input.Map.Clone();

        RunMovementsOnMap(map, input.StartPosition, input.Movements);

        var total = map.GetPositions().Where(x => map[x] == Cell.Box).Sum(x => x.X + x.Y * 100);
        return $"Sum of all boxes GPS coordinates: {total.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(InputData input) {
        var scale = new Vector2Int(2, 1);

        // Scale map up
        var map = new Grid<Cell>(input.Map.Size * scale);
        foreach (var pos in input.Map.GetPositions()) {
            var cell = input.Map[pos];
            var newPos = pos * scale;
            if (cell == Cell.Box) {
                map[newPos] = Cell.BoxL;
                map[newPos + new Vector2Int(1, 0)] = Cell.BoxR;
            } else {
                map[newPos] = cell;
                map[newPos + new Vector2Int(1, 0)] = cell;
            }
        }

        RunMovementsOnMap(map, input.StartPosition * scale, input.Movements);

        var total = map.GetPositions().Where(x => map[x] is Cell.Box or Cell.BoxL).Sum(x => x.X + x.Y * 100);
        return $"Sum of all scaled up boxes GPS coordinates: {total.ToString().Pastel(Color.Yellow)}";
    }

    private static void RunMovementsOnMap(Grid<Cell> map, Vector2Int position, Direction[] movements) {
        foreach (var direction in movements) {
            if (TryGetMovableBoxes(map, position, direction, out var boxes) == false) {
                continue;
            }

            // Clone all values before we start copying
            var boxPositionAndValue = boxes.Select(x => (x, map[x])).ToArray();
            // Clear the old positions
            foreach (var boxPosition in boxes) {
                map[boxPosition] = Cell.Empty;
            }
            // Place the boxes back down shifted
            foreach (var (boxPosition, value) in boxPositionAndValue) {
                map[boxPosition + direction] = value;
            }

            position += direction;
        }
    }

    private static void PrintMap(Grid<Cell> map, Vector2Int position) {
        Console.WriteLine(map.ToGridString((x, p) => x switch {
            Cell.Empty when p == position => "@".Pastel(Color.Red),
            Cell.Empty => " ",
            Cell.Wall => "#".Pastel(Color.White),
            Cell.BoxL => "[".Pastel(Color.Orange),
            Cell.BoxR => "]".Pastel(Color.Orange),
            Cell.Box => "O".Pastel(Color.Orange),
            _ => "?"
        }));
    }


    private static bool TryGetMovableBoxes(Grid<Cell> grid, Vector2Int position, Vector2Int direction, out HashSet<Vector2Int> boxes) {
        var collectedBoxes = new HashSet<Vector2Int>();
        var isVertical = direction.Y != 0;

        bool SearchBoxes(Vector2Int position) {
            if (collectedBoxes.Contains(position)) {
                return true;
            }
            if (grid.IsValidCoord(position) == false) {
                return false;
            }
            var cell = grid[position];
            if (cell == Cell.Wall) {
                return false;
            }
            if (cell == Cell.Empty) {
                return true;
            }
            if (cell is Cell.Box or Cell.BoxL or Cell.BoxR) {
                collectedBoxes.Add(position);
                // if a normal box or we're moving vertical, just check straight
                if (cell == Cell.Box || isVertical == false) {
                    return SearchBoxes(position + direction);
                }
                // if a wide box, bring with the other part and push recursively
                var offset = new Vector2Int(1, 0) * (cell == Cell.BoxL ? 1 : -1);
                collectedBoxes.Add(position + offset);
                return SearchBoxes(position + direction) && SearchBoxes(position + direction + offset);
            }

            throw new Exception("Funny");
        }

        var success = SearchBoxes(position+direction);
        if (success) {
            boxes = collectedBoxes;
            return true;
        }

        boxes = [];
        return false;
    }
}
