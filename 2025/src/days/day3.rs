use crate::days::day::Day;

pub struct Day3;


impl Day for Day3 {
    type Input = Vec<Vec<i32>>;
    fn import(input: &str) -> Vec<Vec<i32>> {
        return input.lines()
            .map(|x| x.chars().map(|c| c.to_digit(10).unwrap() as i32).
            collect()).collect();
    }

    fn part1(input: &Self::Input) -> String {
        let sum = get_sum_of_sequence(input, 2);
        return highlight_part!("Sum of biggest number formed from 2 in sequence: {}", sum )
    }

    fn part2(input: &Self::Input) -> String {
        let sum = get_sum_of_sequence(input, 12);
        return highlight_part!("Sum of biggest number formed from 12 in sequence: {}", sum )
    }
}

fn get_sum_of_sequence(input: &Vec<Vec<i32>>, count: usize) -> i64 {
    let mut sum: i64 = 0;
    
    for line in input {
        let mut line_sum: i64 = 0;
        let mut start_index = 0;

        for i in (0..count).rev() {
            // Get a slice to check, between our previous number from the start, and how many numbers are left at the end (so we always have enough to fill up.)
            let slice = &line[start_index .. (line.len()-i)];
            let new_index = find_index_of_biggest(slice) + start_index;

            line_sum *= 10;
            line_sum += line[new_index] as i64;

            start_index = new_index + 1;
        }

        sum += line_sum;
    }

    return sum;
}

fn find_index_of_biggest(vec: &[i32]) -> usize {
    let mut biggest: i32 = 0;
    let mut biggest_index: usize = 0;

    for (index, number) in vec.iter().enumerate() {
        if *number > biggest {
            biggest = *number;
            biggest_index = index;
        }
    }

    return biggest_index;
}