using InterpolatedParsing;
using Pastel;
using System.Drawing;
using Lists = (int[] LeftList, int[] RightList);

namespace AdventOfCode2024.Days;
public class Day01 : DayLineBase<Lists> {
    public override Lists Import(string[] input) {
        var leftList = new List<int>();
        var rightList = new List<int>();

        foreach (var line in input) {
            int leftNumber = 0, rightNumber = 0;
            InterpolatedParser.Parse(line, $"{leftNumber}   {rightNumber}");
            leftList.Add(leftNumber);
            rightList.Add(rightNumber);
        }

        return ([.. leftList], [.. rightList]);
    }

    public override string Part1(Lists input) {
        var (leftList, rightList) = (input.LeftList.ToList(), input.RightList.ToList());
        leftList.Sort();
        rightList.Sort();

        if (leftList.Count != rightList.Count) {
            return "uh oh";
        }

        var totalAbsoluteDifference = 0;
        for (int i = 0; i < leftList.Count; i++) {
            var absoluteDifference = Math.Abs(leftList[i] - rightList[i]);
            totalAbsoluteDifference += absoluteDifference;
        }
        
        return $"Total absolute difference: {totalAbsoluteDifference.ToString().Pastel(Color.Yellow)}";
    }

    public override string Part2(Lists input) {
        var leftListSet = input.LeftList.ToHashSet();
        var rightSideGroups = input.RightList.GroupBy(x => x);

        var similarityScore = 0;
        foreach (var group in rightSideGroups) {
            if (leftListSet.Contains(group.Key) == false) {
                continue;
            }
            similarityScore += group.Key * group.Count();
        }

        return $"Lists similarity score: {similarityScore.ToString().Pastel(Color.Yellow)}";
    }
}
