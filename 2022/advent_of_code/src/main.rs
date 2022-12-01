use std::fs;

fn main() {
    let input = fs::read_to_string("input/day1.txt").expect("Failed to load input.");
    let input_lines = input.split("\n");

    let mut calories: Vec<i32> = Vec::new();

    {
        let mut current_calories = 0;
        for line in input_lines {
            if line.is_empty() {
                calories.push(current_calories);
                current_calories = 0;
                continue;
            }
            let calories = line.parse::<i32>().unwrap();
            current_calories += calories;
        }
        calories.push(current_calories);
    }

    calories.sort();

    let top_three = match calories[calories.len()-3 ..]  {
        [ a, b, c] => a + b + c,
        _ => 0,
    };

    println!("{}", top_three);
}
