use std::ops::{Index, IndexMut};

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
    pub fn width(&self) -> usize {
        return self.width;
    }

    pub fn height(&self) -> usize {
        return self.height;
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