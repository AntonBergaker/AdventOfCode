use std::{slice::Iter, cmp::Ordering};

pub fn day13(input_lines: Vec<&str>) {
    part1(&input_lines);
    part2(&input_lines);
}

fn part1(input_lines: &Vec<&str>) {
    let mut wrong_pairs = 0;
    for i in (0..input_lines.len()).step_by(3) {
        let a = parse(input_lines[i+0]);
        let b = parse(input_lines[i+1]);

        let result = a.cmp(&b);
        if result == Ordering::Less {
            wrong_pairs += 1 + i/3;
        }
    }

    println!("Number of incorrectly sorted pairs: {}", wrong_pairs);
}

fn part2(input_lines: &Vec<&str>) {
    let mut values: Vec<Value> = input_lines.iter().filter(|x| x.is_empty() == false).map(|x| parse(x)).collect();
        
    let divider_creator = |a| { Value::List(vec![Value::List(vec![Value::Integer(a)])]) };

    let divider2 = divider_creator(2);
    values.push(divider_creator(2));

    let divider6 = divider_creator(6);
    values.push(divider_creator(6));
    
    values.sort();

    let divider2_index = values.iter().position(|x| x == &divider2).unwrap() + 1;
    let divider6_index = values.iter().position(|x| x == &divider6).unwrap() + 1;

    println!("Decoder key: {}", divider2_index * divider6_index);
}

fn parse(input: &str) -> Value {
    let tokens = tokenize(input);
    fn parse_interal(token: &Token, iter: &mut Iter<Token>) -> Value {
        match token {
            Token::Open => {
                let mut entries: Vec<Value> = Vec::new();
                loop {
                    let token = iter.next().unwrap();
                    match token {
                        Token::Close => break,
                        _ => (),
                    }
                    entries.push(parse_interal(token, iter));
                }
                Value::List(entries)
            },
            Token::Value(a) => Value::Integer(*a),
            a => panic!("Unexpected token: {:?}", a)
        }
    }

    let mut iter = tokens.iter();
    let token = iter.next().unwrap();
    return parse_interal(token, &mut iter);
}

fn tokenize(input: &str) -> Vec<Token> {
    let chars: Vec<char> = input.chars().collect();
    let mut tokens: Vec<Token> = Vec::new();

    let mut i = 0;
    while i < chars.len() {
        match chars[i] {
            '[' => tokens.push(Token::Open),
            ']' => tokens.push(Token::Close),
            ',' => (), // Ignore as its just used as a seperator
            a if a.is_numeric() => {
                let mut val = 0;
                while i < chars.len() && chars[i].is_numeric() {
                    val = val*10 + (chars[i] as i32 - '0' as i32);
                    i+=1;
                }
                // Remove 1 from i as it gets increased in the loop
                i-=1;
                tokens.push(Token::Value(val));
            },
            a => panic!("Unknown token: {}", a)
        };

        i+=1;
    }

    return tokens;
}

#[derive(Debug, Clone, Copy)]
enum Token {
    Open,
    Close,
    Value(i32),
}

#[derive(Debug, Clone)]
enum Value {
    List(Vec<Value>),
    Integer(i32),
}

impl PartialEq for Value {
    fn eq(&self, other: &Self) -> bool {
        return self.cmp(other) == Ordering::Equal;
    }
}

impl Eq for Value {}

impl PartialOrd for Value {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

impl Ord for Value {
    fn cmp(&self, other: &Self) -> Ordering {
        return match (&self, other) {
            (Value::Integer(a), Value::Integer(b)) => a.cmp(b),
            (Value::List(a), Value::List(b)) => {
                let min = usize::min(a.len(), b.len());
                for i in 0..min {
                    match (&a[i]).cmp(&b[i]) {
                        Ordering::Greater => return Ordering::Greater,
                        Ordering::Less => return Ordering::Less,
                        Ordering::Equal => (), // keep looping
                    }
                }
                a.len().cmp(&b.len())
            }
            (Value::Integer(a), Value::List(_)) => (&Value::List(vec![Value::Integer(*a)])).cmp(other),
            (Value::List(_), Value::Integer(b)) => self.cmp(&Value::List(vec![Value::Integer(*b)])),
        };
    }
}