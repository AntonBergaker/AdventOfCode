
using Pastel;
using System.Drawing;

namespace AdventOfCode2024.Days;
public class Day09 : DayTextBase<int[]> {
    public override int[] Import(string input) {
        return input.Select(x => x.ToInt()).ToArray();
    }

    public override string Part1(int[] input) {
        var holes = new Queue<int>();
        var memory = new List<int>();

        var position = 0;
        var id = 0;
        var isFile = true;
        foreach (int value in input) {
            for (int i = 0; i < value; i++) {
                if (isFile) {
                    memory.Add(id);
                } else {
                    memory.Add(-1);
                    holes.Enqueue(position);
                }

                position++;
            }

            if (isFile) {
                id++;
            }
            isFile = !isFile;
        }

        while (holes.Count > 0) {
            var nextHole = holes.Dequeue();

            // Remove empty holes
            int last;
            do {
                last = memory[^1];
                memory.RemoveAt(memory.Count-1);
            } while (last == -1); // Skip all empty holes

            // Filled all, exit
            if (nextHole >= memory.Count) {
                // add it back
                memory.Add(last);
                break;
            }
            memory[nextHole] = last;
        }

        var total = memory.Select((x, i) => (long)x * (long)i).Sum();
        return $"Filesystem checksum: {total.ToString().Pastel(Color.Yellow)}";
    }

    private class MemoryHole(int position, int size) {
        public int Position = position;
        public int Size = size;
    }
    private class MemoryFile(int id, int position, int size) {
        public readonly int Id = id;
        public int Position = position;
        public readonly int Size = size;
    }

    public override string Part2(int[] input) {
        var files = new List<MemoryFile>();
        var holes = new LinkedList<MemoryHole>();

        var id = 0;
        var position = 0;
        var isFile = true;
        foreach (var size in input) {
            if (isFile) {
                files.Add(new MemoryFile(id++, position, size));
            } else {
                holes.AddLast(new MemoryHole(position, size));
            }
            
            position += size;
            isFile = !isFile;
        }

        foreach (var file in Enumerable.Reverse(files)) {
            var hole = holes.First;

            while (hole != null) {
                // Gone too far past
                if (hole.Value.Position > file.Position) {
                    hole = null;
                    break;
                }
                if (hole.Value.Size >= file.Size) {
                    break;
                }
                hole = hole.Next;
            }

            if (hole != null) {
                // Move memory
                file.Position = hole.Value.Position;

                if (file.Size >= hole.Value.Size) {
                    holes.Remove(hole);
                }
                else {
                    hole.Value.Position += file.Size;
                    hole.Value.Size -= file.Size;
                }
            }
        }

        var total = files.Select(x => {
            var s = x.Position;
            var e = x.Position + x.Size-1;
            long sumOfNumbers = (e - s + 1) * (s + e) / 2;
            return x.Id * sumOfNumbers;
        }).Sum();
        return $"Filesystem checksum: {total.ToString().Pastel(Color.Yellow)}";
    }
}
