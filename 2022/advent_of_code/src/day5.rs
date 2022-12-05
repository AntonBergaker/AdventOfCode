use std::{iter};

pub fn day5(input_lines: Vec<&str>) {
    implementation(&input_lines, part1_crane);
    implementation(&input_lines, part2_crane);
}

fn implementation(input_lines: &Vec<&str>, crane_function: fn(&mut Vec<Vec<char>>, usize, usize, usize)) {
    const BOX_INPUT_WIDTH: usize = 4;

    let stack_count = (input_lines[0].chars().count() + 1)/BOX_INPUT_WIDTH;

    // Create stacks 0-9
    let mut stacks: Vec<Vec<char>> = iter::repeat(vec![]).take(stack_count).collect();

    let mut index_of_identifiers_option = None;

    for (i, line) in input_lines.iter().enumerate() {
        if line.contains("[") || line.contains("move") {
            continue;
        }
        index_of_identifiers_option = Some(i);
        break;
    }

    let index_of_identifiers = index_of_identifiers_option.expect("No identifier found");

    for line in input_lines[0..index_of_identifiers].iter().rev() {
        let line_chars: Vec<char> = line.chars().collect();
        for line_i in 0..stacks.len() {
            let box_or_space = line_chars[(line_i*BOX_INPUT_WIDTH) + 1];
            if box_or_space == ' ' {
                continue;
            }

            stacks[line_i].push(box_or_space);
        }
    }

    for line in input_lines[index_of_identifiers+2 ..].iter() {
        let words: Vec<&str> = line.split(' ').collect();

        let count = words[1].parse::<usize>().unwrap();
        let from = words[3].parse::<usize>().unwrap() - 1;
        let to = words[5].parse::<usize>().unwrap() - 1;

        crane_function(&mut stacks, count, from, to);
    }

    let mut output: String = String::new();
    for stack in stacks {
        output.push(stack[stack.len()-1]);
    }

    println!("Letters on top of stacks: {}", output);
}

fn part1_crane(stacks: &mut Vec<Vec<char>>, count: usize, from: usize, to: usize) {
    for _ in 0..count {
        let box_char = stacks[from].pop().expect("Didn't have a box");
        stacks[to].push(box_char);
    }
}

fn part2_crane(stacks: &mut Vec<Vec<char>>, count: usize, from: usize, to: usize) {
    let mut crane_inventory: Vec<char> = Vec::new();

    // Remove amount from stack and put in crane inventory
    for _ in 0..count {
        crane_inventory.push(stacks[from].pop().expect("Didn't have a box")); 
    }

    // Put crane inventory down
    while crane_inventory.is_empty() == false {
        stacks[to].push(crane_inventory.pop().expect("Didn't have a box")); 
    }
}