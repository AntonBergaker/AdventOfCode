using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2024.Util;
public readonly struct Direction : IEquatable<Direction> {
    private const byte EastValue = 0;
    private const byte SouthValue = 1;
    private const byte WestValue = 2;
    private const byte NorthValue = 3;

    public static readonly Direction East = new(EastValue);
    public static readonly Direction South = new(SouthValue);
    public static readonly Direction West = new(WestValue);
    public static readonly Direction North = new(NorthValue);

    public static readonly Direction[] Directions = [East, South, West, North];

    private readonly byte _value;
    private Direction(byte value) { 
        _value = value; 
    }

    public static implicit operator Vector2Int(Direction direction) {
        return direction.ToVector2();
    }

    public Vector2Int ToVector2() {
        return _value switch {
            EastValue => new(1, 0),
            SouthValue => new(0, 1),
            WestValue => new(-1, 0),
            NorthValue => new(0, -1),
            _ => throw new Exception("Invalid direction internal value")
        };
    }

    public Direction RotateClockwise() {
        return new Direction((byte)((_value + 1) % 4));
    }
    public Direction RotateCounterClockwise() {
        return new Direction((byte)((_value + 3) % 4));
    }
    public Direction Flip() {
        return new Direction((byte)((_value + 2) % 4));
    }

    public override int GetHashCode() {
        return _value.GetHashCode();
    }

    public override bool Equals([NotNullWhen(true)] object? obj) {
        if (obj is not Direction other) {
            return false;
        }
        return Equals(other);
    }

    public bool Equals(Direction other) {
        return other._value == _value;
    }

    public static bool operator ==(Direction left, Direction right) {
        return left._value == right._value;
    }

    public static bool operator !=(Direction left, Direction right) {
        return left._value != right._value;
    }

    public override string ToString() {
        return _value switch {
            0 => "East",
            1 => "South",
            2 => "West",
            3 => "North",
            _ => throw new Exception("Invalid direction internal value")
        };
    }
}
