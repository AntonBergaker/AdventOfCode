namespace AdventOfCode.Y2021.Day03;

public class BinaryDiagnostic : AocSolution<string[]> {
    public override string Name => "Binary Diagnostics";

    protected override string Part1Implementation(string[] lines) {
        int width = lines[0].Length;
        int height = lines.Length;

        bool[,] bits = new bool[width, height];

        for (int y = 0; y < lines.Length; y++) {
            string line = lines[y];
            for (int x = 0; x < width; x++) {
                bits[x, y] = line[x] == '1';
            }
        }

        ulong epsilon = 0;
        ulong gamma = 0;

        for (int x = 0; x < width; x++) {
            // Shift the value 1, since we want to edit the first bit
            gamma <<= 1;
            epsilon <<= 1;

            int zeros = 0;
            int ones = 0;
            for (int y = 0; y < height; y++) {
                if (bits[x, y] == false) {
                    zeros++;
                }
                else {
                    ones++;
                }
            }

            if (ones > zeros) {
                // Mark the first bit as 1
                gamma |= 1;
            }
            else {
                epsilon |= 1;
            }
        }

        // epsilon is the opposite of gamma for every value

        return $"Gamma: {gamma}, Epsilon: {epsilon}, Power consumption: {gamma * epsilon}";
    }

    protected override string Part2Implementation(string[] input) {
        ulong oxygenRating = CalculateLifeSupportRating(input, (zeros, ones) => zeros <= ones);

        ulong c02ScrubberRating = CalculateLifeSupportRating(input, (zeros, ones) => zeros > ones);

        return $"Oxygen: {oxygenRating}, co2: {c02ScrubberRating}, Life support: {oxygenRating * c02ScrubberRating}";
    }

    private delegate bool BitCriteria(int zeroes, int ones);

    private static ulong CalculateLifeSupportRating(string[] lines, BitCriteria bitCriteria) {
        List<bool[]> bits = new List<bool[]>();
        foreach (string line in lines) {
            bits.Add(line.Select(x => x == '1').ToArray());
        }


        int index = 0;
        while (bits.Count > 1) {
            int onesCount = bits.Count(bitLine => bitLine[index]);
            int zeroesCount = bits.Count - onesCount;

            bool keepBit = bitCriteria(zeroesCount, onesCount);
            bits = bits.Where(x => x[index] == keepBit).ToList();

            index++;
        }

        ulong value = 0;
        foreach (bool bit in bits[0]) {
            value = value << 1 | (bit ? 1UL : 0UL);
        }

        return value;
    }
}
