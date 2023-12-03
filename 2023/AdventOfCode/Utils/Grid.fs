module Grid

type Grid<'a>(width: int, height:int, init: (int -> int -> 'a)) = 
    member private this.array = [| for x in 0..(width-1) -> [|for y in 0..(height-1) -> (init x y) |] |]

    member this.Item
        with get (x, y) = this.array[x][y]
        and set (x, y) value = this.array[x][y] <- value

    member this.Width = width
    member this.Height = height

    member this.IsValidCoord(x: int, y:int) =
        x >= 0 && y >= 0 && x < this.Width && y < this.Height

    new(width, height, init) = Grid(width, height, init)