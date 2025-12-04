use common::data_types::{grid::Grid, point::Point};

use crate::days::day::Day;

pub struct Day4;

const DIAGONAL_NEIGHBORS: [(i64, i64); 8] = [
    (-1,  0),
    ( 1,  0),
    ( 0, -1),
    ( 0,  1),
    (-1, -1),
    ( 1, -1),
    (-1,  1),
    ( 1,  1),
];

fn get_positions_with_neighbor_count(grid: &Grid<bool>, count: i32) -> Vec<Point> {
    let mut removable = Vec::new();
    for p in grid.get_all_points() {
        if grid[p] == false {
            continue;
        }

        let mut neighbor_rolls = 0;
        for neighbor_offset in DIAGONAL_NEIGHBORS {
            let neighbor = p + Point::new(neighbor_offset.0, neighbor_offset.1);
            if grid.point_is_inside(neighbor) && grid[neighbor] {
                neighbor_rolls += 1;
            }
        }

        if neighbor_rolls < count {
            removable.push(p);
        }
    }

    return removable;
}

impl Day for Day4 {
    type Input = Grid<bool>;
    fn import(input: &str) -> Grid<bool> {
        return Grid::from(input, |x| match x {
            '@' => true,
            _ => false,
        });
    }

    fn part1(grid: &Self::Input) -> String {
        let less_than_4_neighbors = get_positions_with_neighbor_count(&grid, 4).len();

        return highlight_part!("Rolls of paper with less than 4 neighbors: {}", less_than_4_neighbors);
    }

    fn part2(input: &Self::Input) -> String {
        let mut removed_count = 0;
        let mut grid = input.clone();
        loop {
            let rolls_to_remove = get_positions_with_neighbor_count(&grid, 4);
            if rolls_to_remove.len() == 0 {
                break;
            }
            for roll_position in rolls_to_remove {
                grid[roll_position] = false;
                removed_count += 1;
            }
        }

        return highlight_part!("Total rolls removed: {}", removed_count);

    }
}