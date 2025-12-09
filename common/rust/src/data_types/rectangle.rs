use super::point::Point;

#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub struct Rectangle {
    pub point0: Point,
    pub point1: Point,
}

impl Rectangle {
    pub fn new(p0: Point, p1: Point) -> Self {
        let point0 = Point::new(i64::min(p0.x, p1.x), i64::min(p0.y, p1.y));
        let point1 = Point::new(i64::max(p0.x, p1.x), i64::max(p0.y, p1.y));
        Self {
            point0, point1
        }
    }
    pub fn new_from_size(start: Point, size: Point) -> Self {
        Self::new(start, start+size)
    }
    
    pub fn top_left(self) -> Point {
        self.point0
    }
    pub fn top_right(self) -> Point {
        Point::new(self.point1.x, self.point0.y)
    }
    pub fn bottom_left(self) -> Point {
        Point::new(self.point0.x, self.point1.y)
    }
    pub fn bottom_right(self) -> Point {
        self.point1
    }
    pub fn size(self) -> Point {
        self.point1 - self.point0
    }
}