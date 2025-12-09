use common::data_types::{point::Point, rectangle::Rectangle};

use crate::days::day::Day;

pub struct Day9;

impl Day for Day9 {
    type Input = Vec<Point>;
    fn import(input: &str) -> Vec<Point> {
        return input.lines()
            .map(|x| scan_fmt!(x, "{},{}", i64, i64).unwrap())
            .map(|p| Point::new(p.0, p.1)).collect::<Vec<Point>>();
    }
    
    fn part1(input: &Self::Input) -> String {
        let mut biggest = 0;
        for i in 0..input.len() {
            for j in (i+1)..input.len() {
                let p0 = input[i];
                let p1 = input[j];
                let size = i64::abs((p0.x - p1.x + 1) * (p0.y - p1.y + 1));
                if size > biggest {
                    biggest = size;
                }
            }
        }

        return highlight_part!("Biggest formed rectangle: {}", biggest);
    }
    
    fn part2(input: &Self::Input) -> String {


        let mut biggest = 0;
        for i in 0..input.len() {
            for j in (i+1)..input.len() {
                let rect = Rectangle::new(input[i], input[j]);
                let rect_size = rect.size();
                let inclusive_area = (rect_size.x + 1) * (rect_size.y + 1);
                if inclusive_area <= biggest {
                    continue;
                }

                if rectangle_intersects_polygon(&rect, input) {
                    continue;
                }
                
                biggest = inclusive_area;
            }
        }

        return highlight_part!("Biggest formed rectangle: {}", biggest);
    }
}

fn rectangle_intersects_polygon(rectangle: &Rectangle, polygon: &Vec<Point>) -> bool {
    let mut pre_i = polygon.len()-1;
    for i in 0..polygon.len() {
        if rectangle_intersects_line(rectangle, &(polygon[pre_i], polygon[i])) {
            return true;
        }
        pre_i = i;
    }

    return false;
}

fn rectangle_intersects_line(rect: &Rectangle, line: &(Point, Point)) -> bool {
    let (rect_p0, rect_p1) = (rect.point0, rect.point1);
    let (line_p0, line_p1) = line;

    if line_p0.x == line_p1.x {
        let line_min_y = i64::min(line_p0.y, line_p1.y);
        let line_max_y = i64::max(line_p0.y, line_p1.y);
        
        return rect.point0.x < line_p0.x && rect.point1.x > line_p0.x && 
            rect_p0.y < line_max_y && rect_p1.y > line_min_y;
    } else if line_p0.y == line_p1.y {
        let line_min_x = i64::min(line_p0.x, line_p1.x);
        let line_max_x = i64::max(line_p0.x, line_p1.x);
        
        return rect_p0.y < line_p0.y && rect_p1.y > line_p0.y && 
            rect_p0.x < line_max_x && rect_p1.x > line_min_x;
    } {
        panic!("Invalid line in input.")
    }
}
