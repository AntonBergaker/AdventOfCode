using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day01; 

internal class SonarSweep {
    public static void Run() {
        int[] numbers = File.ReadAllLines(Path.Join("Day01", "input.txt")).Select(x => int.Parse(x)).ToArray();
        
        {
            Console.WriteLine("Sonar Sweep Part 1");
            int previous = numbers[0];
            int timesIncreased = 0;
            for (int i = 1; i < numbers.Length; i++) {
                int number = numbers[i];
                if (number > previous) {
                    timesIncreased++;
                }

                previous = number;
            }

            Console.WriteLine($"Times increased: {timesIncreased}\n");
        }

        {
            Console.WriteLine("Sonar Sweep Part 2");
            int previous = numbers[0];
            int timesIncreased = 0;
            for (int i = 1; i < numbers.Length - 2; i++) {
                int number = numbers[i] + numbers[i + 1] + numbers[i + 2];
                if (number > previous) {
                    timesIncreased++;
                }

                previous = number;
            }

            Console.WriteLine($"Times increased: {timesIncreased}\n");
        }
    }
}

