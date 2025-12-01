use std::collections::HashSet;

use common::data_types::point::Point;

pub fn day9(input_lines: Vec<&str>) {
    println!("Visited points with 2 points: {}", simulate_rope_tail_visits(&input_lines, 2));
    println!("Visited points with 10 points: {}", simulate_rope_tail_visits(&input_lines, 10));
}

fn simulate_rope_tail_visits(input_lines: &Vec<&str>, rope_length: usize) -> usize {
    let mut rope = vec![Point::new(0, 0); rope_length];
    let mut visited = HashSet::<Point>::new();

    for line in input_lines {
        let words: Vec<&str> = line.split(" ").collect();
        let direction = dir_to_move_point(words[0]);
        let steps: i32 = words[1].parse().unwrap();

        for _ in 0..steps {
            rope[0] += direction;

            for i in 1..rope_length {
                let head = rope[i-1];
                let tail = rope[i];
                // If head and tail are seperated more than 2
                if i64::abs(tail.x - head.x) > 1 || i64::abs(tail.y - head.y) > 1 {
                    // Move 1 point in one or both direction
                    let diff = Point::new(
                        i64::clamp(head.x - tail.x, -1, 1),
                        i64::clamp(head.y - tail.y, -1, 1),
                    );
                    rope[i] += diff;
                }
            }

            visited.insert(rope[rope_length-1]);
        }
    }

    return visited.len()
}


fn dir_to_move_point(input: &str) -> Point {
    return match input {
        "L" => Point::new(-1, 0),
        "U" => Point::new(0, -1),
        "R" => Point::new(1, 0),
        "D" => Point::new(0, 1),
        _ => panic!("Invalid direction"),
    };
}