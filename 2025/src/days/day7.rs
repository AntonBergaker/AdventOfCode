use std::collections::{HashMap, HashSet, VecDeque};

use common::data_types::{grid::Grid, point::Point};

use crate::days::day::Day;

pub struct Day7;

#[derive(PartialEq)]
pub enum Cell {
    Blank,
    Splitter,
    Start
}

impl Day for Day7 {
    type Input = Grid<Cell>;
    fn import(input: &str) -> Grid<Cell> {
        return Grid::from(input, |x| match x {
            'S' => Cell::Start,
            '^' => Cell::Splitter,
            '.' => Cell::Blank,
            _ => panic!("Unknown type")
        });
    }
    
    fn part1(grid: &Self::Input) -> String {
        let start = grid.position_of(Cell::Start);
        let mut visited = HashSet::new();
        let mut queue = VecDeque::new();

        let mut times_split = 0;

        fn travel_to_point(point: Point, grid: &Grid<Cell>, queue: &mut VecDeque<Point>, visited: &mut HashSet<Point>) {
            if visited.contains(&point) || grid.point_is_inside(point) == false {
                return;
            }
            queue.push_back(point);
            visited.insert(point);
        }

        travel_to_point(start, &grid, &mut queue, &mut visited);

        loop {
            let Some(position) = queue.pop_front() else {
                break;
            };

            let cell = &grid[position];
            if *cell == Cell::Splitter {
                travel_to_point(position + Point::new(1, 1), &grid, &mut queue, &mut visited);
                travel_to_point(position + Point::new(-1, 1), &grid, &mut queue, &mut visited);
                times_split += 1;
            } else {
                travel_to_point(position + Point::new(0, 1), &grid, &mut queue, &mut visited);
            }
        }

        return highlight_part!("Times the beam was split: {}", times_split);
    }
    
    fn part2(grid: &Self::Input) -> String {
        let start = grid.position_of(Cell::Start);
        let mut visited = HashMap::new();
        let mut queue = VecDeque::new();
        travel_to_point(start, 1, &grid, &mut queue, &mut visited);

        fn travel_to_point(point: Point, count: i64, grid: &Grid<Cell>, queue: &mut VecDeque<Point>, visited: &mut HashMap<Point, i64>) {
            if grid.point_is_inside(point) == false {
                return;
            }
            if let Some(previous_count) = visited.get(&point) {
                visited.insert(point, previous_count + count);
                return;
            }

            queue.push_back(point);
            visited.insert(point, count);
        }

        loop {
            let Some(position) = queue.pop_front() else {
                break;
            };

            let cell = &grid[position];
            let count = *visited.get(&position).unwrap();
            if *cell == Cell::Splitter { 
                travel_to_point(position + Point::new(1, 1), count, &grid, &mut queue, &mut visited);
                travel_to_point(position + Point::new(-1, 1), count, &grid, &mut queue, &mut visited);
            } else {
                travel_to_point(position + Point::new(0, 1), count, &grid, &mut queue, &mut visited);
            }
        }

        let mut total_timelines: i64 = 0;
        for i in 0..grid.width() {
            if let Some(total) = visited.get(&Point::new(i, grid.height()-1)) {
                total_timelines += *total;
            }
        }

        return highlight_part!("Total timelines of travel: {}", total_timelines);
    }

}

