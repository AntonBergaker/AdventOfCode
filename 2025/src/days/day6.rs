use std::{error::Error};

use common::data_types::{grid::Grid, point::Point};

use crate::days::day::Day;

pub struct Day6;

#[derive(PartialEq)]
pub enum Operator {
    Add,
    Multiply
}

impl Operator {
    fn from_char(s: char) -> Result<Self, Box<dyn Error>> {
        return match s {
            '+' => Ok(Operator::Add),
            '*' => Ok(Operator::Multiply),
            _ => Err("Invalid operator")?
        };
    }
}

type Problem = (Grid<char>, Operator);

impl Day for Day6 {
    type Input = Vec<Problem>;
    
    fn import(lines: &str) -> Self::Input {
        let grid = Grid::from(lines, |x| x);

        let mut problems = Vec::new();

        // Slice the grid based on the positions of the operators
        let last_y = grid.height()-1;
        let mut current_index = 0;
        let mut current_op = grid[Point::new(0, last_y)];
        for i in 1..grid.width() {
            let op = grid[Point::new(i, last_y)];
            if op != ' ' {
                problems.push(import_problem(&grid, current_op, current_index, i-1));
                current_op = op;
                current_index = i;
            }
        }

        problems.push(import_problem(&grid, current_op, current_index, grid.width()));
        
        return problems;
    }
    
    fn part1(input: &Self::Input) -> String {
        let mut total = 0;
        for problem in input {
            total += calculate_problem(&problem.0, &problem.1);
        }

        return highlight_part!("Total sum of problems: {}", total);
    }
    
    fn part2(input: &Self::Input) -> String {
        let mut total = 0;
        for problem in input {
            let grid = problem.0.rotate().rotate().rotate();
            total += calculate_problem(&grid, &problem.1);
        }

        return highlight_part!("Total sum of problems: {}", total);
    }
}

fn import_problem(grid: &Grid<char>, op: char, start: usize, end: usize) -> Problem {

    return (grid.clone_region(Point::new(start, 0), Point::new(end, grid.height()-1)), Operator::from_char(op).unwrap());
}

fn import_numbers(grid: &Grid<char>) -> Vec<i64> {
    let mut numbers = Vec::new();

    for y in 0..grid.height() {
        let mut num_str_chars = Vec::new();
        for x in 0..grid.width() {
            let char = grid[Point::new(x, y)];
            if char != ' ' {
                num_str_chars.push(grid[Point::new(x, y)]);
            }
        }

        let num_str: String = num_str_chars.iter().collect();
        numbers.push(num_str.parse::<i64>().unwrap());
    }

    return numbers;
}

fn calculate_problem(grid: &Grid<char>, operator: &Operator) -> i64 {
    let numbers = import_numbers(grid);
    if *operator == Operator::Add {
        return numbers.iter().sum::<i64>();
    } else {
        return numbers.iter().fold(1, |a, b| a * b);
    }
}