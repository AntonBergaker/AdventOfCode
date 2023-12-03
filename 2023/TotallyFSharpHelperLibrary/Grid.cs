namespace TotallyFSharpHelperLibrary;
internal class Grid<T> {
    private readonly T[,] _array;

    public int Width { get; }
    public int Height { get; }

    public T this[int x, int y] {
        get => _array[x, y];
        set => _array[x, y] = value;
    }

    public Grid(int width, int height) {
        _array = new T[width, height];
        Width = width;
        Height = height;
    }

    public Grid(int width, int height, Func<int, int, T> initFunction) : this(width, height) {
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                _array[x, y] = initFunction(x, y);
            }
        }
    }

    public bool IsValidCoord(int x, int y) {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

}
