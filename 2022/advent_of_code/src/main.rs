use std::{fs};

mod day1;
mod day2;
mod day3;
mod day4;
mod day5;

fn main() {
    // Necessary to cast to, when function expects a Str instead of a vector
    type StrInput = fn(&str);

    evaluate_day(day1::day1, 1);
    evaluate_day(day2::day2, 2);
    evaluate_day(day3::day3, 3);
    evaluate_day(day4::day4, 4);
    evaluate_day(day5::day5, 5);
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
    println!("Day {}:", day);

    let input_path = format!("input/day{}.txt", day);
    let input = fs::read_to_string(input_path).expect(&format!("Failed to load input for day {}", day));
    
    function.evaluate(&input);
    println!();
}
