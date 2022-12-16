use crate::data_types::{grid::Grid, point::Point};

pub fn day14(input_lines: Vec<&str>) {
    let point_list: Vec<Vec<Point>> = input_lines.iter().map(|x| parse_point_list(x)).collect();
    
    let max_y = point_list.iter().flatten().map(|x| x.y).max().unwrap();

    let sand_till_void = run_sand_sim(
        &mut parse_grid(1000, max_y as usize + 2, &point_list),
        |point| point.y >= max_y,
        |point| point.y > max_y,
    );

    println!("Sand until falling into the void: {}", sand_till_void);

    let sand_to_cover = run_sand_sim(
        &mut parse_grid(1000, max_y as usize + 3, &point_list),
        |point| point == Point::new(500, 0),
        |point| point.y >= max_y + 2
    );

    println!("Sand until covering hole: {}", sand_to_cover + 1 /* add one since it returns before placing 500,0 */);
}

fn parse_grid(width: usize, height: usize, point_list: &Vec<Vec<Point>>) -> Grid<Tile> {
    let mut grid: Grid<Tile> = Grid::new(Tile::Air, width, height);

    for coord_pairs in point_list {

        let mut current = coord_pairs[0];
        for i in 1..coord_pairs.len() {
            let target = coord_pairs[i];

            let diff = target-current;
            let sign = Point::new(diff.x.signum(), diff.y.signum());

            for _ in 0..=i64::max(diff.x.abs(), diff.y.abs()) {
                grid[current] = Tile::Wall;
                current += sign;
            }

            current = target;
        }
    }

    return grid;
}

fn run_sand_sim<F0: Fn(Point)->bool, F1: Fn(Point) -> bool>(grid: &mut Grid<Tile>, exit_condition: F0, stop_function: F1) -> usize {
    let mut sand_count = 0;
    
    let targets = [Point::new(0, 1), Point::new(-1, 1), Point::new(1, 1)];
    'sanding: loop {
        let mut position = Point::new(500, 0);

        'falling: loop {
            
            for target in targets {
                let new_position = position + target;

                if grid[new_position] == Tile::Air && stop_function(new_position) == false {
                    position = new_position;
                    continue 'falling;
                }
            }

            // Break free
            if exit_condition(position) {
                break 'sanding;
            }

            // Cant fall anywhere
            grid[position] = Tile::Sand;
            sand_count += 1;
            break;
        }
    }

    return sand_count;
}

fn parse_point_list(line: &str) -> Vec<Point> {
    return line.split(" -> ").map(|x| parse_point(x)).collect();
}

fn parse_point(input: &str) -> Point {
    let pair: Vec<&str> = input.split(",").collect();
    return Point::new(pair[0].parse().unwrap(), pair[1].parse().unwrap());
}

#[derive(Copy, Clone, PartialEq)]
enum Tile {
    Air,
    Wall,
    Sand,
}