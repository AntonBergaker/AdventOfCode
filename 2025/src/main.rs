#[macro_use] extern crate scan_fmt;
use std::{fs, time::Instant};

use colored::Colorize;

mod days;

fn main() {
    println!("Hello, world!");

    evaluate_day(days::day1::part1, 1);
    evaluate_day(days::day1::part2, 1);

    evaluate_day(days::day2::part1 as fn(&str), 2);
    evaluate_day(days::day2::part2 as fn(&str), 2);
}

pub trait DayInput {
    fn evaluate(self, input: &str);
}

// Can be implicitly converted
impl<F: Fn(Vec<&str>)> DayInput for F {
    fn evaluate(self, input: &str) {
        let input_lines: Vec<&str> = input.lines().collect();
        self(input_lines);
    }
}

impl DayInput for fn(&str) {
    fn evaluate(self, input: &str) {
        self(input.trim());
    }
}

fn evaluate_day<T: DayInput>(function: T, day: i32) {
    println!("{}", format!("Day {}:", day).blue().bold());
    println!();

    let input_path = format!("input/day{}.txt", day);
    let input = fs::read_to_string(input_path).expect(&format!("Failed to load input for day {}", day));
    
    let start = Instant::now();

    function.evaluate(&input);

    println!();
    println!("{}", format!("Took: {} ms", start.elapsed().as_millis()).bright_black().italic());
    println!();
}
