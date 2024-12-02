
namespace AdventOfCode2024.Days;
public abstract class DayLineBase<T> : IDay where T: notnull {

    public abstract T Import(string[] input);

    public abstract string Part1(T input);

    public abstract string Part2(T input);

    object IDay.Import(string input) {
        return Import(input.TrimEnd().Split(["\r\n", "\n"], StringSplitOptions.None));
    }

    string IDay.Part1(object input) {
        return Part1((T)input);
    }

    string IDay.Part2(object input) {
        return Part2((T)input);
    }
}
