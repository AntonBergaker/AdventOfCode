use common::data_types::grid::Grid;

pub fn day10(input_lines: Vec<&str>) {
    part1(&input_lines);
    part2(&input_lines);
}


fn part1(input_lines: &Vec<&str>) {
    let mut cycle = 0;
    let mut sum = 0;
    let mut register = 1;

    let mut increase_cycle = |register: i32| {
        cycle += 1;
        if (cycle+20) % 40 == 0 {
            sum += cycle * register;
        }
    };

    for line in input_lines {
        let words: Vec<&str> = line.split(" ").collect();
        match words[..] {
            ["noop"] => increase_cycle(register),
            ["addx", add] => {
                increase_cycle(register);
                increase_cycle(register);
                register += add.parse::<i32>().unwrap();
            },

            _ => panic!("Unknown input"),
        }
    }

    println!("Sum of register every 20th cycle: {}", sum);

}

fn part2(input_lines: &Vec<&str>) {
    let mut grid: Grid<bool> = Grid::new(false, 40, 6);

    let mut cycle = 0;
    let mut register = 1;

    let mut increase_cycle = |register: i32| {
        let x = cycle % 40;
        let y = cycle / 40;

        grid[(x, y)] = i32::abs(x as i32 - register) <= 1;

        cycle += 1;
    };

    for line in input_lines {
        let words: Vec<&str> = line.split(" ").collect();
        match words[..] {
            ["noop"] => increase_cycle(register),
            ["addx", add] => {
                increase_cycle(register);
                increase_cycle(register);
                register += add.parse::<i32>().unwrap();
            },

            _ => panic!("Unknown input"),
        }
    }

    println!("Output drawn to the CRT: ");
    println!("{}", grid.to_string(|x| if x {'#'} else {' '}));
}