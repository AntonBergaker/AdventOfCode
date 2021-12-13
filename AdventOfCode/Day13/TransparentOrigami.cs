using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorInt;

namespace AdventOfCode.Day13; 

static class TransparentOrigami {

    public static void Run() {
        string[] input = File.ReadAllLines(Path.Join("Day13", "input.txt"));
        int dotsCount = Array.IndexOf(input, "");
        VectorInt2[] dots = input[..dotsCount].Select(x => x.Split(',')).Select(x => new VectorInt2(int.Parse(x[0]), int.Parse(x[1]))).ToArray();
        int width = dots.Select(x => x.X).Max()+1;
        int height = dots.Select(x => x.Y).Max()+1;
        string[] instructions = input[(dotsCount + 1) ..];

        Grid<bool> defaultGrid = new(width, height);
        foreach (VectorInt2 dot in dots) {
            defaultGrid[dot] = true;
        }

        {
            Console.WriteLine("Transparent Origami Part 1");
            var foldedGrid = Fold(defaultGrid, instructions[0]);
            
            Console.WriteLine($"Number of dots: {foldedGrid.Count(x => x)}\n");
        }

        {
            Console.WriteLine("Transparent Origami Part 2");
            var foldedGrid = defaultGrid;
            foreach (string instruction in instructions) {
                foldedGrid = Fold(foldedGrid, instruction);
            }


            Console.WriteLine("Activation code:");
            Console.WriteLine(foldedGrid.ToGridString(x => x ? "x" : " "));
            Console.WriteLine();
            
        }
    }

    private static Grid<bool> Fold(Grid<bool> grid, string instruction) {
        instruction = instruction.Remove(0, "fold along ".Length);
        bool isXAxis = instruction[0] == 'x';
        int foldPosition = int.Parse(instruction[2..]);

        Grid<bool> newGrid;
        if (isXAxis) {
            int unfoldedWidth = foldPosition;
            int foldedWidth = grid.Width - foldPosition - 1;

            newGrid = new Grid<bool>(Math.Max(unfoldedWidth, foldedWidth), grid.Height);

            // Transfer the unfolded side, go backwards so the x match up
            for (int x = 0; x < unfoldedWidth; x++) {
                for (int y = 0; y < newGrid.Height; y++) {
                    newGrid[foldPosition - x - 1, y] = grid[foldPosition - x - 1, y];
                }
            }

            // Transfer the folded side
            for (int x = 0; x < foldedWidth; x++) {
                for (int y = 0; y < newGrid.Height; y++) {
                    newGrid[newGrid.Width - x - 1, y] |= grid[foldPosition + 1 + x, y];
                }
            }
        }
        else {
            int unfoldedHeight = foldPosition;
            int foldedHeight = grid.Height - foldPosition - 1;

            newGrid = new Grid<bool>(grid.Width, Math.Max(unfoldedHeight, foldedHeight));

            // Transfer the unfolded side, go backwards so the x match up
            for (int y = 0; y < unfoldedHeight; y++) {
                for (int x = 0; x < newGrid.Width; x++) {
                    newGrid[x, newGrid.Height - y - 1] = grid[x, foldPosition - y - 1];
                }
            }

            // Transfer the folded side
            for (int y = 0; y < foldedHeight; y++) {
                for (int x = 0; x < newGrid.Width; x++) {
                    newGrid[x, newGrid.Height - y - 1] |= grid[x, foldPosition + 1 + y];
                }
            }
        }

        return newGrid;
    }
}