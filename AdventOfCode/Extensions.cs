namespace AdventOfCode;

public static class Extensions {

    private static string[] splitLineArray = { "\n", "\r\n" };

    public static string[] SplitLines(this string text) {
        var entries = text.Split(splitLineArray, StringSplitOptions.None);
        if (entries.LastOrDefault() == "") {
            return entries[..^1];
        }
        return entries;
    }

}
