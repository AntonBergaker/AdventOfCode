module Day2

open System

type Grab = { Count: int; Color: string }

type Round = { Red: int; Green: int; Blue: int }

type Game = {Number: int; Rounds: Round array }

let day2 (input: string[]) = 

    let getGrab (grabString: string)  =
        let pieces = grabString.Split(' ')
        {Count = Int32.Parse(pieces[0]); Color = pieces[1]}

    let getRound (roundString: string) = 
        let grabs =
            roundString.Split(", ") |> Seq.map getGrab

        let findGame color =
            match grabs |> Seq.tryFind (fun x -> x.Color = color) with
            | Some(x) -> x.Count
            | None -> 0

        {
            Red = findGame "red";
            Green = findGame "green";
            Blue = findGame "blue";
        }

    let getGameNumber (gameNumberString: string) =
        Int32.Parse (gameNumberString.Split(' ')[1])

    let getGame (gameString: string) = 
        let pieces = gameString.Split(": ")
        let rounds = pieces[1].Split("; ") |> Seq.map getRound |> Seq.toArray
        { Number = getGameNumber pieces[0]; Rounds = rounds }

    let games = input |> Seq.map getGame|> Seq.toArray

    // Part 1
    let sumOfPossibleGames = 
        games
        |> Seq.filter (fun game -> 
            game.Rounds |> Seq.forall (fun x -> x.Red <= 12 && x.Green <= 13 && x.Blue <= 14)
        )
        |> Seq.map (fun game -> game.Number)
        |> Seq.sum
    
    printfn "Sum of possible games: %d" sumOfPossibleGames


    // Part 2
    let sumOfProductOfMinimum =
        let getMax (game, predicate) =
            game.Rounds 
            |> Seq.map predicate
            |> Seq.max

        games
        |> Seq.map (fun game ->
            getMax (game, fun x -> x.Red) *
            getMax (game, fun x -> x.Green) *
            getMax (game, fun x -> x.Blue)
        )
        |> Seq.sum

    printfn "Product of minimum cubes: %d" sumOfProductOfMinimum