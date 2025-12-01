use std::{collections::VecDeque};

use common::data_types::{grid::Grid, point::Point};

pub fn day12(input: &str) {
    let grid = Grid::from(input, |x| x);

    part1(&grid);
    part2(&grid);
}

fn part1(grid: &Grid<char>) {
    let start: Point = grid.position_of('S');

    let (visited, end) = dijkstra(&grid, start, |c| c == 'E', |a, b| a >= b - 1);

    println!("Distance to the tip {}", visited[end]);

    // Prints visited
    /*println!("{}", visited.to_string(|x| format!("{: >3} ", match x {
        i32::MAX => -1,
        a => a,
    })));*/
}

fn part2(grid: &Grid<char>) {
    let start: Point = grid.position_of('E');

    let (visited, end) = dijkstra(&grid, start, |c| c == 'a', |a, b| b >= a - 1);

    println!("Distance to the closest a {}", visited[end]);
}

fn dijkstra(grid: &Grid<char>, start: Point, victory_condition: fn(char) -> bool, valid_movement: fn(i32, i32) -> bool) -> (Grid<i32>, Point) {
    let mut visited = Grid::new(i32::MAX, grid.width(), grid.height());

    let mut queue: VecDeque<Point> = VecDeque::new();
    visited[start] = 0;
    queue.push_back(start);
    let mut final_position = Point::new(-1, -1);

    while queue.is_empty() == false {
        let pos = queue.pop_front().unwrap();

        let grid_char = grid[pos];
        if victory_condition(grid_char) {
            final_position = pos;
            break;
        }

        let current_height = get_cell_height(grid[pos]);
        let order = visited[pos];

        let mut try_visit = |pos: Point| {
            if grid.point_is_inside(pos) == false {
                return;
            }

            if visited[pos] <= order+1 {
                return;
            }

            let cell_height = get_cell_height(grid[pos]);
            if valid_movement(current_height, cell_height) == false {
                return;
            }

            visited[pos] = order + 1;
            queue.push_back(pos);
        }; 

        try_visit(pos + Point::new(-1, 0));
        try_visit(pos + Point::new(1, 0));
        try_visit(pos + Point::new(0, 1));
        try_visit(pos + Point::new(0, -1));
    }

    if final_position.x < 0 {
        panic!("Failed to find the target!");
    }
    return (visited, final_position);
}

fn get_cell_height(cell: char) -> i32 {
    return match cell {
        'S' => 'a',
        'E' => 'z',
        a => a,
    } as i32 - 'a' as i32
}