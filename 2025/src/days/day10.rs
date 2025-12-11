use good_lp::{Expression, Solution, SolverModel, Variable, variable, variables};

use crate::days::day::Day;

pub struct Day10;

impl Day for Day10 {
    type Input = Vec<(u32, Vec<Vec<usize>>, Vec<i32>)>;
    fn import(input: &str) -> Self::Input {
        return input.lines().map(|line| {
            let parts: Vec<&str> = line.split(' ').collect();
            
            let mut expected_indicators: u32 = 0;
            let indicator_chars: Vec<char> = parts[0][1 .. parts[0].len()-1].chars().collect();
            
            for i in 0 .. indicator_chars.len() {
                if indicator_chars[i] == '#' {
                    expected_indicators |= 1 << i;
                }
            }
            
            let wiring_schematics: Vec<Vec<usize>> = parts[1..parts.len()-1].iter()
                .map(|w| w[1..w.len()-1].split(',').map(|n| str::parse(n).unwrap()).collect::<Vec<usize>>()).collect();

            let last_entry = parts[parts.len() - 1];
            let expected_voltage: Vec<i32> = last_entry[1 .. last_entry.len()-1].split(',').map(|x| x.parse().unwrap()).collect();

            return (expected_indicators, wiring_schematics, expected_voltage);
        }).collect();
    }
    
    fn part1(input: &Self::Input) -> String {
        let mut sum = 0;
        for line in input {
            let (expected_value, flips, _) = line;
            sum += try_flip(0, *expected_value, &flips[..], 0);
        }

        return highlight_part!("Sum of minimum presses to match indicators: {}", sum);
    }
    
    fn part2(input: &Self::Input) -> String {
        let mut sum_of_presses = 0;
        for line in input {
            let (_, inputs, target) = line;
            let expanded_inputs: Vec<Vec<i32>> = inputs.iter().map(|x| {
                let mut expanded_input = vec![0; target.len()];
                for index in x {
                    expanded_input[*index] = 1;
                }
                expanded_input
            }).collect();

            if let Some(solution) = try_solve(&expanded_inputs, &target) {
                sum_of_presses += solution as i64;
            } else {
                panic!("AAAA");
            }
        }

        return highlight_part!("Sum of minimum presses to match joltage: {}", sum_of_presses);
    }
}

fn try_flip(current_value: u32, expected_value: u32, to_test: &[Vec<usize>], flips: i32) -> i32 {
    if expected_value == current_value {
        return flips;
    }
    if to_test.len() == 0 {
        return i32::MAX;
    }

    let unchanged = try_flip(current_value, expected_value, &to_test[1..], flips);
    
    let mut new_value = current_value;
    for bit in &to_test[0] {
        new_value ^= 1 << *bit;
    }
    let changed = try_flip(new_value, expected_value, &to_test[1..], flips+1);

    return i32::min(unchanged, changed);
}

fn try_solve(inputs: &[Vec<i32>], target: &[i32]) -> Option<i32> {
    // Create variables
    let mut vars = variables!();
    let k_vars: Vec<Variable> = inputs.iter().map(|_| vars.add(variable().min(0).integer())).collect();

    // Objective: minimize sum(k_i)
    let objective: Expression = k_vars.iter().copied().sum();

    // Build model
    let mut problem = vars.minimise(&objective).using(good_lp::microlp);

    // Constraints: for each dimension j:
    //   sum_i k_i * input[i][j] == target[j]
    for j in 0..target.len() {
        let mut expr = Expression::from(0);
        for i in 0..k_vars.len() {
            expr = expr + k_vars[i] * inputs[i][j];
        }
        problem = problem.with(expr.eq(target[j]));
    }

    // Solve
    let solution = problem.solve().ok()?;

    return Some(solution.eval(&objective).round() as i32);
}