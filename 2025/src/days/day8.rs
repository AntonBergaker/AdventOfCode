use core::f32;
use std::{collections::{HashMap}, error::Error};

use crate::days::day::Day;

#[derive(Debug, Clone, Copy)]
pub struct Point3D {
    x: i32,
    y: i32,
    z: i32
}

impl Point3D {
    fn new(x: i32, y: i32, z: i32) -> Self {
        return Self {
            x, y, z
        };
    }
    fn from_str(str: &str) -> Result<Self, Box<dyn Error>> {
        let number_results: Result<Vec<i32>, _> = str.split(',').map(|i| str::parse(i)).collect();

        let numbers = number_results?;

        return Ok(Self::new(numbers[0], numbers[1], numbers[2]));
    }
}

#[derive(Debug, Clone, Copy)]
struct Connection {
    point0: usize,
    point1: usize,
    distance: f32
}

pub struct Day8;

impl Day for Day8 {
    type Input = Vec<Point3D>;
    fn import(input: &str) -> Vec<Point3D> {
        return input.lines().map(|x| Point3D::from_str(x).unwrap()).collect::<Vec<Point3D>>();
    }
    
    fn part1(junctions: &Self::Input) -> String {
        let distances = get_distances(&junctions);
        let mut circuits: Vec<usize> = (0 .. junctions.len()).collect();

        let connections_to_do = if junctions.len() < 30 {10} else {1000};

        for i in 0.. usize::min(distances.len(), connections_to_do) {
            let distance = distances[i];
            connect_circuit(distance, &mut circuits);
        }

        let counts = counts_sorted(&circuits);

        return highlight_part!("Product of count of 3 biggest circuits: {}", counts.iter().take(3).fold(1, |a, b| a * (b.1 as i64)));
    }
    
    fn part2(junctions: &Self::Input) -> String {
        let distances = get_distances(&junctions);
        let mut circuits: Vec<usize> = (0 .. junctions.len()).collect();

        let mut final_product = 0;

        for i in 0..distances.len() {
            let distance = distances[i];
            connect_circuit(distance, &mut circuits);

            if is_all_one_circuit(&circuits) {
                final_product = junctions[distance.point0].x as i64 * junctions[distance.point1].x as i64;
                break;
            }
        }

        return highlight_part!("Product of last two X coordinates: {}", final_product);
    }
    
}

fn connect_circuit(distance: Connection, circuits: &mut [usize]) {
    // Transfer circuit
    let old_circuit = circuits[distance.point0];
    let new_circuit = circuits[distance.point1];
    for i in 0 .. circuits.len() {
        if circuits[i] == old_circuit {
            circuits[i] = new_circuit;
        }
    }
}

fn counts_sorted(nums: &[usize]) -> Vec<(usize, usize)> {
    let mut map = HashMap::new();

    // Count occurrences
    for &n in nums {
        *map.entry(n).or_insert(0) += 1;
    }

    // Move into Vec and sort by count (descending)
    let mut pairs: Vec<(usize, usize)> =
        map.into_iter().collect();

    pairs.sort_by_key(|&(_, count)| std::cmp::Reverse(count));
    pairs
}

fn is_all_one_circuit(nums: &[usize]) -> bool {
    let first = nums[0];
    return nums.iter().all(|x| *x == first);
}

fn get_distances(junctions: &Vec<Point3D>) -> Vec<Connection> {
    let jun_count = junctions.len();
    let mut distances = Vec::new();

    for x in 0.. jun_count {
        for y in (x+1).. jun_count {
            let point0 = junctions[x];
            let point1 = junctions[y];
            distances.push(Connection {
                point0: x,
                point1: y,
                distance: 
                    sqr(point0.x as f32 - point1.x as f32) + 
                    sqr(point0.y as f32 - point1.y as f32) + 
                    sqr(point0.z as f32 - point1.z as f32)
            });
        }
    }

    distances.sort_by(|a, b| f32::total_cmp(&a.distance, &b.distance));

    return distances;
}

fn sqr(value: f32) -> f32 {
    return value*value;
}

