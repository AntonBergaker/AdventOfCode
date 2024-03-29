﻿namespace AdventOfCode.Y2021.Day12;

public class PassagePathing : AocSolution<string[]> {
    public override string Name => "Passage Pathing";

    private class Graph {

        private readonly Dictionary<string, Node> nodes;

        public Node this[string name] => nodes[name];

        public Graph(string[] input) {
            nodes = input.SelectMany(x => x.Split("-")).Distinct().Select(x => new Node(x)).ToDictionary(x => x.Name);

            void Connect(Node n0, Node n1) {
                n0.Edges.Add(n1);
                n1.Edges.Add(n0);
            }

            foreach (string str in input) {
                string[] sides = str.Split("-");
                Connect(this[sides[0]], this[sides[1]]);
            }
        }

    }

    private class Node {
        public readonly string Name;
        public readonly List<Node> Edges;
        public readonly bool IsBigCave;

        public Node(string name) {
            Name = name;
            Edges = new();
            IsBigCave = name.ToUpperInvariant() == name;
        }

        public override string ToString() {
            return Name;
        }
    }

    protected override string Part1Implementation(string[] input) {
        Graph graph = new Graph(input);
        List<List<Node>> paths = new();

        HashSet<Node> visited = new();
        Stack<Node> path = new();
        void Visit(Node node) {
            if (node.Name == "end") {
                paths.Add(path.Append(node).ToList());
                return;
            }
            if (node.IsBigCave == false && visited.Contains(node)) {
                return;
            }

            visited.Add(node);
            path.Push(node);
            foreach (Node edge in node.Edges) {
                Visit(edge);
            }
            visited.Remove(node);
            path.Pop();
        }

        Visit(graph["start"]);

        return $"Number of unique paths: {paths.Count}";
    }

    protected override string Part2Implementation(string[] input) {
        Graph graph = new Graph(input);
        List<List<Node>> paths = new();

        HashSet<Node> visited = new();
        Stack<Node> path = new();
        bool spentTwoVisit = false;

        void Visit(Node node) {
            if (node.Name == "end") {
                paths.Add(path.Reverse().Append(node).ToList());
                return;
            }

            bool onSecondVisit = false;
            bool previousVisitValue = spentTwoVisit;

            if (node.IsBigCave == false && visited.Contains(node)) {
                if (node.Name == "start") {
                    return;
                }

                if (spentTwoVisit == false) {
                    onSecondVisit = true;
                    spentTwoVisit = true;
                }
                else {
                    return;
                }
            }

            visited.Add(node);
            path.Push(node);

            foreach (Node edge in node.Edges) {
                Visit(edge);
            }

            spentTwoVisit = previousVisitValue;
            if (onSecondVisit == false) {
                visited.Remove(node);
            }

            path.Pop();
        }

        Visit(graph["start"]);

        return $"Number of unique paths: {paths.Count}";
    }
}