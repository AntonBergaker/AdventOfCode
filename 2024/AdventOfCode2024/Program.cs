using AdventOfCode2024.Days;
using Pastel;
using System.Diagnostics;
using System.Drawing;

RunDay<Day01>(01);
RunDay<Day02>(02);
RunDay<Day03>(03);
RunDay<Day04>(04);
RunDay<Day05>(05);
RunDay<Day06>(06);

static void RunDay<TDay>(int dayNumber) where TDay: IDay, new() {
    var day = new TDay();

    Console.WriteLine($"{" ==== ".Pastel(Color.Gray)}{"Day ".Pastel(Color.LimeGreen)}{dayNumber.ToString().Pastel(Color.Red)}{" ==== ".Pastel(Color.Gray)}");

#if DEBUG
    RunInput("TestInput", "(test) ");
    Console.WriteLine();
#endif
    RunInput("Input", "");

    void RunInput(string folderPath, string suffix) {
        var import = day.Import(File.ReadAllText($"{folderPath}/day{dayNumber:00}.txt"));

        RunPart("1", suffix, () => day.Part1(import));
        Console.WriteLine();
        RunPart("2", suffix, () => day.Part2(import));
    }

    void RunPart(string part, string suffix, Func<string> function) {
        var stopwatch = Stopwatch.StartNew();
        string result;
        try {
            result = function();
        } catch (NotImplementedException) {
            return;
        }
        
        stopwatch.Stop();

        Console.WriteLine(
            $"Part {part.Pastel(Color.LightSkyBlue)}: " + suffix +
            $"{"(took ".Pastel(Color.Gray)}{stopwatch.ElapsedMilliseconds.ToString().Pastel(Color.LightSkyBlue)}{" ms)".Pastel(Color.Gray)}");

        Console.WriteLine(result);
    }
}