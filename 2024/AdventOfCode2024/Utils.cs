using System.Numerics;

namespace AdventOfCode2024;
public static class Utils {
    public static bool IsInsideBounds(Vector2 position, Vector2 size) {
        return position.X >= 0 && position.Y >= 0 && position.X < size.X && position.Y < size.Y;
    }
}
