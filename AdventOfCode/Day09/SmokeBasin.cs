using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorInt;

namespace AdventOfCode.Day09; 

static class SmokeBasin {

    public static void Run() {
        string[] input = File.ReadAllLines(Path.Join("Day09", "input.txt"));
        int width = input[0].Length;
        int height = input.Length;
        int[,] data = new int[width, height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                data[x, y] = int.Parse(input[y][x].ToString());
            }
        }

        {
            Console.WriteLine("Smoke Basin Part 1");

            int total = GetLowestPoints(data).Select(x => data[x.X, x.Y] + 1).Sum();

            Console.WriteLine($"Sum of risk level: {total}\n");
        }
        {
            Console.WriteLine("Smoke Basin Part 2");

            var lowPoints = GetLowestPoints(data);

            // Make an array of bools to mark where we've been
            bool[,] visited = new bool[width, height];
            List<int> basinSizes = new List<int>();

            foreach (VectorInt2 lowPoint in lowPoints) {
                int size = 0;

                Stack<VectorInt2> nextPositions = new();
                nextPositions.Push(lowPoint);

                while (nextPositions.Count > 0) {
                    VectorInt2 pos = nextPositions.Pop();
                    var nearby = GetNearbyCoords(pos, width, height);
                    foreach (VectorInt2 near in nearby) {
                        if (visited[near.X, near.Y]) {
                            continue;
                        }

                        if (data[near.X, near.Y] == 9) {
                            continue;
                        }

                        visited[near.X, near.Y] = true;
                        size++;
                        nextPositions.Push(near);
                    }
                }

                basinSizes.Add(size);
            }

            basinSizes.Sort();
            int productOfBasinSizes = basinSizes.Take(^3..).Aggregate(1, (i, x) => i * x);

            Console.WriteLine($"Product of largest basin sizes: {productOfBasinSizes}\n");
        }

    }

    private static List<VectorInt2> GetNearbyCoords(VectorInt2 pos, int width, int height) {

        List<VectorInt2> list = new();
        if (pos.X > 0) {
            list.Add(pos + new VectorInt2(-1, 0));
        }
        if (pos.X < width-1) {
            list.Add(pos + new VectorInt2(1, 0));
        }
        if (pos.Y > 0) {
            list.Add(pos + new VectorInt2(0, -1));
        }
        if (pos.Y < height-1) {
            list.Add(pos + new VectorInt2(0, 1));
        }


        return list;
    }

    private static List<VectorInt2> GetLowestPoints(int[,] heightMap) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        List<VectorInt2> points = new List<VectorInt2>();

        // Lol I hate it
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var nearby = GetNearbyCoords(new(x, y), width, height);
                int min = nearby.Select(near => heightMap[near.X, near.Y]).Min();

                if (heightMap[x, y] < min) {
                    points.Add(new(x, y));
                }
            }
        }

        return points;
    }

}