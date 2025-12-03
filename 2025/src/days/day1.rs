use crate::{days::day::Day};

pub struct Day1;

impl Day for Day1 {
    type Input = Vec<i32>;

    fn import(input: &str) -> Vec<i32> {
        return input.trim().lines().map(|x| {
            let (direction, steps) =
                scan_fmt!(x, "{[LR]}{}", char, i32).expect("Invalid format");
            let step_mod = if direction == 'L' {-1} else {1};
            return steps * step_mod;
        }).collect();
    }

    fn part1(input_lines: &Vec<i32>) -> String {
        let times_at_0 = evaluate(input_lines,&|clock, steps| {
            *clock += steps;
            *clock %= 100;

            if *clock == 0 {
                return 1;
            }
            return 0;
        });

        return highlight_part!("Times clock stopped at 0: {}", times_at_0);
    }

    fn part2(input_lines: &Vec<i32>) -> String {
        let times_at_0 = evaluate(input_lines,&|clock, steps| {
            let sign = i32::signum(steps);
            let count = i32::abs(steps);

            let mut times_passed_0 = 0;

            for _ in 0..count {
                *clock += sign;
                *clock %= 100;

                if *clock == 0 {
                    times_passed_0 += 1;
                }
            }

            return times_passed_0;
        });

        return highlight_part!("Times clock went past 0: {}", times_at_0);
    }
}

fn evaluate(input_lines: &Vec<i32>, count_fun: &dyn Fn(&mut i32, i32) -> i32) -> i32 {
    let mut clock = 50;
    let mut times_at_0 = 0;

    for line in input_lines {
        times_at_0 += count_fun(&mut clock, *line)
    }

    return times_at_0;
}