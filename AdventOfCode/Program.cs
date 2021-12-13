using System.Diagnostics;

namespace AdventOfCode; 

public class Program {
    public static void Main(string[] args) {
        var methods = new Action[] {
            Day01.SonarSweep.Run,
            Day02.Dive.Run,
            Day03.BinaryDiagnostic.Run,
            Day04.GiantSquid.Run,
            Day05.HydrothermalVenture.Run,
            Day06.Lanternfish.Run,
            Day07.TheTreacheryOfWhales.Run,
            Day08.SevenSegmentSearch.Run,
            Day09.SmokeBasin.Run,
            Day10.SyntaxScoring.Run,
            Day11.DumboOctopus.Run,
            Day12.PassagePathing.Run,
            Day13.TransparentOrigami.Run
        };

        foreach (var method in methods) {
            Stopwatch watch = Stopwatch.StartNew();
            method();
            watch.Stop();
            Console.WriteLine($"Took: {watch.ElapsedMilliseconds}ms\n\n");
        }
    }
}
