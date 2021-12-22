using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Y2021.Day02;

public class Dive : AocSolution<string[]> {
    public override string Name => "Dive";

    protected override string Part1Implementation(string[] input) {
        int x = 0;
        int y = 0;
        foreach (string line in input) {
            string[] components = line.Split(' ');
            string direction = components[0].ToLower();
            int count = int.Parse(components[1]);
            switch (direction) {
                case "forward":
                    x += count;
                    break;
                case "down":
                    y += count;
                    break;
                case "up":
                    y -= count;
                    break;
            }
        }

        return $"X: {x}, Y: {y}, Product: {x * y}";
    }

    protected override string Part2Implementation(string[] input) {
        int x = 0;
        int y = 0;
        int aim = 0;
        foreach (string line in input) {
            string[] components = line.Split(' ');
            string direction = components[0].ToLower();
            int count = int.Parse(components[1]);
            switch (direction) {
                case "forward":
                    x += count;
                    y += aim * count;
                    break;
                case "down":
                    aim += count;
                    break;
                case "up":
                    aim -= count;
                    break;
            }
        }

        return $"X: {x}, Y: {y}, Product: {x * y}";
    }
}