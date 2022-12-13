use std::{ops::{Index, IndexMut}, fmt::Display};

use super::point::Point;

#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub struct Grid<T> {
    pub data: Vec<T>,
    width: usize,
    height: usize,
}


impl<T: std::clone::Clone> Grid<T> {
    pub fn new(default_value: T, width: usize, height: usize) -> Grid<T> {
        return Self {
            data: vec![default_value; width*height],
            width: width,
            height: height,
        };
    }
}

impl<T> Grid<T> {
    pub fn from<F: Fn(char) -> T>(str: &str, convert_func: F) -> Grid<T> {
        let lines: Vec<&str> = str.lines().filter(|x| x.is_empty() == false).collect();
        let width = lines[0].len();
        let height = lines.len();

        let mut data: Vec<T> = Vec::with_capacity(width * height);

        for y in 0..lines.len() {
            let line = lines[y];
            if line.len() != width {
                panic!("Line {} was not the expected width of {}", y, width);
            }
            let chars: Vec<char> = line.chars().collect();
            for x in 0..chars.len() {
                data.push(convert_func(chars[x]));
            }
        }

        return Self {
            data: data,
            width: width,
            height: height,
        };
    }
}

impl<T:Copy> Grid<T> {
    pub fn to_string<D: Display, F: Fn(T) -> D>(&self, convert_func: F) -> String {
        let mut str = String::new();
        for y in 0..self.height() {
            for x in 0..self.width() {
                str.push_str(&convert_func(self[(x, y)]).to_string());
            }
    
            str.push('\n');
        }
        return str;
    }
}


impl<T> Grid<T> {
    pub fn width(&self) -> usize {
        return self.width;
    }

    pub fn height(&self) -> usize {
        return self.height;
    }

    pub fn point_is_inside(&self, point: Point) -> bool {
        return point.x >= 0 && point.y >= 0 && point.x < self.width as i32 && point.y < self.height as i32;
    }
}

impl<T: PartialEq> Grid<T> {
    pub fn position_of(&self, value: T) -> Point {
        let index: usize = self.data.iter().position(|x| *x == value).unwrap();
        return Point::new((index % self.width) as i32, (index / self.width) as i32);
    }
}



impl<T> Index<(usize, usize)> for Grid<T> {
    type Output = T;
    fn index<'a>(&'a self, i: (usize, usize)) -> &'a T {
        return &self.data[i.0 + i.1 * self.width];
    }
}

impl<T> IndexMut<(usize, usize)> for Grid<T> {
    fn index_mut<'a>(&'a mut self, i: (usize, usize)) -> &'a mut T {
         return &mut self.data[i.0 + i.1 * self.width];
    }
}

impl<T> Index<Point> for Grid<T> {
    type Output = T;
    fn index<'a>(&'a self, i: Point) -> &'a T {
        return &self.data[i.x as usize + i.y as usize * self.width];
    }
}

impl<T> IndexMut<Point> for Grid<T> {
    fn index_mut<'a>(&'a mut self, i: Point) -> &'a mut T {
         return &mut self.data[i.x as usize + i.y as usize * self.width];
    }
}