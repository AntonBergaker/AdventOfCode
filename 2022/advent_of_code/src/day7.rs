use std::{collections::HashMap, slice::Iter};

pub fn day7(input_lines: Vec<&str>) {
    let root_name = String::from("root");
    let mut root = Folder::new(&root_name);

    let mut iter = input_lines.iter();

    let mut return_to_root = false;

    populate_tree_root(&mut iter, &mut root, &mut return_to_root);

    fn populate_tree_root(iter: &mut Iter<&str>, folder: &mut Folder, return_to_root: &mut bool) {
        loop {
            *return_to_root = false;
            match iter.next() {
                Some(a) => if populate_tree_parse(a, iter, folder, return_to_root) { return },
                None => return,
            }
        }
    }

    fn populate_tree_loop(iter: &mut Iter<&str>, folder: &mut Folder, return_to_root: &mut bool) {
        loop {
            if *return_to_root {
                return;
            }
            match iter.next() {
                Some(a) => if populate_tree_parse(a, iter, folder, return_to_root) { return },
                None => return,
            }
        }
    }

    // returns true if previous method should go up one step
    fn populate_tree_parse(line: &str, iter: &mut Iter<&str>, folder: &mut Folder, return_to_root: &mut bool) -> bool {
        let words: Vec<&str> = line.split(' ').collect();

        match words[..] {
            ["$", "cd", "/"] => *return_to_root = true,
            ["$", "cd", ".."] => return true,
            ["$", "cd", destination] => populate_tree_loop(iter, 
                folder.folders.get_mut(&String::from(destination)).expect(&format!("Folder not found: {}", destination)),
                return_to_root),
            ["$", "ls"] => (),
            ["dir", dir_name] => folder.add_folder(Folder::new(dir_name)),
            [file_size, file_name] => folder.add_file(File::new(file_name, file_size.parse().unwrap())),
            _ => panic!()
        }   
        return false;
    }

    fn get_sum_of_folder_under_size(folder: &Folder, size: usize) -> usize {
        let mut total_size = 0;
        let folder_size = folder.get_size();
        if folder_size <= size {
            total_size += folder_size;
        }

        for pair in &folder.folders {
            total_size += get_sum_of_folder_under_size(&pair.1, size);
        }
        return total_size;
    }

    println!("Sum of directories under the size of 100 000: {}", get_sum_of_folder_under_size(&root, 100_000));

    fn find_smallest_folder_over_limit(folder: &Folder, limit: usize, smallest: &mut usize) {
        let folder_size = folder.get_size();
        if folder_size < limit {
            return;
        }

        for pair in &folder.folders {
            find_smallest_folder_over_limit(&pair.1, limit, smallest);
        }
        
        if folder_size < *smallest {
            *smallest = folder_size;
        }
    }


    let mut smallest = usize::MAX;
    let available_space = 70_000_000 - root.get_size();
    find_smallest_folder_over_limit(&root, 30_000_000 - available_space, &mut smallest);

    println!("Size of smallest directory to remove to free 30 000 000: {}", smallest);
}

struct Folder {
    pub name: String,
    pub folders: HashMap<String, Folder>,
    pub files: HashMap<String, File>,
}

impl Folder {
    pub fn new(name: &str) -> Self {
        return Self {
            name: String::from(name),
            folders: HashMap::new(),
            files: HashMap::new(),
        }
    }

    pub fn add_file(&mut self, file: File) {
        println!("Made file {}", &file.name);
        self.files.insert(file.name.clone(), file);
    }

    pub fn add_folder(&mut self, folder: Folder) {
        println!("Made folder {}", &folder.name);
        self.folders.insert(folder.name.clone(), folder);
    }

    pub fn get_size(&self) -> usize {
        let mut size = 0;
        for pair in &self.files {
            size += pair.1.size;
        }

        for pair in &self.folders {
            size += pair.1.get_size();
        }

        return size;
    }
}

struct File {
    pub name: String,
    pub size: usize,
}

impl File {
    pub fn new(name: &str, size: usize) -> Self {
        return Self {
            name: String::from(name),
            size: size,
        }
    }
}