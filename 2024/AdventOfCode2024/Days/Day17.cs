
using InterpolatedParsing;
using Microsoft.Win32;
using Pastel;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2024.Days;
public class Day17 : DayLineBase<Day17.StartupData> {
    public record StartupData(ulong A, ulong B, ulong C, int[] Instructions);

    private enum Opcode {
        Adv = 0,
        Bxl = 1,
        Bst = 2,
        Jnz = 3,
        Bxc = 4,
        Out = 5,
        Bdv = 6,
        Cdv = 7
    }

    public override StartupData Import(string[] input) {
        var splitLine = Array.IndexOf(input, "");
        var registerValues = input[..splitLine].Select(x => {
            string register = null!;
            ulong value = 0;
            InterpolatedParser.Parse(x, $"Register {register}: {value}");
            return value;
        }).ToArray();

        int[] instructions = null!;
        InterpolatedParser.Parse(input[splitLine + 1], $"Program: {instructions:,}");

        return new(registerValues[0], registerValues[1], registerValues[2], instructions);
    }

    public override string Part1(StartupData input) {
        var (registerA, registerB, registerC, instructions) = input;
        List<int> @out = RunProgram(registerA, registerB, registerC, instructions, null);
        var a = TestSimplifiedProgram(registerA);

        return $"Program output: {string.Join(',', @out).Pastel(Color.Yellow)}";
    }

    public override string Part2(StartupData input) {
        var (_, registerB, registerC, instructions) = input;

        if (input.Instructions.Length <= 6) {
            return "Not relevant for test data.";
        }
        // ######################
        // ##### DISCLAIMER #####
        // ######################
        // This solution is dependent on my input. Depending on how the problem is made, this might not work for other inputs

        // The program runs until A is 0, outputting one value each iteration. Nothing modifies A except itself, dividing by 8 each iteration.
        // The program is 8 operations, which needs 16 instructions.
        // This means the initial value of A must be between A >= 8^15 < A 8^16 (0x200000000000 <= A <= 0x1000000000000)

        // The final value of the printed value is:
        // B = A % 8
        // B = B ^ 1
        // C = A / 2^B
        // B = (B ^ C ^ 4) % 8

        // Print(B % 8)

        // Every value of both C and B are extracted from A.
        // B = last 3 bits of A
        // C = Extract 3 bits from anywhere upwards of 11 (8+3) bits
        // A/8 = next 3 bits of A
        // So only a subset of 11 bits are looked at to extract a given digit

        var lowerEnd = (ulong)Math.Pow(8, 15);
        var higherEnd = (ulong)Math.Pow(8, 16);

        var count = higherEnd - lowerEnd;

        var target = Convert.ToUInt64("2411750314455530", 8);

        // Start heuristics with a random number
        var rnd = new Random();
        var workingInput = (ulong)Random.Shared.NextInt64((long)lowerEnd, (long)higherEnd);
        var workingOutput = TestSimplifiedProgram(workingInput);
        var workingScore = CalculateScore(workingOutput, target);

        var generationInput = workingInput;
        var generationOutput = workingOutput;
        var generationScore = workingScore;

        var possibleAnswers = new HashSet<ulong>();
        var generations = 50;

        // Run for a certain number of generations, keeping the best number after each generation
        for (var generationIndex = 0; generationIndex < generations; generationIndex++) {
            workingInput = generationInput;
            workingOutput = generationOutput;
            workingScore = generationScore;
            //Console.WriteLine($"Target: {Convert.ToString((long)target, 8)} Working Output: {Convert.ToString((long)workingOutput, 8)} Score: {workingScore}/16");

            for (var i = 0; i < 100_000; i++) {
                var changeNumber = rnd.Next(16);

                // If already right, don't change it
                var extractedBits = (workingOutput >> (changeNumber * 3)) & 7;
                var targetBits = (target >> (changeNumber * 3)) & 7;

                // Prefer to change bits that will have effect, but allow some entropy
                if (extractedBits == targetBits || rnd.Next(10) == 0) {
                    //continue;
                }

                // Pick a random bit to flip
                var bitFlip = Math.Clamp(changeNumber * 3 + rnd.Next(15), 0, 48);
                var newInput = workingInput ^ (1ul << bitFlip);
                var newOutput = TestSimplifiedProgram(newInput);

                if (newOutput == target) {
                    possibleAnswers.Add(newInput);
                }

                var newScore = CalculateScore(newOutput, target);

                if (newScore > workingScore || rnd.Next(10) == 0) {
                    workingInput = newInput;
                    workingOutput = newOutput;
                    workingScore = newScore;

                    if (newScore >= generationScore) {
                        generationInput = newInput;
                        generationOutput = newOutput;
                        generationScore = Math.Max(generationScore, newScore);
                    }
                }
            }
        }

        return $"Maybe smallest A register value that outputs the program: {possibleAnswers.Min().ToString().Pastel(Color.Yellow)}";
    }

