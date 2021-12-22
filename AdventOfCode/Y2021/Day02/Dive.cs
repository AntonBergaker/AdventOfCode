using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day02; 

static class Dive {

    public static void Run() {
        string[] lines = File.ReadAllLines(Path.Join("Day02", "input.txt"));
        {
            Console.WriteLine("Dive Part 1");
            int x = 0;
            int y = 0;
            foreach (string line in lines) {
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

            Console.WriteLine($"X: {x}, Y: {y}, Product: {x * y}\n");
        }

        {
            Console.WriteLine("Dive Part 2");
            int x = 0;
            int y = 0;
            int aim = 0;
            foreach (string line in lines) {
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

            Console.WriteLine($"X: {x}, Y: {y}, Product: {x * y}\n");
        }
    }

}