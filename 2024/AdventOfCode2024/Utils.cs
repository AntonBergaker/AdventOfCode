using System.Numerics;

namespace AdventOfCode2024;
public static class Utils {

    public static bool IsInsideBounds(Vector2Int position, Vector2Int size) {
        return position.X >= 0 && position.Y >= 0 && position.X < size.X && position.Y < size.Y;
    }

    public static int ToInt(this char @char) {
        // Funny way to convert char to int... but doesn't create garbage like ToString() and int.Parse() would.
        return @char - '0'; 
    }

    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createFunction) where TKey: notnull {
        if (dictionary.TryGetValue(key, out var value)) {
            return value;
        }
        value = createFunction();
        dictionary.Add(key, value);
        return value;
    }

    /// <summary>
    /// Modulus but it keeps negative inputs within the bound of 0 - (size-1)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static int Wrap(this int value, int size) {
        return (value % size + size) % size;
    }
}
