pub fn day1(input_lines: Vec<&str>) {
    let mut calories: Vec<i32> = Vec::new();

    {
        let mut current_calories = 0;
        for line in input_lines {
            if line.is_empty() {
                calories.push(current_calories);
                current_calories = 0;
                continue;
            }
            let calories = line.parse::<i32>().unwrap();
            current_calories += calories;
        }
        calories.push(current_calories);
    }

    calories.sort();

    let top_three = match calories[calories.len()-3 ..]  {
        [ a, b, c] => a + b + c,
        _ => 0,
    };

    println!("Top one: {}", calories.last().expect(""));
    println!("Top three: {}", top_three);
}