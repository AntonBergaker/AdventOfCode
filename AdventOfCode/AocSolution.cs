namespace AdventOfCode;

internal abstract class AocSolution<T> {

    public abstract string Name { get; }

    public void RunPart1(string input) {
        Part1(ProcessInput(input));
    }

    public void RunPart2(string input) {
        Part2(ProcessInput(input));
    }

    protected abstract void Part1(T input);

    public abstract void Part2(T input);

    public virtual T ProcessInput(string input) {
        if (input is T typedInput) {
            return typedInput;
        }

        if (typeof(T) == typeof(string[])) {
            return (T)(object)input.Split('\n');
        }

        throw new Exception($"Type not automatically implemented for type {typeof(T)}. Please override ProcessInput to make it the correct type!");
    }
}
