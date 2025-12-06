use std::ops;
use std::fmt::Debug;

#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub struct Point {
    pub x: i64,
    pub y: i64,
}

impl ops::Add for Point {
    type Output = Self;

    fn add(self, rhs: Point) -> Self {
        return Self {
            x: self.x + rhs.x,
            y: self.y + rhs.y
        }
    }
}

impl ops::AddAssign for Point {
    fn add_assign(&mut self, rhs: Self) {
        self.x += rhs.x;
        self.y += rhs.y;
    }
}

impl ops::Sub for Point {
    type Output = Self;

    fn sub(self, rhs: Self) -> Self {
        return Self {
            x: self.x - rhs.x,
            y: self.y - rhs.y
        }
    }
}

impl ops::SubAssign for Point {
    fn sub_assign(&mut self, rhs: Self) {
        *self = Self {
            x: self.x + rhs.x,
            y: self.y + rhs.y,
        };
    }
}


impl Point {
    pub fn new<T>(x: T, y: T) -> Self where T: TryInto<i64>, T::Error: Debug {
        Self {
            x: x.try_into().expect("Number too large for i64"),
            y: y.try_into().expect("Number too large for i64")
        }
    }
}