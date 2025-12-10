#[macro_use] extern crate scan_fmt;

use std::{fs, time::Instant};

use colored::Colorize;

use crate::days::{day::Day};
#[macro_use]
pub mod highlight_part;
mod days;


//const ONLY_DAY: Option<usize> = None;
const ONLY_DAY: Option<usize> = Some(10);

fn main() {
    println!("Hello, world!");

    evaluate_day::<days::day1::Day1>(1);
    evaluate_day::<days::day2::Day2>(2);
    evaluate_day::<days::day3::Day3>(3);
    evaluate_day::<days::day4::Day4>(4);
    evaluate_day::<days::day5::Day5>(5);
    evaluate_day::<days::day6::Day6>(6);
    evaluate_day::<days::day7::Day7>(7);
    evaluate_day::<days::day8::Day8>(8);
    evaluate_day::<days::day9::Day9>(9);
    evaluate_day::<days::day10::Day10>(10);
}

fn evaluate_day<D: Day>(index: usize) {
    if ONLY_DAY != None && Some(index) != ONLY_DAY {
        return;
    }

    evaluate_day_with_data::<D>("test_input", " (test)", index);
    evaluate_day_with_data::<D>("input", "", index);
}

fn evaluate_day_with_data<D: Day>(folder: &str, message: &str, index: usize) {
    println!("{}", format!("Day {}{}:", index, message).blue().bold());

    let input_path = format!("{}/day{}.txt", folder, index);
    
    let input = match fs::read_to_string(input_path) {
        Ok(input) => input,
        Err(_) => {
            println!();
            println!("{}", format!("Failed to import day {}", index).bright_red().italic());
            println!();
            return;
        }
    };
    
    let start = Instant::now();
    let data = D::import(input.as_str());
    println!("{}", format!("Import took {} ms", start.elapsed().as_millis()).bright_black().italic());
    println!();

    evaluate_part(D::part1, &data, 1);
    evaluate_part(D::part2, &data, 2);
}

fn evaluate_part<T, F: Fn(&T) -> String>(function: F, data: &T, part: usize) {
    let start = Instant::now();

    let result = function(data);

    println!("{} {}", format!("Part {}:", part).bright_magenta(), format!("({} ms)", start.elapsed().as_millis()).bright_black().italic() );
    println!("{}", result);

    println!();
}