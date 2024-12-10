using System.Numerics;

namespace AdventOfCode2024;
public static class Utils {
    public static bool IsInsideBounds(Vector2 position, Vector2 size) {
        return position.X >= 0 && position.Y >= 0 && position.X < size.X && position.Y < size.Y;
    }

    public static int ToInt(this char @char) {
        // Funny way to convert char to int... but doesn't create garbage like ToString() and int.Parse() would.
        return @char - '0'; 
    }
}
