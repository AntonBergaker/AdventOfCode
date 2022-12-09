use std::{ops::Range};

pub fn day4(input_lines: Vec<&str>) {
    let mut total_contains_other = 0;
    let mut total_overlap = 0;

    for line in input_lines {
        let ranges: Vec<&str> = line.split(",").collect();
        let range0 = str_to_range(ranges[0]);
        let range1 = str_to_range(ranges[1]);

        let range0_contains_other = range0.start <= range1.start && range0.end >= range1.end;
        let range1_contains_other = range1.start <= range0.start && range1.end >= range0.end;

        if range0_contains_other || range1_contains_other {
            total_contains_other += 1;
        }
        
        let any_overlap = 
            (range0.end >= range1.start && range0.start <= range1.start) ||
            (range1.end >= range0.start && range1.start <= range0.start);

        if any_overlap {
            total_overlap += 1;
        }
    }

    println!("Total tasks containing others: {}", total_contains_other);
    println!("Total overlapping tasks: {}", total_overlap);
}

fn str_to_range(str: &str) -> Range<i32> {
    let split: Vec<&str> = str.split("-").collect();
    return split[0].parse::<i32>().expect("failed to parse") .. split[1].parse::<i32>().expect("failed to parse");
}