pub trait Day {
    type Input;

    fn import(lines: &str) -> Self::Input;
    fn part1(input: &Self::Input) -> String;
    fn part2(input: &Self::Input) -> String;
}
