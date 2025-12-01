pub fn day11(input: &str) {
    part1(input);
    part2(input);
}

pub fn part1(input: &str) {
    let mut monkeys = get_monkeys(input);

    for _ in 0..20 {
        simulate_monkey_round(&mut monkeys, |x| x / 3);
    }

    let mut inspection_totals: Vec<i32> = monkeys.iter().map(|x| x.times_inspected).collect();
    inspection_totals.sort_by(|a, b| b.partial_cmp(a).unwrap());
    
    println!("Multiplied inspection totals: {}", inspection_totals[0] as i64 * inspection_totals[1] as i64);
}

fn part2(input: &str) {
    let mut monkeys = get_monkeys(input);

    let greatest_common_denominator = monkeys.iter().fold(1, |i, x| i * x.test_division_number);

    for _ in 0..10000 {
        simulate_monkey_round(&mut monkeys, |x| x % greatest_common_denominator as i64);
    }

    let mut inspection_totals: Vec<i32> = monkeys.iter().map(|x| x.times_inspected).collect();
    inspection_totals.sort_by(|a, b| b.partial_cmp(a).unwrap());
    
    println!("Multiplied inspection totals: {}", inspection_totals[0] as i64 * inspection_totals[1] as i64);
}

fn simulate_monkey_round<F: Fn(i64) -> i64>(monkeys: &mut Vec<Monkey>, relief_function: F) {
    for i in 0..monkeys.len() {
        let monkey = &mut monkeys[i];
        // Temp variable of transfered items to make rust happy
        let mut transfered_items: Vec<(usize, i64)> = Vec::new();

        for item in &monkey.items {
            // Inspect
            let words: Vec<&str> = monkey.operation.split(" ").collect();
            let lhs = get_operation_side(words[0], *item);
            let rhs = get_operation_side(words[2], *item);
            let mut new_value = run_operation(words[1], lhs, rhs);

            // Relief
            new_value = relief_function(new_value);

            // Test
            let target_monkey = if new_value % monkey.test_division_number as i64 == 0 
                { monkey.true_monkey_id } else { monkey.false_monkey_id };
            transfered_items.push((target_monkey as usize, new_value));
        }

        monkey.times_inspected += monkey.items.len() as i32;
        monkey.items.clear();

        for item in transfered_items {
            monkeys[item.0].items.push(item.1);
        }
    }
}

fn get_operation_side(text: &str, old: i64) -> i64 {
    return match text {
        "old" => old,
        a => a.parse().unwrap(),
    }
}

fn run_operation(op: &str, lhs: i64, rhs: i64) -> i64 {
    return match op {
        "+" => lhs + rhs,
        "-" => lhs - rhs,
        "*" => lhs * rhs,
        "/" => lhs / rhs,
        _ => panic!("Unknown operation"),
    };
}

struct Monkey {
    items: Vec<i64>,
    times_inspected: i32,
    operation: String,
    test_division_number: i32,
    true_monkey_id: i32,
    false_monkey_id: i32,
}

fn get_monkeys(input: &str) -> Vec<Monkey> {
    let monkey_lines = input.split("\r\n\r\n");

    let mut monkeys: Vec<Monkey> = Vec::new();

    for money_input in monkey_lines {
        let lines: Vec<&str> = money_input.lines().map(|x| x.trim()).collect();

        let id = lines[0].trim_start_matches("Monkey ").trim_end_matches(":").parse().unwrap();
        assert!(id == monkeys.len());

        let items: Vec<i64> = lines[1].trim_start_matches("Starting items: ")
            .split(", ").map(|x| x.parse().unwrap()).collect();

        let operation = lines[2].trim_start_matches("Operation: new = ");

        let test_devision_number: i32 = lines[3].trim_start_matches("Test: divisible by ").parse().unwrap();

        let true_monkey_id: i32 = lines[4].trim_start_matches("If true: throw to monkey ").parse().unwrap();
        let false_monkey_id: i32 = lines[5].trim_start_matches("If false: throw to monkey ").parse().unwrap();

        monkeys.insert(id, Monkey { 
            items: items,
            times_inspected: 0,
            operation: String::from(operation), 
            test_division_number: test_devision_number,
            true_monkey_id: true_monkey_id,
            false_monkey_id: false_monkey_id 
        });
    }

    return monkeys;
}