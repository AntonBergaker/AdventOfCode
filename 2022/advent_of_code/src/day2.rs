use std::fs;

pub fn day2() {
    let input = fs::read_to_string("input/day2.txt").expect("Failed to load input.");
    let input_lines: Vec<&str> = input.lines().collect();

    {
        let mut score = 0;
        for line in &input_lines {
            let choices: Vec<&str> = line.split(" ").collect();
            let opponent_hand =  get_hand_from_opponent(choices[0]);
            let my_hand =  match choices[1] {
                "X" => 'R',
                "Y" => 'P',
                "Z" => 'S',
                _ => panic!("Unknown character")
            };

            let result = match (my_hand, opponent_hand) {
                (a, b) if (a == b) => 'T',
                ('R', 'S') => 'W',
                ('R', 'P') => 'L',
                ('P', 'R') => 'W',
                ('P', 'S') => 'L',
                ('S', 'P') => 'W',
                ('S', 'R') => 'L',
                _ => panic!("Unknown combination")
            };

            let choice_score = get_score_from_hand(my_hand);
            let win_score = get_score_from_result(result);

            score += choice_score + win_score;
        }

        println!("Score according to XYZ being hands: {}", score);
    }

    {
        let mut score = 0;

        for line in &input_lines {
            let choices: Vec<&str> = line.split(" ").collect();
            let opponent_hand =  get_hand_from_opponent(choices[0]);
            let expected_result = match choices[1] {
                "X" => 'L',
                "Y" => 'T',
                "Z" => 'W',
                _ => panic!("Unknown character")
            };

            let my_hand = match (opponent_hand, expected_result) {
                (a, b) if (b == 'T') => a,
                ('R', 'W') => 'P',
                ('R', 'L') => 'S',
                ('P', 'W') => 'S',
                ('P', 'L') => 'R',
                ('S', 'W') => 'R',
                ('S', 'L') => 'P',
                _ => panic!("Unknown combination")
            };

            let choice_score = get_score_from_hand(my_hand);
            let win_score = get_score_from_result(expected_result);

            score += choice_score + win_score;
        }

        println!("Score according to XYZ being result: {}", score);
    }
}


fn get_hand_from_opponent(hand: &str) -> char {
    return match hand {
        "A" => 'R',
        "B" => 'P',
        "C" => 'S',
        _ => panic!("Unknown character")
    };
}


fn get_score_from_result(result: char) -> i32 {
    return match result {
        'W' => 6,
        'T' => 3,
        'L' => 0,
        _ => panic!("Unknown win condition")
    };
}

fn get_score_from_hand(hand: char) -> i32 {
    return match hand {
        'R' => 1,
        'P' => 2,
        'S' => 3,
        _ => panic!("Unknown hand")
    };
}