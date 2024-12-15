using AdventOfCode2024.Days;
using System.Collections;
using System.Text;

namespace AdventOfCode2024;

/// <summary>
/// Because it seems every task needs a grid
/// </summary>
public class Grid<T> : IEnumerable<T>, ICloneable {
    private readonly T[,] _data;
    public readonly int Width;
    public readonly int Height;

    public T this[int x, int y] {
        get => _data[x, y];
        set => _data[x, y] = value;
    }

    public T this[Vector2Int point] {
        get => _data[point.X, point.Y];
        set => _data[point.X, point.Y] = value;
    }

    public Grid(int width, int height) {
        _data = new T[width, height];
        Width = width;
        Height = height;
    }

    public Grid(T[,] array) : this(array.GetLength(0), array.GetLength(1)) {
        array.CopyTo(_data, 0);
    }

    public Grid(T[][] array) : this(array[0].Length, array.Length) {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                _data[x, y] = array[y][x];
            }
        }
    }

    public Grid(Vector2Int size) : this(size.X, size.Y) {
    }

    public Vector2Int Size => new(Width, Height);

    public IEnumerator<T> GetEnumerator() {
        return _data.Cast<T>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public Grid<T> Clone() {
        Grid<T> clone = new(Width, Height);
        Array.Copy(_data, clone._data, _data.Length);
        return clone;
    }

    public IEnumerable<Vector2Int> GetPositionNeighbors(Vector2Int pos) {
        if (pos.X > 0) {
            yield return pos + new Vector2Int(-1, 0);
        }
        if (pos.X < Width - 1) {
            yield return pos + new Vector2Int(1, 0);
        }
        if (pos.Y > 0) {
            yield return pos + new Vector2Int(0, -1);
        }
        if (pos.Y < Height - 1) {
            yield return pos + new Vector2Int(0, 1);
        }
    }

    public IEnumerable<Vector2Int> GetPositions() {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                yield return new(x, y);
            }
        }
    }

    public string ToGridString() {
        return ToGridString((x, _) => x?.ToString() ?? "null");
    }

    public string ToGridString(Func<T, string> toStringFunc) {
        return ToGridString((x, _) => toStringFunc(x));
    }

    public string ToGridString(Func<T, Vector2Int, string> toStringFunc) { 
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++) {
            if (y > 0) {
                sb.Append('\n');
            }
            for (int x = 0; x < Width; x++) {
                sb.Append(toStringFunc(_data[x, y], new(x, y)));
            }
        }

        return sb.ToString();
    }

    object ICloneable.Clone() {
        return this.Clone();
    }

    public void SetAll(T value) {
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                _data[x, y] = value;
            }
        }
    }
    public bool IsValidCoord(int x, int y) {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }
    public bool IsValidCoord(Vector2Int position) {
        return IsValidCoord(position.X, position.Y);
    }

    public static Grid<T> FromChars(string[] chars, Func<char, T> transform) {
        var height = chars.Length;
        var width = chars[0].Length;

        var grid = new Grid<T>(width, height);

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                grid[x, y] = transform(chars[y][x]);
            }
        }

        return grid;
    }

    public static Grid<T> FromChars(string chars, Func<char, T> transform) {
        return FromChars(chars.TrimEnd().Split(["\r\n", "\n"], StringSplitOptions.None), transform);
    }

    public Vector2Int PositionOf(T value) {
        var comparer = EqualityComparer<T>.Default;
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (comparer.Equals(value, _data[x, y])) {
                    return new(x, y);
                }
            }
        }
        return new(-1, -1);
    }
}