using System.Numerics;

namespace AdventOfCode.Y2021.Day10;

public class SyntaxScoring : AocSolution<string[]> {

    public override string Name => "Syntax Scoring";

    private Dictionary<char, char> openToClose;
    private Dictionary<char, char> closeToOpen;
    private Dictionary<char, int> closeToErrorScore;
    private Dictionary<char, int> closeToAutocompleteScore;


    public SyntaxScoring() {
        (string pairs, int errorScore, int autocompleteScore)[] pairs = {
            ("()", 3, 1),
            ("[]", 57, 2),
            ("{}", 1197, 3),
            ("<>", 25137, 4),
        };
        openToClose = pairs.ToDictionary(x => x.pairs[0], x => x.pairs[1]);
        closeToOpen = pairs.ToDictionary(x => x.pairs[1], x => x.pairs[0]);
        closeToErrorScore = pairs.ToDictionary(x => x.pairs[1], x => x.errorScore);
        closeToAutocompleteScore = pairs.ToDictionary(x => x.pairs[1], x => x.autocompleteScore);
    }

    private (BigInteger errorScore, List<BigInteger> autoCompleteScores) GetSyntaxScores(string[] lines) {
        BigInteger errorScoreTotal = 0;
        List<BigInteger> autoCompleteScores = new();

        foreach (string line in lines) {
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

        outer_continue:;
        }

        return (errorScoreTotal, autoCompleteScores);
    }

    protected override string Part1Implementation(string[] input) {
        var (errorScoreTotal, _) = GetSyntaxScores(input);
        return $"Syntax error score: {errorScoreTotal}";
    }

    protected override string Part2Implementation(string[] input) {
        var (_, autoCompleteScores) = GetSyntaxScores(input);
        autoCompleteScores.Sort();
        return $"Syntax error score: {autoCompleteScores[autoCompleteScores.Count / 2]}";
    }
}