use std::collections::{HashSet};

pub fn day6(input: &str) {
    println!("Start of packet index: {}", find_distinct_index(input, 4).unwrap());
    println!("Start of message index: {}", find_distinct_index(input, 14).unwrap());
}

pub fn find_distinct_index(input: &str, count: usize) -> Option<usize> {
    let chars:Vec<char> = input.chars().collect();
    for i in count..chars.len() {
        let last_chars: HashSet<&char> = chars[i-count..i].iter().collect();
        if last_chars.len() >= count {
            return Some(i);
        }
    }

    return None;
}