    private static List<int> RunProgram(ulong registerA, ulong registerB, ulong registerC, int[] instructions, int[]? expectedPrints) {
        var @out = new List<int>();

        var instructionPointer = 0;
        while (instructionPointer < instructions.Length) {
            var opcode = (Opcode)instructions[instructionPointer];
            var operand = instructions[instructionPointer + 1];

            switch (opcode) {
                case Opcode.Adv:
                case Opcode.Bdv:
                case Opcode.Cdv:
                    var denominator = (ulong)Math.Pow(2, GetComboOperand(operand));
                    var result = registerA / denominator;
                    if (opcode == Opcode.Adv) {
                        registerA = result;
                    } else if (opcode == Opcode.Bdv) {
                        registerB = result;
                    } else {
                        registerC = result;
                    }
                    break;
                case Opcode.Bxl:
                    registerB = registerB ^ (ulong)operand;
                    break;
                case Opcode.Bst:
                    registerB = GetComboOperand(operand) % 8;
                    break;
                case Opcode.Jnz:
                    if (registerA == 0) {
                        break;
                    }
                    instructionPointer = operand - 2; // -2 because 2 always gets added end of loop
                    break;
                case Opcode.Bxc:
                    registerB = registerB ^ registerC;
                    break;
                case Opcode.Out:
                    var outVal = (int)(GetComboOperand(operand) % 8);
                    @out.Add(outVal);
                    if (expectedPrints != null && (
                        expectedPrints.Length <= @out.Count ||
                        expectedPrints[@out.Count] != outVal)) {
                        goto outer_break;
                    }
                    break;

            }

            instructionPointer += 2;
        }
        outer_break:

        ulong GetComboOperand(int operand) {
            return operand switch {
                >= 0 and <= 3 => (ulong)operand,
                4 => registerA,
                5 => registerB,
                6 => registerC,
                _ => throw new NotImplementedException()
            };
        }

        return @out;
    }

    private static ulong TestSimplifiedProgram(ulong a) {
        // B = A % 8
        // B = B ^ 1
        // C = A / 2^B
        // B = (B ^ 4 ^ C) % 8

        // Print(B % 8)

        ulong @out = 0;
        for (int i = 0; i < 16; i++) {
            uint b = (uint)(a & 0x7); 
            b = b ^ 1;
            uint c = (uint)((a >> (int)b) & 0x7);
            a >>= 3;
            b = (b ^ c ^ 4);

            @out <<= 3;
            @out |= (ulong)b & 0x7;
        }

        return @out;
    }

    private static int CalculateScore(ulong value, ulong target) {
        int score = 0;
        for (int i = 0; i < 16; i++) {
            var valueBits = value & 0x7;
            var targetBits = target & 0x7;

            if (valueBits == targetBits) {
                score++;
            }
            value >>= 3;
            target >>= 3;
        }
        return score;
    }
}
