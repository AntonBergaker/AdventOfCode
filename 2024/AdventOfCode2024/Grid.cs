﻿using System.Collections;
using System.Text;
using VectorInt;

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

    public T this[VectorInt2 point] {
        get => _data[point.X, point.Y];
        set => _data[point.X, point.Y] = value;
    }

    public Grid(int width, int height) {
        _data = new T[width, height];
        Width = width;
        Height = height;
    }

    public Grid(VectorInt2 size) : this(size.X, size.Y) {
    }

    public VectorInt2 Size => new(Width, Height);

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

    public IEnumerable<VectorInt2> GetPositionNeighbors(VectorInt2 pos) {
        if (pos.X > 0) {
            yield return pos + new VectorInt2(-1, 0);
        }
        if (pos.X < Width - 1) {
            yield return pos + new VectorInt2(1, 0);
        }
        if (pos.Y > 0) {
            yield return pos + new VectorInt2(0, -1);
        }
        if (pos.Y < Height - 1) {
            yield return pos + new VectorInt2(0, 1);
        }
    }

    public string ToGridString() {
        return ToGridString(x => x?.ToString() ?? "null");
    }

    public string ToGridString(Func<T, string> toStringFunc) {
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++) {
            if (y > 0) {
                sb.Append('\n');
            }
            for (int x = 0; x < Width; x++) {
                sb.Append(toStringFunc(_data[x, y]));
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
    public bool IsValidCoord(VectorInt2 position) {
        return IsValidCoord(position.X, position.Y);
    }
}