namespace AdventOfCode.Y2021.Day04;

public class GiantSquid : AocSolution<string[]> {
    public override string Name => "Giant Squid";

    private const int BingoBoardSize = 5;

    private class BingoBoard {
        public bool Won { get; private set; }
        private bool[,] marked;
        private int[,] numbers;
        private Dictionary<int, (int x, int y)> numberToPosition;

        public BingoBoard(int[,] numbers) {
            this.numbers = numbers;
            numberToPosition = new Dictionary<int, (int, int)>();
            marked = new bool[BingoBoardSize, BingoBoardSize];
            for (int y = 0; y < BingoBoardSize; y++) {
                for (int x = 0; x < BingoBoardSize; x++) {
                    numberToPosition.Add(numbers[x, y], (x, y));
                }
            }
        }

        public void MarkNumber(int number) {
            if (numberToPosition.TryGetValue(number, out (int x, int y) coordinates) == false) {
                return;
            }

            var (x, y) = coordinates;
            marked[x, y] = true;

            // Check win on x
            {
                bool won = true;
                for (int xx = 0; xx < BingoBoardSize; xx++) {
                    if (marked[xx, y] == false) {
                        won = false;
                        break;
                    }
                }

                if (won) {
                    Won = true;
                    return;
                }
            }
            // Check win on y
            {
                bool won = true;
                for (int yy = 0; yy < BingoBoardSize; yy++) {
                    if (marked[x, yy] == false) {
                        won = false;
                        break;
                    }
                }

                if (won) {
                    Won = true;
                    return;
                }
            }
        }

        public int GetSumOfUnmarkedNumbers() {
            int sum = 0;
            for (int y = 0; y < BingoBoardSize; y++) {
                for (int x = 0; x < BingoBoardSize; x++) {
                    if (marked[x, y]) {
                        continue;
                    }

                    sum += numbers[x, y];
                }
            }

            return sum;
        }
    }

    private static (int[] drawnNumbers, List<BingoBoard> boards) GetBoards(string[] lines) {
        int[] drawnNumbers = lines[0].Split(",").Select(x => int.Parse(x)).ToArray();

        List<BingoBoard> boards = new();

        for (int i = 2; i < lines.Length; i += 6) {
            int[,] boardNumbers = new int[BingoBoardSize, BingoBoardSize];
            for (int y = 0; y < BingoBoardSize; y++) {
                int x = 0;
                foreach (string numberStr in lines[i + y].Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
                    boardNumbers[x++, y] = int.Parse(numberStr);
                }
            }

            boards.Add(new BingoBoard(boardNumbers));
        }

        return (drawnNumbers, boards);
    }

    protected override string Part1Implementation(string[] lines) {
        var (drawnNumbers, boards) = GetBoards(lines);

        foreach (int number in drawnNumbers) {
            foreach (BingoBoard board in boards) {
                board.MarkNumber(number);
                if (board.Won) {
                    return $"Board score: {board.GetSumOfUnmarkedNumbers() * number}";
                }
            }
        }

        return "You are eaten by a squid";
    }

    protected override string Part2Implementation(string[] lines) {
        var (drawnNumbers, boards) = GetBoards(lines);

        foreach (int number in drawnNumbers) {
            // Make a copy of the boards so we can safely delete it without breaking enumeration
            foreach (BingoBoard board in boards.ToList()) {
                board.MarkNumber(number);
                if (board.Won) {
                    if (boards.Count == 1) {
                        return $"Board score: {board.GetSumOfUnmarkedNumbers() * number}";
                    }

                    boards.Remove(board);

                }
            }
        }

        return "You are eaten by a squid";
    }
}
