namespace AdventOfCode.Y2021.Day18;

public class Snailfish : AocSolution<string[]> {
    public override string Name => "Snailfish";

    protected override string Part1Implementation(string[] input) {
        return $"Magnitude of sum: {GetSumForInput(input)}";
    }

    public long GetSumForInput(string[] input) {
        SnailNumber number = ParseString(input[0]);

        for (int i = 1; i < input.Length; i++) {
            number = new Pair(number, ParseString(input[i]));
            Reduce(number);
        }
        return number.GetMagnitude();
    }

    protected override string Part2Implementation(string[] input) {
        // Test every combination!
        long largestMagnitude = 0;
        
        foreach (string leftSide in input) {
            foreach (string rightSide in input) {
                var left = ParseString(leftSide);
                var right = ParseString(rightSide);
                var sum = new Pair(left, right);
                Reduce(sum);
                long magnitude = sum.GetMagnitude();
                if (magnitude > largestMagnitude) {
                    largestMagnitude = magnitude;
                }
            }
        }

        return $"Largest magnitude of summation of two: {largestMagnitude}";
    }

    public static void Reduce(SnailNumber number) {
        bool Explode(SnailNumber number, int depth) {
            if (number is Pair pair) {
                if (depth >= 4) {
                    NumberElement? left = GetLeft(pair);
                    if (left != null) {
                        left.Value += ((NumberElement)pair.Left).Value;
                    }
                    
                    NumberElement? right = GetRight(pair);
                    if (right != null) {
                        right.Value += ((NumberElement)pair.Right).Value;
                    }

                    // Replace number with 0
                    pair.Parent?.Replace(pair, new NumberElement(0));

                    return true;
                }
                else {
                    if (Explode(pair.Left, depth + 1)) return true;
                    if (Explode(pair.Right, depth + 1)) return true;
                }
            }

            return false;
        }
        bool Split(SnailNumber number) {

            if (number is NumberElement element) {
                if (element.Value >= 10) {
                    element.Parent?.Replace(element, new Pair(
                        new NumberElement(element.Value/2),
                        new NumberElement(element.Value - element.Value/2)
                    ));

                    return true;
                }
            }
            if (number is Pair pair) {
                if (Split(pair.Left)) return true;
                if (Split(pair.Right)) return true;
            }


            return false;
        }

        // Keep reducing until it returns false
        while (true) {
            if (Explode(number, 0)) {
                continue;
            }
            if (Split(number)) {
                continue;
            }
            break;
        }
    }

    public static NumberElement? GetLeft(SnailNumber number) {
        SnailNumber lastNumber = number;
        Pair? parent = lastNumber.Parent;

        while (parent != null) {
            // If we're already leftmost, go out a step furher
            if (parent.Left == lastNumber) {
                lastNumber = parent;
                parent = parent.Parent;
                continue;
            }

            lastNumber = parent.Left;

            // If we're not leftmost, go down right side until we find our first number
            while (lastNumber is Pair childPair) {
                lastNumber = childPair.Right;
            }
            return lastNumber as NumberElement;
        }

        return null;
    }

    public static NumberElement? GetRight(SnailNumber number) {
        SnailNumber lastNumber = number;
        Pair? parent = lastNumber.Parent;

        while (parent != null) {
            // If we're already rightmost, go out a step furher
            if (parent.Right == lastNumber) {
                lastNumber = parent;
                parent = parent.Parent;
                continue;
            }

            lastNumber = parent.Right;

            // If we're not rightmost, go down left side until we find our first number
            while (lastNumber is Pair childPair) {
                lastNumber = childPair.Left;
            }
            return lastNumber as NumberElement;
        }

        return null;
    }

    public static SnailNumber ParseString(string input) {
        SnailNumber ParseStringImplementation(string input, ref int index) {
            if (input[index] == '[') {
                index++; // skip [
                SnailNumber left = ParseStringImplementation(input, ref index);
                index++; // skip comma
                SnailNumber right = ParseStringImplementation(input, ref index);
                index++; // skip ]
                Pair pair = new Pair(left, right);
                return pair;
            }
            else {
                string digits = "";
                while (index < input.Length-1) {
                    if (char.IsDigit(input[index]) == false) {
                        break;
                    }
                    digits += input[index++];
                }
                int number = int.Parse(digits);
                return new NumberElement(number);
            }
        }

        int index = 0;
        return ParseStringImplementation(input, ref index);
    }

    public abstract class SnailNumber {
        public Pair? Parent;

        public abstract long GetMagnitude();
    }

    public class Pair : SnailNumber {

        public SnailNumber Left;
        public SnailNumber Right;

        public Pair(SnailNumber left, SnailNumber right) {
            Left = left;
            Left.Parent = this;
            Right = right;
            Right.Parent = this;
        }

        public void Replace(SnailNumber old, SnailNumber @new) {
            @new.Parent = this;
            if (Left == old) {
                Left = @new;
            }
            else if (Right == old) {
                Right = @new;
            }
            else {
                throw new Exception("Old was neither child.");
            }
        }

        public override string ToString() {
            return $"[{Left},{Right}]";
        }

        public override long GetMagnitude() {
            return (Left.GetMagnitude() * 3 + Right.GetMagnitude() * 2);
        }
    }

    public class NumberElement : SnailNumber {
        public long Value;

        public NumberElement(long value) {
            Value = value;
        }

        public override string ToString() {
            return Value.ToString();
        }

        public override long GetMagnitude() {
            return Value;
        }
    }

}
