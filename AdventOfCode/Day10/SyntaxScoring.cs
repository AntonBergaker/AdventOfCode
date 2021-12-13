using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day10; 

static class SyntaxScoring {
    
    public static void Run() {
        string[] input = File.ReadAllLines(Path.Join("Day10", "input.txt"));
        (string pairs, int errorScore, int autocompleteScore)[] pairs = {
            ("()", 3, 1),
            ("[]", 57, 2),
            ("{}", 1197, 3),
            ("<>", 25137, 4),
        };
        var openToClose = pairs.ToDictionary(x => x.pairs[0], x => x.pairs[1]);
        var closeToOpen = pairs.ToDictionary(x => x.pairs[1], x => x.pairs[0]);
        var closeToErrorScore = pairs.ToDictionary(x => x.pairs[1], x => x.errorScore);
        var closeToAutocompleteScore = pairs.ToDictionary(x => x.pairs[1], x => x.autocompleteScore);

        BigInteger errorScoreTotal = 0;
        List<BigInteger> autoCompleteScores = new();

        foreach (string line in input) {
            Stack<char> expectedPop = new();
            foreach (char @char in line) {
                {
                    if (openToClose.TryGetValue(@char, out char closer)) {
                        expectedPop.Push(@closer);
                        continue;
                    }
                }
                {
                    if (closeToOpen.ContainsKey(@char)) {
                        char closer = expectedPop.Pop();
                        if (closer != @char) {
                            errorScoreTotal += closeToErrorScore[@char];
                            goto outer_continue;
                        }
                        continue;
                    }
                }

                throw new Exception("uh oh");
            }

            BigInteger autoCompleteScore = 0;
            while (expectedPop.Count > 0) {
                autoCompleteScore = autoCompleteScore * 5 + closeToAutocompleteScore[expectedPop.Pop()];
            }

            autoCompleteScores.Add(autoCompleteScore);

            outer_continue: ;
        }

        {
            Console.WriteLine("Syntax Scoring Part 1");
            Console.WriteLine($"Syntax error score: {errorScoreTotal}\n");
        }
        {
            Console.WriteLine("Syntax Scoring Part 2");
            autoCompleteScores.Sort();
            Console.WriteLine($"Syntax error score: {autoCompleteScores[autoCompleteScores.Count/2]}\n");
        }

    }

}