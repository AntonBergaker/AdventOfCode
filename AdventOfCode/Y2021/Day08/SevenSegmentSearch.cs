using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day08; 

static class SevenSegmentSearch {

    public static void Run() {
        string[] input = File.ReadAllLines(Path.Join("Day08", "input.txt"));

        {
            Console.WriteLine("Seven Segment Search Part 1");
            int total = 0;
            foreach (string line in input) {
                int[] outputLength = line[(line.IndexOf('|')+1)..].Split(' ').Select(x => x.Length).ToArray();
                total += outputLength.Count(x => x is 2 or 4 or 3 or 7);

            }

            Console.WriteLine($"Number of unique values: {total}\n");
        }

        {
            int total = 0;
            foreach (string line in input) {
                string[] keyStrings = line[..line.IndexOf('|')].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var keys = GetKeys(keyStrings);
                string[] output = line[(line.IndexOf('|') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int lineTotal = 0;
                foreach (string s in output) {
                    lineTotal *= 10;
                    lineTotal += keys[SortString(s)];
                }

                total += lineTotal;
            }

            Console.WriteLine("Seven Segment Search Part 2");

            Console.WriteLine($"Sum of all values: {total}\n");
        }
    }

    private static string SortString(string str) {
        return new string(str.OrderBy(c => c).ToArray());
    } 

    private static bool ContainsSegments(string segment, string subsegments) {
        return subsegments.All(x => segment.Contains(x));
    }

    private static string GetDifference(string segment, string subsegments) {
        return new string(segment.Where(x => subsegments.Contains(x) == false).ToArray());
    }


    private static Dictionary<string, int> GetKeys(string[] keyStrings) {
        // Make a copy of the keys and sort them alphabetically
        List<string> unfoundKeyList = keyStrings.Select(x => SortString(x)).ToList();
        string[] foundKeys = new string[10];

        void FindKey(int number, string key) {
            foundKeys[number] = key;
            unfoundKeyList.Remove(key);
        }

        FindKey(1, unfoundKeyList.First(x => x.Length == 2));
        FindKey(4, unfoundKeyList.First(x => x.Length == 4));
        FindKey(7, unfoundKeyList.First(x => x.Length == 3));
        FindKey(8, unfoundKeyList.First(x => x.Length == 7));

        string aSegment = GetDifference(foundKeys[7], foundKeys[1]);

        // 4 + a segment + one unknown can only be 9
        string fourAndA = SortString(aSegment + foundKeys[4]);
        FindKey(9, unfoundKeyList.First(x => x.Length == 6 && ContainsSegments(x, fourAndA)));
        string eSegment = GetDifference(foundKeys[8], foundKeys[9]);

        string gSegment = GetDifference(foundKeys[8], foundKeys[4] + eSegment + aSegment);
        // Of the remaining ones, only 0 contains ACEFG
        FindKey(0, unfoundKeyList.First(x => ContainsSegments( x, SortString( foundKeys[7] + eSegment + gSegment))));
        string dSegment = GetDifference(foundKeys[8], foundKeys[0]);
        string bSegment = GetDifference(foundKeys[4], foundKeys[1] + dSegment);

        FindKey(3, SortString(GetDifference(foundKeys[8], bSegment + eSegment)));
        // Of remaining, only 2 doesn't have b
        FindKey(2, unfoundKeyList.First(x => x.Contains(bSegment) == false));
        FindKey(5, unfoundKeyList.First(x => x.Length == 5));
        FindKey(6, unfoundKeyList.First());

        Dictionary<string, int> dictionary = new();
        for (int i = 0; i < 10; i++) {
            dictionary.Add(foundKeys[i], i);
        }

        return dictionary;
    }
}