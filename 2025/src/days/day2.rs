

fn import(input: &str) -> Vec<(i64, i64)> {
    let mut vec = Vec::new();

    for pair in input.split(',') {
        let (start, end) =
            scan_fmt!(pair, "{}-{}", i64, i64).expect("Invalid format");

        vec.push((start as i64, end as i64));
    }

    return vec;
}

fn iterate_ranges(data: Vec<(i64, i64)>, comparison_function: fn(&str) -> bool) -> i64 {
    let mut total: i64 = 0;

    for pair in data {
        for id in pair.0 ..= pair.1 {
            let id_string = id.to_string();
            if comparison_function(id_string.as_str()) {
                total += id;
            }
        }
    }

    return total;
}

pub fn part1(input: &str) {
    let data: Vec<(i64, i64)> = import(input);
    
    let total = iterate_ranges(data,
         |str| str[.. str.len()/2] == str[str.len()/2 ..]
    );

    println!("Ids that repeat twice: {}", total);
}

pub fn part2(input: &str) {
    let data: Vec<(i64, i64)> = import(input);
    let total = iterate_ranges(data, |id_string| {
        let id_len = id_string.len();

        for count in 1..=id_len/2 {
            // not even, skip
            if id_len % count != 0 {
                continue;
            }
            let compare = id_string[ ..count].as_bytes();
            
            if id_string.as_bytes().chunks(count).all(|x| x == compare) {
                return true;
            }
        }

        return false;
    });

    println!("Ids that repeat multiple times: {}", total);
}