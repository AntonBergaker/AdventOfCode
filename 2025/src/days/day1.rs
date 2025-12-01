use sscanf::sscanf;

pub fn part1(input_lines: Vec<&str>) {
    let times_at_0 = evaluate(input_lines,&|clock, steps| {
        *clock += steps;
        *clock %= 100;

        if *clock == 0 {
            return 1;
        }
        return 0;
    });

    println!("Times clock stopped at 0: {}", times_at_0);
}

pub fn part2(input_lines: Vec<&str>) {
    let times_at_0 = evaluate(input_lines,&|clock, steps| {
        let sign = i32::signum(steps);
        let count = i32::abs(steps);

        let mut times_passed_0 = 0;

        for _ in 0..count {
            *clock += sign;
            *clock %= 100;

            if *clock == 0 {
                times_passed_0 += 1;
            }
        }

        return times_passed_0;
    });

    println!("Times clock went past 0: {}", times_at_0);
}

fn evaluate(input_lines: Vec<&str>, count_fun: &dyn Fn(&mut i32, i32) -> i32) -> i32 {
    let mut clock = 50;
    let mut times_at_0 = 0;

    for line in input_lines {
        let (direction, steps) =
            sscanf!(line, "{char}{i32}").expect("a");
        let step_mod = if direction == 'L' {-1} else {1};
        
        times_at_0 += count_fun(&mut clock, steps * step_mod)
    }

    return times_at_0;
}