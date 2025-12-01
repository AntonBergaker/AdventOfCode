use std::{fs,time::Instant};

use colored::Colorize;

mod days;

fn main() {
    // Necessary to cast to, when function expects a Str instead of a vector
    type StrInput = fn(&str);
    /*
    evaluate_day(days::day1::day1, 1);
    evaluate_day(days::day2::day2, 2);
    evaluate_day(days::day3::day3, 3);
    evaluate_day(days::day4::day4, 4);
    evaluate_day(days::day5::day5, 5);
    evaluate_day(days::day6::day6 as StrInput, 6);
    evaluate_day(days::day7::day7, 7);
    evaluate_day(days::day8::day8, 8);
    evaluate_day(days::day9::day9, 9);
    evaluate_day(days::day10::day10, 10);
    evaluate_day(days::day11::day11 as StrInput, 11);
    evaluate_day(days::day12::day12 as StrInput, 12);
    evaluate_day(days::day13::day13, 13);
    evaluate_day(days::day14::day14, 14);
    evaluate_day(days::day15::day15, 15); */
    evaluate_day(days::day16::day16, 16);
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

// Can not be implicitly converted. It's currently not supported to have more than one fn trait.
impl DayInput for fn(&str) {
    fn evaluate(self, input: &str) {
        self(input);
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
