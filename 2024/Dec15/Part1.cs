namespace Dec15;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        // Parse input into grid and moves
        var (grid, moves) = ParseInput(input);

        // Solve the problem
        int gpsSum = Solve(grid, moves);

        return gpsSum;
    }

    private static (char[,], string) ParseInput(string[] lines)
    {
        var gridLines = new List<string>();
        string moves = "";

        foreach (var line in lines)
        {
            if (line.Contains('#'))
            {
                gridLines.Add(line);
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                moves += line.Trim();
            }
        }

        int rows = gridLines.Count;
        int cols = gridLines[0].Length;
        char[,] grid = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = gridLines[i][j];
            }
        }

        return (grid, moves);
    }

    private static int Solve(char[,] grid, string moves)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int robotX = 0, robotY = 0;

        // Locate the robot's starting position
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == '@')
                {
                    robotX = i;
                    robotY = j;
                    grid[i, j] = '.'; // Clear robot's position
                    break;
                }
            }
        }

        // Direction vectors for movements
        Dictionary<char, (int dx, int dy)> directions = new()
        {
            { '<', (0, -1) }, { '^', (-1, 0) },
            { '>', (0, 1) }, { 'v', (1, 0) }
        };

        foreach (var move in moves)
        {
            if (!directions.ContainsKey(move)) continue;

            var (dx, dy) = directions[move];
            if (TryPush(grid, robotX, robotY, dx, dy))
            {
                Push(grid, robotX, robotY, dx, dy);
                robotX += dx;
                robotY += dy;
            }
        }

        // Calculate GPS sum
        return CalculateGpsSum(grid);
    }

    private static bool TryPush(char[,] grid, int x, int y, int dx, int dy)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        x += dx;
        y += dy;

        while (x >= 0 && x < rows && y >= 0 && y < cols)
        {
            if (grid[x, y] == '#') return false; // Wall blocks
            if (grid[x, y] == '.') return true; // Free space
            x += dx;
            y += dy;
        }

        return false; // Out of bounds
    }

    private static void Push(char[,] grid, int x, int y, int dx, int dy)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        Stack<(int x, int y)> boxes = new();

        x += dx;
        y += dy;

        while (x >= 0 && x < rows && y >= 0 && y < cols && grid[x, y] == 'O')
        {
            boxes.Push((x, y));
            x += dx;
            y += dy;
        }

        while (boxes.Count > 0)
        {
            var (bx, by) = boxes.Pop();
            grid[bx + dx, by + dy] = 'O';
            grid[bx, by] = '.';
        }
    }

    private static int CalculateGpsSum(char[,] grid)
    {
        int sum = 0;
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 'O')
                {
                    sum += 100 * i + j;
                }
            }
        }

        return sum;
    }
}
