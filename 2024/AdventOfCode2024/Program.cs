using AdventOfCode2024.Days;
using Pastel;
using System.Diagnostics;
using System.Drawing;

RunDay<Day01>(01);
RunDay<Day02>(02);
RunDay<Day03>(03);
RunDay<Day04>(04);
RunDay<Day05>(05);
//RunDay<Day06>(06);
RunDay<Day07>(07);
RunDay<Day08>(08);
RunDay<Day09>(09);
RunDay<Day10>(10);
RunDay<Day11>(11);
RunDay<Day12>(12);

static void RunDay<TDay>(int dayNumber) where TDay: IDay, new() {
    var day = new TDay();

    Console.WriteLine($"{" ==== ".Pastel(Color.Gray)}{"Day ".Pastel(Color.LimeGreen)}{dayNumber.ToString().Pastel(Color.Red)}{" ==== ".Pastel(Color.Gray)}");

#if DEBUG
    RunInput("TestInput", $"{"(".Pastel(Color.Gray)}{"test".Pastel(Color.DeepPink)}{") "}".Pastel(Color.Gray));
    Console.WriteLine();
#endif
    RunInput("Input", "");

    void RunInput(string folderPath, string suffix) {
        var stopwatch = Stopwatch.StartNew();
        var import = day.Import(File.ReadAllText($"{folderPath}/day{dayNumber:00}.txt"));
        stopwatch.Stop();

        RunPart("1", suffix, stopwatch.ElapsedTicks, () => day.Part1(import));
        Console.WriteLine();
        RunPart("2", suffix, stopwatch.ElapsedTicks, () => day.Part2(import));
    }

    void RunPart(string part, string suffix, long importTime, Func<string> function) {
        var stopwatch = Stopwatch.StartNew();
        string result;
        try {
            result = function();
        } catch (NotImplementedException) {
            return;
        }
        
        stopwatch.Stop();

        var time = (stopwatch.ElapsedTicks + importTime) / (Stopwatch.Frequency/1000.0);

        Console.WriteLine(
            $"Part {part.Pastel(Color.LightSkyBlue)}: " + suffix +
            $"{"(".Pastel(Color.Gray)}{time.ToString("0.0").Pastel(Color.LightSkyBlue)}{" ms)".Pastel(Color.Gray)}");

        Console.WriteLine(result);
    }
}