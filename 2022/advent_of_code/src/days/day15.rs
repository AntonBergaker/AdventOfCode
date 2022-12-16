use std::{ops::{Range}, collections::HashSet, hash::Hash};

use regex::Regex;

use crate::data_types::point::Point;

pub fn day15(input_lines: Vec<&str>) {
    // I promised I never would, yet here I go regexing.
    let number_regex = Regex::new(r"(-?\d+)").unwrap();

    let mut sensors: Vec<Sensor> = Vec::new();

    for line in input_lines {
        let numbers: Vec<i64> = number_regex.find_iter(line).map(|f| f.as_str().parse().unwrap()).collect();
        let position = Point::new(numbers[0], numbers[1]);
        let beacon = Point::new(numbers[2], numbers[3]);
        let diff = position - beacon;
        let radius = diff.x.abs() + diff.y.abs();

        sensors.push(Sensor {
            position,
            radius,
            nearest_beacon_position: beacon
        });
    }

    {
        let (beacon_count, ranges) = project_over_line(&sensors, 2000000);

        println!("Occupied spaces on line 2000000: {}", ranges.iter().map(|x| (x.end - x.start).abs() + 1).sum::<i64>() - beacon_count);
    }

    {
        const MAX_COORD: i64 = 4000000;
        fn test(point: Point) -> bool {
            return point.x >= 0 && point.y >= 0 && point.x <= MAX_COORD && point.y <= MAX_COORD;
        }

        let mut found = Point::new(-1 , -1);
        // Sorry father for I have sinned
        'loopin: for y in 0..MAX_COORD {
            let (_, ranges) = project_over_line(&sensors, y);

            let last_range = ranges[0].copy();
            for i in 1..ranges.len() {
                let range = ranges[i].copy();
                if range.start > last_range.end + 1 {
                    if test(Point::new(range.start+1, y)) {
                        found = Point::new(range.start-1, y);
                        break 'loopin;
                    }
                }
            }
        }

        println!("Tuning frequency: {}", found.x*MAX_COORD+found.y);
    }
}

fn project_over_line(sensors: &Vec<Sensor>, y: i64) -> (i64, Vec<Range<i64>>) {
    let mut ranges: Vec<Range<i64>> = Vec::new();
    let mut beacons_on_line: HashSet<i64> = HashSet::new();
    
    for sensor in sensors {
        let y_diff = sensor.radius - (y - sensor.position.y).abs();

        if y_diff < 0 {
            continue;
        }

        if sensor.nearest_beacon_position.y == y {
            beacons_on_line.insert(sensor.nearest_beacon_position.x);
        }

        ranges.push( sensor.position.x - y_diff .. sensor.position.x + y_diff);
    }
    
    ranges.sort_by(|a, b| a.start.cmp(&b.start));

    let mut joined_ranges: Vec<Range<i64>> = Vec::new();
    let mut current_range = ranges[0].copy();
    
    for i in 1..ranges.len() {
        let range = ranges[i].copy();

        if range.start <= current_range.end {
            current_range = current_range.start .. current_range.end.max(range.end);
            continue;
        }

        joined_ranges.push(current_range);
        current_range = range;
    }
    joined_ranges.push(current_range);

    return (beacons_on_line.len() as i64, joined_ranges);
}

pub trait RangeExt<T> {
    fn copy(&self) -> Range<T>;
}
impl<T:Copy> RangeExt<T> for Range<T> {
    fn copy(&self) -> Range<T> {
        return self.start .. self.end
    }
}
struct Sensor {
    pub position: Point,
    pub radius: i64,
    pub nearest_beacon_position: Point,
}