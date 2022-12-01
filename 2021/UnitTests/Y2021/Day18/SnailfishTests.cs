using AdventOfCode;
using AdventOfCode.Y2021.Day18;
using NUnit.Framework;
using System;

namespace UnitTest.Y2021.Day18;

 static class SnailfishHelpers {
    public static Snailfish.SnailNumber LeftChild(this Snailfish.SnailNumber number) {
        if (number is not Snailfish.Pair pair) {
            throw new Exception("Not a pair");
        }
        return (pair.Left);
    }

    public static Snailfish.SnailNumber RightChild(this Snailfish.SnailNumber number) {
        if (number is not Snailfish.Pair pair) {
            throw new Exception("Not a pair");
        }
        return (pair.Right);
    }
}

internal class SnailfishTests {


    [Test]
    public void TestSnailfishGetLeft() {
        {
            var pair = new Snailfish.Pair(
                new Snailfish.NumberElement(1),
                new Snailfish.NumberElement(2)
            );

            Assert.AreEqual(pair.Left, Snailfish.GetLeft(pair.Right));
        }

        {
            var pair = new Snailfish.Pair(
                new Snailfish.NumberElement(2),
                new Snailfish.Pair(
                    new Snailfish.NumberElement(1),
                    new Snailfish.NumberElement(3)
                )
            );

            Assert.AreEqual(pair.Left, Snailfish.GetLeft(pair.Right.LeftChild()));

            Assert.AreEqual(pair.Right.LeftChild(), Snailfish.GetLeft(pair.Right.RightChild()));
        }

        {
            var pair = new Snailfish.Pair(
                new Snailfish.Pair(
                    new Snailfish.NumberElement(2),
                    new Snailfish.NumberElement(4)
                ),
                new Snailfish.Pair(
                    new Snailfish.NumberElement(1),
                    new Snailfish.NumberElement(3)
                )
            );

            Assert.AreEqual(pair.Left.RightChild(), Snailfish.GetLeft(pair.Right.LeftChild()));
        }
    }

    [Test]
    public void TestReduceExplode() {
       void AssertReducedToExpected(string expected, string input) {
            var number = Snailfish.ParseString(input);
            Snailfish.Reduce(number);
            Assert.AreEqual(expected, number.ToString());
        }
        
        AssertReducedToExpected("[[[[0,9],2],3],4]", "[[[[[9,8],1],2],3],4]");
        AssertReducedToExpected("[7,[6,[5,[7,0]]]]", "[7,[6,[5,[4,[3,2]]]]]");
        AssertReducedToExpected("[[6,[5,[7,0]]],3]", "[[6,[5,[4,[3,2]]]],1]");
        AssertReducedToExpected("[[3,[2,[8,0]]],[9,[5,[7,0]]]]", "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]");
    }

    [Test]
    public void TestReduceSplit() {
        var number = Snailfish.ParseString("[4,11]");
        Snailfish.Reduce(number);
        Assert.AreEqual("[4,[5,6]]", number.ToString());
    }


    [Test]
    public void TestMagnitude() {
        var pair = new Snailfish.Pair(
            new Snailfish.Pair(
                new Snailfish.NumberElement(1),
                new Snailfish.NumberElement(2)
            ),
            new Snailfish.Pair(
                new Snailfish.Pair(
                    new Snailfish.NumberElement(3),
                    new Snailfish.NumberElement(4)
                ),
                new Snailfish.NumberElement(5)
            )
        );

        Assert.AreEqual(143L, pair.GetMagnitude());
    }

    [Test]
    public void TestSampleInput() {
        var snailfish = new Snailfish();

        var sample =
            "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]\n" +
            "[[[5,[2,8]],4],[5,[[9,9],0]]]\n" +
            "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]\n" +
            "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]\n" +
            "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]\n" +
            "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]\n" +
            "[[[[5,4],[7,7]],8],[[8,3],8]]\n" +
            "[[9,3],[[9,9],[6,[4,9]]]]\n" +
            "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]\n" +
            "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]\n";


        Assert.AreEqual(4140L, snailfish.GetSumForInput(sample.SplitLines()));
    }
}
