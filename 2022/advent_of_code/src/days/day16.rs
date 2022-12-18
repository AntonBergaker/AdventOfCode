use std::collections::{HashMap, HashSet, VecDeque};

use regex::Regex;

use crate::data_types::grid::Grid;

pub fn day16(input_lines: Vec<&str>) {
    let mut flow_nodes: Vec<FlowNode> = Vec::new();
    let mut all_nodes: HashMap<String, FullNode> = HashMap::new();

    // I don't respect regex. This is not an admission of it being useful.
    let name_regex = Regex::new(r"Valve [A-Z]+").unwrap();
    let flow_regex = Regex::new(r"flow rate=\d+").unwrap();
    let connections_regex = Regex::new(r"valve(s)? ([A-Z]+(, )?)+").unwrap();

    for line in input_lines {
        let name = String::from(name_regex.find(line).unwrap().as_str().trim_start_matches("Valve "));
        let flow: i32 = flow_regex.find(line).unwrap().as_str().trim_start_matches("flow rate=").parse().unwrap();
        let connections_str = connections_regex.find(line).unwrap().as_str().trim_start_matches("valves ").trim_start_matches("valve ");
        let connections: Vec<String> = connections_str.split(", ").map(|x| String::from(x)).collect();

        let flow_index;

        if name == "AA" || flow > 0 {
            flow_index = Some(flow_nodes.len() as u32);
        } else {
            flow_index = None;
        }

        all_nodes.insert(name.clone(), FullNode {
            id: name.clone(),
            edges: connections,
            flow_node_index: flow_index,
        });
        if flow_index != None {
            flow_nodes.push(FlowNode {
                id: String::from(name.clone()),
                flow
            });
        }
    }

    let distance_matrix = build_distance_matrix(&all_nodes, flow_nodes.len());

    let start_node = flow_nodes.iter().position(|x| x.id == "AA").unwrap() as u32;

    {
        let mut cache: HashMap<u32, i32> = HashMap::new();
        let without_elephant = dfs_release(start_node as u32, 31 /* Add 1 to offset opening AA */, 0, &mut cache, &flow_nodes, &distance_matrix, None);
        println!("Steam released without elephant: {}", without_elephant);
    }

    {
        let mut cache: HashMap<u32, i32> = HashMap::new();
        let elephant_instructions = ElephantInstructions {
            time_left: 27,
            start_node: start_node
        };
        let with_elephant = dfs_release(start_node as u32, 27 /* Add 1 to offset opening AA */, 0, &mut cache, &flow_nodes, &distance_matrix, Some(&elephant_instructions));

        println!("Steam released with elephant: {}", with_elephant);
    }
}

fn dfs_release(node: u32, time_left: i32, visited: u32, cache: &mut HashMap<u32, i32>, nodes: &Vec<FlowNode>, distance_matrix: &Grid<u32>, elephant: Option<&ElephantInstructions>) -> i32 {
    if time_left < 0 {
        return 0;
    }
    
    let cache_key = make_key(node, time_left as u32, visited, elephant.is_some());
    let cached_value = cache.get(&cache_key);
    if cached_value != None {
        return *cached_value.unwrap();
    }

    let new_visited = set_bit(visited, node);
    let release_val = nodes[node as usize].flow * (time_left-1);
    let mut best_route = 0;

    // Test if this is a good time to release the time traveling elephant
    if elephant.is_some() {
        let elephant_instructions = elephant.unwrap();
        let route_val = dfs_release(elephant_instructions.start_node, elephant_instructions.time_left, new_visited, cache, nodes, distance_matrix, None);
        if route_val > best_route {
            best_route = route_val;
        }
    }

    for i in 0..nodes.len() {
        let new_node = i as u32;
        // Skip already visited
        if is_bit(new_visited, new_node) {
            continue;
        }

        let route_val = dfs_release(new_node, time_left - 1 - distance_matrix[(node as usize, i)] as i32, new_visited, cache, nodes, distance_matrix, elephant);
        if route_val > best_route {
            best_route = route_val;
        }
    }

    let final_val = release_val + best_route;

    cache.insert(cache_key, final_val);
    return final_val;
}

fn make_key(node: u32, time_left: u32, visited: u32, has_elephant: bool) -> u32 {
    // 1 bit for elephant
    // 8 bits for node
    // 8 bits for time_left
    // 16 bits for visited
    return ((has_elephant as u32) << 31) | (node << 24) | (time_left << 16) | visited;
}
fn set_bit(value: u32, index: u32) -> u32 {
    return value | (1 << index);
}

fn is_bit(value: u32, index: u32) -> bool {
    return (value & (1 << index)) > 0;
}

fn build_distance_matrix(nodes: &HashMap<String, FullNode>, count: usize) -> Grid<u32> {

    // Create a distance matrix containing the flow nodes
    let mut distance_matrix: Grid<u32> = Grid::new(0, count, count);

    for node in nodes.values() {
        if node.flow_node_index == None {
            continue;
        }

        let node_index = node.flow_node_index.unwrap() as usize;

        let mut visited: HashSet<String> = HashSet::new();
        visited.insert(node.id.clone());

        let mut destinations: VecDeque<(&FullNode, u32)> = VecDeque::new();
        destinations.push_back((node, 0));

        while destinations.is_empty() == false {
            let (target_node, distance) = destinations.pop_front().unwrap();

            match target_node.flow_node_index {
                Some(a) => distance_matrix[(node_index, a as usize)] = distance,
                _ => ()
            };

            for edge in &target_node.edges {
                if visited.contains(edge) {
                    continue;
                }

                visited.insert(edge.clone());
                destinations.push_back((&nodes[edge], distance+1));
            }
        }
    }

    return distance_matrix;
}

struct ElephantInstructions {
    start_node: u32,
    time_left: i32,
}

struct FullNode {
    id: String,
    edges: Vec<String>,
    flow_node_index: Option<u32>,
}

struct FlowNode {
    id: String,
    flow: i32,
}