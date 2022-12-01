namespace AdventOfCode;

public abstract class AocSolution {
    public abstract string Name { get; }
    public virtual string InputPath {
        get {
            string namespaceName = GetType().FullName ?? throw new NullReferenceException("FullName not defined for type");
            string[] pieces = namespaceName.Split('.');
            return string.Join(Path.DirectorySeparatorChar, pieces[1..3]);
        }
    }

    public string Part1(string input) {
        return Part1Implementation(input);
    }

    public string Part2(string input) {
        return Part2Implementation(input);
    }

    protected abstract string Part1Implementation(string input);

    protected abstract string Part2Implementation(string input);
}

public abstract class AocSolution<T> : AocSolution {

    protected sealed override string Part1Implementation(string input) {
        return Part1Implementation(ProcessInput(input));
    }

    protected sealed override string Part2Implementation(string input) {
        return Part2Implementation(ProcessInput(input));
    }

    protected abstract string Part1Implementation(T input);

    protected abstract string Part2Implementation(T input);

    protected virtual T ProcessInput(string input) {
        if (input is T typedInput) {
            return typedInput;
        }

        if (typeof(T) == typeof(string[])) {
            return (T)(object)input.SplitLines();
        }

        throw new Exception($"Type not automatically implemented for type {typeof(T)}. Please override ProcessInput to make it the correct type!");
    }
}
