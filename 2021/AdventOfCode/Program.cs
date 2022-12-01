using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode;

public class Program {
    public static void Main(string[] args) {

        var solutionTypes = Assembly.GetAssembly(typeof(AocSolution))!.GetTypes()
        .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AocSolution)));

        List<AocSolution> solutions = new();

        foreach (var solutionType in solutionTypes) {
            var solution = Activator.CreateInstance(solutionType) as AocSolution;

            if (solution == null) {
                throw new Exception($"Failed to create instance for {solutionType}");
            }

            solutions.Add(solution);
        }

        solutions.Sort((a, b) => string.Compare(a.GetType().FullName, b.GetType().FullName));

        foreach (AocSolution solution in solutions) {
            void RunSolution(AocSolution solution, int part) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{solution.Name} Part {part}");

                string input = File.ReadAllText(Path.Join(solution.InputPath, "input.txt"));

                Stopwatch watch = Stopwatch.StartNew();

                Console.ForegroundColor = ConsoleColor.White;
                string result = part == 1 ? solution.Part1(input) : solution.Part2(input);

                watch.Stop();
                Console.WriteLine(result + "\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"Took: {watch.ElapsedMilliseconds}ms\n\n");

            }

            RunSolution(solution, 1);
            RunSolution(solution, 2);
        }

    }
}
