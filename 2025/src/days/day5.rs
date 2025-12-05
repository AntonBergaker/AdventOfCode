use std::ops::RangeInclusive;

use crate::days::day::Day;

pub struct Day5;

impl Day for Day5 {
    type Input = (Vec<RangeInclusive<i64>>, Vec<i64>);
    
    fn import(input: &str) -> Self::Input {
        let lines = input.lines().collect::<Vec<&str>>();
        let lines_cutoff = lines.iter().position(|x| *x == "").unwrap();

        return (
            lines[ .. (lines_cutoff)].iter().map(|x| {
                let (start, end) =
                    scan_fmt!(x, "{}-{}", i64, i64).expect("Invalid format");
                return start ..= end;
            }).collect::<Vec<RangeInclusive<i64>>>(),
            lines[lines_cutoff + 1 ..].iter().map(|x| (*x).parse::<i64>().unwrap()).collect::<Vec<i64>>()
        );
    }
    
    fn part1(input: &Self::Input) -> String {
        let (valid_id_ranges, ids) = input;

        let mut valid_count = 0;

        for id in ids {
            for range in valid_id_ranges {
                if range.contains(id) {
                    valid_count += 1;
                    break;
                }
            }
        }

        return highlight_part!("Amount of fresh ingredient IDs: {}", valid_count)
    }
    
    fn part2(input: &Self::Input) -> String {
        let (valid_id_ranges, _) = input;

        // I stole the range merging from myself from AoC 2022 Day 15. Lucky me it was also in rust.
        // I had an original lazy implementation that joined ranges very innefficiently, running in O(n^3).
        // But even though it finished under 1ms I decided I'd rather steal my good code^tm from previous years.
        let mut sorted_ranges = valid_id_ranges.clone();
        sorted_ranges.sort_by(|a, b| i64::cmp(a.start(), b.start()));

        let mut joined_ranges: Vec<RangeInclusive<i64>> = Vec::new();
        let mut current_range = sorted_ranges[0].clone();
        
        for range in sorted_ranges[1 ..].iter() {
            if *range.start() <= *current_range.end() + 1 {
                current_range = *current_range.start() ..= i64::max(*current_range.end(), *range.end());
                continue;
            }

            joined_ranges.push(current_range);
            current_range = range.clone();
        }
        joined_ranges.push(current_range);
        
        let total_sum: i64 = joined_ranges.iter().map(|x| *x.end() - *x.start() + 1).sum();

        return highlight_part!("Total valid IDs: {}", total_sum)
    }
}
