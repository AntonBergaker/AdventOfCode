use std::{collections::{HashMap, HashSet}, fmt::Display, str::FromStr};

use priority_queue::PriorityQueue;

use crate::days::day::Day;

pub struct Day11;

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
pub struct Identifier {
    value: u32
}

impl FromStr for Identifier {
    type Err = ();

    fn from_str(input: &str) -> Result<Self, Self::Err> {
        if input.len() > 4 {
            return Err(());
        }
        let mut value: u32 = 0;

        for c in input.bytes() {
            value <<= 8;
            value |= c as u32;
        }

        return Ok(Self {
            value
        });
    }
}

impl Display for Identifier {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        let mut value = self.value;
        let mut letters = Vec::new();
        while value > 0 {
            letters.push((value & 0xFF) as u8);
            value >>= 8;
        }

        write!(f, "{}", str::from_utf8(&letters).unwrap())
    }
}

struct Node {
    from: Vec<Identifier>,
    to: Vec<Identifier>
}
impl Node {
    fn new() -> Self {
        Self { 
            from: Vec::new(),
            to: Vec::new()
        }
    }
}

type Graph = HashMap<Identifier, Node>;


impl Day for Day11 {
    type Input = Vec<(Identifier, Vec<Identifier>)>;
    fn import(input: &str) -> Self::Input {
        input.lines().map(|line| {
            let identifier = Identifier::from_str(&line[..3]).unwrap();

            let edges: Vec<Identifier> = line[5..].split(' ').filter_map(|x| Identifier::from_str(x).ok()).collect();
            (identifier, edges)
        }).collect()
    }
    
    fn part1(input: &Self::Input) -> String {
        let graph = build_graph(input);
        
        let out_id = Identifier::from_str("out").unwrap();
        let you_id = Identifier::from_str("you").unwrap(); 

        let paths_to_out = paths_between_points(you_id, out_id, &graph);
        return highlight_part!("Total ways to reach out: {}", paths_to_out);
    }
    
    fn part2(input: &Self::Input) -> String {
        let graph = build_graph(input);

        let out_id = Identifier::from_str("out").unwrap();
        let svr_id = Identifier::from_str("svr").unwrap(); 
        let dac_id = Identifier::from_str("dac").unwrap();
        let fft_id = Identifier::from_str("fft").unwrap();

        let svr_dac = paths_between_points(svr_id, dac_id, &graph);
        let svr_fft = paths_between_points(svr_id, fft_id, &graph);
        
        let dac_fft = paths_between_points(dac_id, fft_id, &graph);
        let fft_dac = paths_between_points(fft_id, dac_id, &graph);

        let dac_out = paths_between_points(dac_id, out_id, &graph);
        let fft_out = paths_between_points(fft_id, out_id, &graph);

        return highlight_part!("Total ways to reach out that pass fft and dac: {}", svr_dac * dac_fft * fft_out + svr_fft * fft_dac * dac_out);        
    }
}

fn build_graph(node_connections: &Vec<(Identifier, Vec<Identifier>)>) -> Graph {
    let mut nodes = HashMap::new();
    let out_id = Identifier::from_str("out").unwrap();
    nodes.insert(out_id, Node::new());

    for node_connection in node_connections {
        let (id, _) = node_connection;
        nodes.insert(*id, Node::new());
    }

    for node_connection in node_connections {
        let (id, edges) = node_connection;
        // Push tos
        {
            let source = nodes.get_mut(&id).unwrap();
            
            for edge in edges {
                source.to.push(*edge);
            }
        }
        // Push froms
        {            
            for edge in edges {
                let target =nodes.get_mut(&edge).unwrap();
                target.from.push(*id);
            }
        }
    }

    return nodes;
}

fn build_distance_from_end(end: Identifier, graph: &Graph) -> HashMap<Identifier, i32> {
    let mut distance_from_end  = HashMap::<Identifier, i32>::new();
    let mut queue = HashSet::<Identifier>::new();
    queue.insert(end);
    distance_from_end.insert(end, 0);

    while let Some(&id) = queue.iter().next() {
        queue.remove(&id);

        let node = graph.get(&id).unwrap();
        let new_distance = *distance_from_end.get(&id).unwrap() + 1;

        for edge in &node.from {
            let worsened = match distance_from_end.get(edge) {
                Some(previous_distance) => *previous_distance < new_distance,
                None => true
            };

            if worsened {
                distance_from_end.insert(*edge, new_distance);
                queue.insert(*edge);
            }
        }
    }

    return distance_from_end;
}

fn paths_between_points(start: Identifier, end: Identifier, graph: &Graph) -> i64 {
    // Flood fill from the back
    let distance_from_end  = build_distance_from_end(end, &graph);

    let mut total_ways = HashMap::<Identifier, i64>::new();
    let mut queue = PriorityQueue::<Identifier, i32>::new();
    total_ways.insert(start, 1);
    let Some(distance_from_end_from_start) = distance_from_end.get(&start) else {
        return 0;
    };
    queue.push(start, *distance_from_end_from_start);

    while let Some((id, _)) = queue.pop() {
        let node = graph.get(&id).unwrap();
        let ways_here = *total_ways.get(&id).unwrap();

        for edge in &node.to {
            if let Some(existing_ways) = total_ways.get(edge) {
                total_ways.insert(*edge, *existing_ways + ways_here);
            } else {
                total_ways.insert(*edge, ways_here);
                let Some(distance_from_end) = distance_from_end.get(edge) else {
                    // Can not reach end on this path, discard
                    continue;
                };
                queue.push(*edge, *distance_from_end);
            }
        }
    }

    return *total_ways.get(&end).unwrap();
}
