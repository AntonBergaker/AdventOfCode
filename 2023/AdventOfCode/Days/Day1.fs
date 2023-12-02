module Day1

open System
open System.Text

let day1 (input: string[]) =
    // Filter out everything but numbers, and grab the first and last
    
    let sumFirstAndLastNumbers (lines: string seq) = 
        lines
        |> Seq.map (fun x -> 
            String.Concat (x
                |> Seq.filter (fun y -> Char.IsDigit y)
        ))
        |> Seq.map (fun x -> Int32.Parse x.[..0] * 10 + Int32.Parse x.[(x.Length - 1) ..])
        |> Seq.sum
    
    printfn "Sum of digits: %d" (sumFirstAndLastNumbers input)
   
    let numbers = dict [
        "one", 1;
        "two", 2;
        "three", 3;
        "four", 4;
        "five", 5;
        "six", 6;
        "seven", 7;
        "eight", 8;
        "nine", 9;
    ]

    let rec replaceLineWithDigits (line: string) =
        let sb = new StringBuilder()
        for i in 0..line.Length-1 do
            match numbers |> Seq.tryFind (fun x -> line[i..].StartsWith x.Key ) with
            | Some(x) -> sb.Append (x.Value.ToString()) |> ignore
            | None -> sb.Append (line[i]) |> ignore

        sb.ToString()

    let sumAndReplaceLines (lines: string seq) =
        sumFirstAndLastNumbers (
        lines
        |> Seq.map (fun x -> replaceLineWithDigits x)
        )


    printfn "Sum of digits and spelled out digits: %d" (sumAndReplaceLines input)



(*
This is a monument to all my sins. I wanted to solve it in a very F# way, but as you can see I discovered the wonderous
edge case where the tail of a number actually was important to the rest of the solution. The solution below is
very lovely, in the way an ugly baby is. Unfortunately for my own sanity it's been aborted.

let rec replaceLineWithDigits (line: char list) =
    match line with
    | 'o' :: 'n' :: 'e' :: tail -> '1' :: replaceLineWithDigits ('n' :: 'e' :: tail)
    | 't' :: 'w' :: 'o' :: tail -> '2' :: replaceLineWithDigits ('w' :: 'o' :: tail)
    | 't' :: 'h' :: 'r' :: 'e' :: 'e' :: tail -> '3' :: replaceLineWithDigits ('h' :: 'r' :: 'e' :: 'e' :: tail)
    | 'f' :: 'o' :: 'u' :: 'r' :: tail -> '4' :: replaceLineWithDigits ('o' :: 'u' :: 'r' :: tail)
    | 'f' :: 'i' :: 'v' :: 'e' :: tail -> '5' :: replaceLineWithDigits ('i' :: 'v' :: 'e' :: tail)
    | 's' :: 'i' :: 'x' ::  tail -> '6' :: replaceLineWithDigits ('i' :: 'x' :: tail)
    | 's' :: 'e' :: 'v' :: 'e' :: 'n' :: tail -> '7' :: replaceLineWithDigits ('e' :: 'v' :: 'e' :: 'n' :: tail)
    | 'e' :: 'i' :: 'g' :: 'h' :: 't' :: tail -> '8' :: replaceLineWithDigits ('i' :: 'g' :: 'h' :: 't' :: tail)
    | 'n' :: 'i' :: 'n' :: 'e' :: tail -> '9' :: replaceLineWithDigits ('i' :: 'n' :: 'e' :: tail)
    | head :: tail -> head :: replaceLineWithDigits tail
    | [] -> []
*)