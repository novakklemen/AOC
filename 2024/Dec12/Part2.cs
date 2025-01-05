namespace Dec12;

internal class Part2
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");
        var grid = ParseInput(input);
        return PriceOfAllRegions(grid);
    }
    private static char[,] ParseInput(string[] input)
    {
        int rows = input.Length;
        int cols = input[0].Length;
        var grid = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = input[i][j];
            }
        }

        return grid;
    }

    private static int Region(int x, int y, char[,] grid, HashSet<(int, int)> seen)
    {
        var current = grid[x, y];
        var queue = new Queue<(int, int)>();
        queue.Enqueue((x, y));
        var count = 0;
        var pos = new HashSet<(double, double)>();

        while (queue.Count > 0)
        {
            var (i, j) = queue.Dequeue();

            if (i < 0 || j < 0 || i >= grid.GetLength(0) || j >= grid.GetLength(1) || grid[i, j] != current)
            {
                continue;
            }

            if (!seen.Add((i, j)))
                continue;

            pos.Add((i, j));

            count++;
            queue.Enqueue((i + 1, j));
            queue.Enqueue((i - 1, j));
            queue.Enqueue((i, j + 1));
            queue.Enqueue((i, j - 1));
        }

        var possibleCorners = new HashSet<(double, double)>();
        var directions = new List<(double, double)> { (-0.5, -0.5), (0.5, -0.5), (-0.5, 0.5), (0.5, 0.5) };

        foreach (var (i, j) in pos)
        {
            foreach (var (ni, nj) in directions.Select(d => (i + d.Item1, j + d.Item2)))
            {
                possibleCorners.Add((ni, nj));
            }
        }

        var sides = 0;

        foreach (var (ci, cj) in possibleCorners)
        {
            var connected = directions
                .Select(d => (ci + d.Item1, cj + d.Item2))
                .Where(pos.Contains)
                .ToArray();

            switch (connected.Length)
            {
                case 1:
                case 3:
                    sides += 1;
                    break;
                case 2:
                    var diff = (
                        connected[0].Item1 - connected[1].Item1,
                        connected[0].Item2 - connected[1].Item2);

                    if (diff == (1, 1) || diff == (-1, -1) || diff == (-1, 1) || diff == (1, -1))
                        sides += 2;
                    break;
            }
        }

        return sides * count;
    }

    private static (int, int) Next(char[,] grid, HashSet<(int, int)> seen)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (!seen.Contains((i, j)))
                    return (i, j);
            }
        }

        return (-1, -1);
    }

    public static int PriceOfAllRegions(char[,] grid)
    {
        var seen = new HashSet<(int, int)>();
        var sum = 0;
        var (x, y) = (0, 0);

        while (x != -1)
        {
            sum += Region(x, y, grid, seen);
            (x, y) = Next(grid, seen);
        }

        return sum;
    }
}
