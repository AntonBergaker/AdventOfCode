use std::collections::HashSet;

use common::data_types::{grid::Grid, point::Point};

use crate::days::day::Day;

pub struct Day12;

impl Day for Day12 {
    type Input = (Vec<Grid<bool>>, Vec<(Point, Vec<i32>)>);

    fn import(input: &str) -> Self::Input {
        let replaced = input.replace("\r", "");
        let segments: Vec<&str> = replaced.split("\n\n").collect();
        let mut patterns = Vec::new();

        for segment in segments[.. segments.len()-1].iter() {
            let first_line = segment.find("\n").unwrap();
            patterns.push(Grid::from(&segment[first_line+1 ..], |x| x == '#'));
        }

        let mut grids = Vec::new();
        for line in segments[segments.len()-1].lines() {
            let colon = line.find(":").unwrap();
            let (x, y) = scan_fmt!(&line[.. colon], "{}x{}", usize, usize).unwrap();
            let presents = line[(colon+2)..].split(" ").map(|x| str::parse(x).unwrap()).collect();

            grids.push((Point::new(x, y), presents));
        }

        return (patterns, grids);
    }
    
    fn part1(input: &Self::Input) -> String {
        let (patterns, grids) = input;
        let pattern_slots: Vec<i32> = 
            patterns.iter().map(|x| x.get_all_points().fold(0, |a, b| a + x[b] as i32)).collect();
        let mut total_success = 0;
        
        let rotations_and_flips = patterns.iter().map(|x| get_unique_rotations_and_flips(x)).collect();

        for grid in grids {
            let (size, presents) = grid;

            let area = (size.x * size.y) as i32;
            let trivial_area = (((size.x / 3)*3) * ((size.y / 3)*3)) as i32;
            
            let mut required_presents = 0;
            let mut required_presents_slots = 0;
            for (i, present_count) in presents.iter().enumerate() {
                required_presents += present_count;
                required_presents_slots += pattern_slots[i] * present_count;
            }

            // Check if everything trivially fits
            if trivial_area >= (required_presents * 9) {
                total_success += 1;
                continue;
            }

            // Check if it cant fit
            if required_presents_slots > area {
                continue;
            }

            // Hardmode:
            let mut pieces = Vec::new();
            for (i, present_count) in presents.iter().enumerate() {
                for _ in 0..*present_count {
                    pieces.push(i);
                }
            }
            let mut grid = Grid::<bool>::new(false, size.x as usize, size.y as usize);

            if try_fit(&mut grid, &pieces, &rotations_and_flips) {
                total_success += 1;
            }
        }

        return highlight_part!("{}", total_success);
    }
    
    fn part2(_: &Self::Input) -> String {
        return highlight_part!("{}", "Merry Christmas!");
    }
}

fn try_fit(exisiting_grid: &mut Grid<bool>, remaining_presents: &[usize], present_rotations: &Vec<Vec<Grid<bool>>>) -> bool {
    if remaining_presents.len() == 0 {
        return true
    }

    for present in &present_rotations[remaining_presents[0]] {
        let points: Vec<Point> = exisiting_grid.get_all_points().collect();
        for point in points {
            if can_fit_pattern(&exisiting_grid, point, present) {
                set_grid_to_pattern(exisiting_grid, point, present, true);
                if try_fit(exisiting_grid, &remaining_presents[1..], present_rotations) {
                    return true;
                }
                set_grid_to_pattern(exisiting_grid, point, present, false);
            }
        }

    }

    return false;
}

// Returns true if the grid can fit a present.
fn can_fit_pattern(exisiting_grid: &Grid<bool>, position: Point, present: &Grid<bool>) -> bool {
    for x in 0..present.width() {
        for y in 0..present.height() {
            if present[(x, y)] == false {
                continue;
            }

            let outer_pos = position + Point::new(x, y);
            if exisiting_grid.point_is_inside(outer_pos) == false {
                return false;
            }
            if exisiting_grid[outer_pos] {
                return false;
            }
        }
    }

    return true;
}

fn set_grid_to_pattern(exisiting_grid: &mut Grid<bool>, position: Point, pattern: &Grid<bool>, value: bool) {
    for x in 0..pattern.width() {
        for y in 0..pattern.height() {
            if pattern[(x, y)] == false {
                continue;
            }
            exisiting_grid[position + Point::new(x, y)] = value;
        }
    }
}

fn get_unique_rotations_and_flips(grid: &Grid<bool>) -> Vec<Grid<bool>> {
    let mut vec = HashSet::new();
    let mut mutable_grid = grid.clone();

    for _ in 0..4 {
        vec.insert(mutable_grid.clone());
        mutable_grid = mutable_grid.rotate();
    }
    mutable_grid = mutable_grid.flip();
    for _ in 0..4 {
        vec.insert(mutable_grid.clone());
        mutable_grid = mutable_grid.rotate();
    }

    return vec.into_iter().collect();
}