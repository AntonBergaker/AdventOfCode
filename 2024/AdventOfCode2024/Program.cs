using AdventOfCode2024.Days;
using Pastel;
using System.Diagnostics;
using System.Drawing;

bool useTestInput = false;

RunDay<Day01>(01, useTestInput);
RunDay<Day02>(02, useTestInput);


static void RunDay<TDay>(int dayNumber, bool useTestInput) where TDay: IDay, new() {
    var day = new TDay();
    var folder = useTestInput ? "TestInput" : "Input";
    var import = day.Import(File.ReadAllText($"{folder}/day{dayNumber:00}.txt"));
    
    Console.WriteLine($"{" ==== ".Pastel(Color.Gray)}{"Day ".Pastel(Color.LimeGreen)}{dayNumber.ToString().Pastel(Color.Red)}{" ==== ".Pastel(Color.Gray)}");

    RunPart("1", () => day.Part1(import));
    Console.WriteLine();
    RunPart("2", () => day.Part2(import));

    void RunPart(string part, Func<string> function) {
        var stopwatch = Stopwatch.StartNew();
        var result = function();
        stopwatch.Stop();

        Console.WriteLine(
            $"Part {part.Pastel(Color.LightSkyBlue)}: " +
            $"{"(took: ".Pastel(Color.Gray)}{stopwatch.ElapsedMilliseconds.ToString().Pastel(Color.LightSkyBlue)}{" ms)".Pastel(Color.Gray)}");

        Console.WriteLine(result);
    }
}