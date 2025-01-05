namespace Dec25;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllLines("input.txt");

        // Parse the input into separate lock and key grids
        var locks = ParseGrids(input, true);
        var keys = ParseGrids(input, false);

        // Count the number of fitting lock-key pairs
        long fittingPairs = CountFittingPairs(locks, keys);

        return fittingPairs;
    }

    private static List<Grid> ParseGrids(string[] input, bool isLock)
    {
        var grids = new List<Grid>();
        var currentGridLines = new List<string>();

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (currentGridLines.Count > 0)
                {
                    var grid = Grid.FromText(currentGridLines);
                    if ((isLock && grid[0, 0] == '#') || (!isLock && grid[0, 0] != '#'))
                    {
                        grids.Add(grid);
                    }
                    currentGridLines.Clear();
                }
            }
            else
            {
                currentGridLines.Add(line);
            }
        }

        if (currentGridLines.Count > 0)
        {
            var grid = Grid.FromText(currentGridLines);
            if ((isLock && grid[0, 0] == '#') || (!isLock && grid[0, 0] != '#'))
            {
                grids.Add(grid);
            }
        }

        return grids;
    }

    private static long CountFittingPairs(List<Grid> locks, List<Grid> keys)
    {
        long fittingPairs = 0;

        foreach (var lockGrid in locks)
        {
            foreach (var keyGrid in keys)
            {
                if (DoGridsFit(lockGrid, keyGrid))
                {
                    fittingPairs++;
                }
            }
        }

        return fittingPairs;
    }

    private static bool DoGridsFit(Grid lockGrid, Grid keyGrid)
    {
        foreach (var (x, y) in lockGrid.GetAllCoordinates())
        {
            // Check for overlapping lock and key pins
            if (lockGrid[x, y] == '#' && keyGrid[x, y] == '#')
            {
                return false;
            }
        }

        return true;
    }

    private class Grid(char[,] data)
    {
        private readonly char[,] _data = data;

        public char this[int x, int y] => _data[x, y];

        public static Grid FromText(List<string> lines)
        {
            int rows = lines.Count;
            int cols = lines[0].Length;
            var data = new char[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = lines[i][j];
                }
            }
            return new Grid(data);
        }

        public IEnumerable<(int x, int y)> GetAllCoordinates()
        {
            for (int x = 0; x < _data.GetLength(0); x++)
            {
                for (int y = 0; y < _data.GetLength(1); y++)
                {
                    yield return (x, y);
                }
            }
        }
    }
}
