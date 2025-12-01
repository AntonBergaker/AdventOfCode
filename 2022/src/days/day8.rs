use common::data_types::point::Point;

pub fn day8(input_lines: Vec<&str>) {
    let mut trees: Vec<Vec<Tree>> = Vec::new();
    for line in input_lines {
        trees.push(line.chars().map(|f| 
            Tree {
                height: f.to_string().parse().unwrap(),
                visible: false
            }
        ).collect());
    }

    let height = trees.len();
    let width = trees.first().unwrap().len();

    // Reveal left to right and right to left
    for y in 0..height {
        // Left to right
        flag_visible(&mut trees, width, height, Point::new(0, y as i64), Point::new(1, 0));

        // Right to left
        flag_visible(&mut trees, width, height, Point::new(width as i64- 1, y as i64), Point::new(-1, 0));
    }

    // Reveal up to down and down to up
    for x in 0..width {
        // Up to down
        flag_visible(&mut trees, width, height, Point::new(x as i64, 0), Point::new(0, 1));

        // Down to up
        flag_visible(&mut trees, width, height, Point::new(x as i64, height as i64 -1), Point::new(0, -1));
    }

    let mut visible_count = 0;

    for y in 0..height {
        for x in 0..width {
            if trees[y][x].visible {
                visible_count += 1;
            }
        }
    }

    println!("Visible trees: {}", visible_count);

    let mut best_scenic_score = -1;

    for y in 0..height {
        for x in 0..width {
            let point = Point::new(x as i64, y as i64);
            let score = 
                // Left to right
                get_scenic_score_in_direction(&trees, width, height, point, Point::new(1, 0)) *
                // Right to left
                get_scenic_score_in_direction(&trees, width, height, point, Point::new(-1, 0)) *
                // Up to down
                get_scenic_score_in_direction(&trees, width, height, point, Point::new(0, 1)) *
                // Down to up
                get_scenic_score_in_direction(&trees, width, height, point, Point::new(0, -1));


            if score > best_scenic_score {
                best_scenic_score = score;
            }
        }
    }

    println!("Best scenic score: {}", best_scenic_score);
}

fn flag_visible(trees: &mut Vec<Vec<Tree>>, width: usize, height: usize, start: Point, step: Point) {
    let mut max_height = -1;
    
    let mut point = start;
    while 
        point.x >= 0 && point.y >= 0 &&
        point.x < width as i64 && point.y < height as i64 {

        let tree = &mut trees[point.x as usize][point.y as usize];
        if tree.height > max_height {
            max_height = tree.height;
            tree.visible = true;
        }

        point += step;
    }
}

fn get_scenic_score_in_direction(trees: &Vec<Vec<Tree>>, width: usize, height: usize, start: Point, step: Point) -> i32 {    
    let mut point = start;
    let mut score = 0;
    let max_height = trees[point.x as usize][point.y as usize].height;

    point += step;

    while 
        point.x >= 0 && point.y >= 0 &&
        point.x < width as i64 && point.y < height as i64 {
            
        score += 1;

        let tree = &trees[point.x as usize][point.y as usize];
        if tree.height >= max_height {
            break;
        }

        point += step;
    }

    return score;
}

struct Tree {
    height: i32,
    visible: bool,
}