#[macro_use] extern crate scan_fmt;

use std::{fs, time::Instant};

use colored::Colorize;

use crate::days::{day::Day};
#[macro_use]
pub mod highlight_part;
mod days;


//const ONLY_DAY: Option<usize> = None;
const ONLY_DAY: Option<usize> = Some(11);

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
    evaluate_day::<days::day11::Day11>(11);
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

    let input_path_both = format!("{}/day{}.txt", folder, index);
    
    if let Some((data, total_time)) = import_part::<D>(input_path_both.as_str()) {
        println!("{}", format!("Import took {} ms", total_time).bright_black().italic());
        println!();

        evaluate_part(D::part1, &data, 1);
        evaluate_part(D::part2, &data, 2);
    } else {
        let input_path_p1 = format!("{}/day{}p1.txt", folder, index);
        let input_path_p2 = format!("{}/day{}p2.txt", folder, index);

        if let Some((data, _)) = import_part::<D>(input_path_p1.as_str()) {
            evaluate_part(D::part1, &data, 1);
        } else {
            println!();
            println!("{}", format!("Failed to import day {} part 1", index).bright_red().italic());
            println!();
        }
        if let Some((data, _)) = import_part::<D>(input_path_p2.as_str()) {
            evaluate_part(D::part2, &data, 2);
        } else {
            println!();
            println!("{}", format!("Failed to import day {} part 2", index).bright_red().italic());
            println!();
        }
        return;
    }
}

fn import_part<D: Day>(path: &str, ) -> Option<(D::Input, u128)> {
    let input = fs::read_to_string(path).ok()?;
    let start = Instant::now();
    let data = D::import(input.as_str());
    let time = start.elapsed().as_millis();

    return Some((data, time));
}

fn evaluate_part<T, F: Fn(&T) -> String>(function: F, data: &T, part: usize) {
    let start = Instant::now();

    let result = function(data);

    println!("{} {}", format!("Part {}:", part).bright_magenta(), format!("({} ms)", start.elapsed().as_millis()).bright_black().italic() );
    println!("{}", result);

    println!();
}