use std::{collections::HashSet};

pub fn day3(input_lines: Vec<&str>) {
    part1(&input_lines);
    part2(&input_lines);
}

fn part1(input_lines: &Vec<&str>) {
    let mut sum_of_priority = 0;

    for line in input_lines {
        let mid_index = line.len() / 2;
        let left = &line[..mid_index];
        let right = &line[mid_index..];

        let left_set: HashSet<char> = string_to_set(left);
        let right_vec: Vec<char> = string_to_char_vec(right);

        let mut dupe_char = '\0';

        for char in &right_vec {
            if left_set.contains(&char) {
                dupe_char = *char;
                break;
            }
        }

        let priority = get_char_priority(dupe_char);

        sum_of_priority += priority;
    }

    println!("Sum or priority: {}", sum_of_priority)
}

fn part2(input_lines: &Vec<&str>) {
    let mut sum_of_priority = 0;

    let mut i = 0;
    while i < input_lines.len() {
        
        let line_sets: Vec<HashSet<char>> = input_lines[i .. i+3].iter().map(|f| {string_to_set(f)}).collect();

        let mut common_char = '\0';

        'outer_loop: for char in &line_sets[0] {
            for set in &line_sets[1..3] {
                if set.contains(char) == false {
                    continue 'outer_loop;
                }
            }

            common_char = *char;
            break;
        }

        let priority_of_common_char = get_char_priority(common_char);
        sum_of_priority += priority_of_common_char;

        i += 3;
    }

    println!("Sum of badge priorities: {}", sum_of_priority);

}

fn string_to_char_vec(input: &str) -> Vec<char> {
    return input.chars().collect();
}

fn string_to_set(input: &str) -> HashSet<char> {
    let input_vec: Vec<char> = string_to_char_vec(input);
    return HashSet::from_iter(input_vec.iter().cloned());
}

fn get_char_priority(char: char) -> i32 {
    return match char {
        c if c >= 'a' && c <= 'z' => ((c as u32 - 'a' as u32) + 1) as i32,
        c if c >= 'A' && c <= 'Z' => ((c as u32 - 'A' as u32) + 27) as i32,
        _ => panic!("Unknown character")
    };
}

