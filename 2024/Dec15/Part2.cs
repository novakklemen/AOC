using System.Text;

namespace Dec15;

internal class Part2
{
    public static long Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        // Parse input into grid and moves
        var (grid, moves) = ParseInput(input);

        PrintGrid(grid, @"InitialGrid.txt");

        // Solve the problem with solve2 logic
        int gpsSum = Solve(grid, moves);

        // Print the Grid
        PrintGrid(grid, @"FinalGrid.txt");

        return gpsSum;
    }

    private static (char[,], string) ParseInput(string[] lines)
    {
        List<string> gridLines = [];
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
        int cols = gridLines[0].Length * 2; // Double the columns for `[ ]`
        char[,] grid = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            List<char> row = [];
            foreach (var c in gridLines[i])
            {
                if (c == '#')
                {
                    row.Add('#');
                    row.Add('#');
                }
                else if (c == 'O')
                {
                    row.Add('[');
                    row.Add(']');
                }
                else if (c == '.')
                {
                    row.Add('.');
                    row.Add('.');
                }
                else if (c == '@')
                {
                    row.Add('@');
                    row.Add('.');
                }
            }
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = row[j];
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
        int nx = x + dx;
        int ny = y + dy;

        if (grid[nx, ny] == '#') return false;
        if (grid[nx, ny] == '.') return true;

        if (dy == 0)
        {
            if (grid[nx, ny] == ']')
                return TryPush(grid, nx, ny, dx, dy) && TryPush(grid, nx, ny - 1, dx, dy);
            else if (grid[nx, ny] == '[')
                return TryPush(grid, nx, ny, dx, dy) && TryPush(grid, nx, ny + 1, dx, dy);
        }
        else if (dy == -1 && grid[nx, ny] == ']') // Push left
        {
            return TryPush(grid, nx, ny - 1, dx, dy);
        }
        else if (dy == 1 && grid[nx, ny] == '[') // Push right
        {
            return TryPush(grid, nx, ny + 1, dx, dy);
        }

        return false;
    }

    private static void Push(char[,] grid, int x, int y, int dx, int dy)
    {
        int nx = x + dx;
        int ny = y + dy;

        if (grid[nx, ny] == '#') return;

        if (grid[nx, ny] == '.')
        {
            Swap(ref grid[x, y], ref grid[nx, ny]);
            return;
        }

        if (dy == 0)
        {
            if (grid[nx, ny] == ']')
            {
                Push(grid, nx, ny, dx, dy);
                Push(grid, nx, ny - 1, dx, dy);
                Swap(ref grid[x, y], ref grid[nx, ny]);
                return;
            }
            else if (grid[nx, ny] == '[')
            {
                Push(grid, nx, ny, dx, dy);
                Push(grid, nx, ny + 1, dx, dy);
                Swap(ref grid[x, y], ref grid[nx, ny]);
                return;
            }
        }
        else if (dy == -1 && grid[nx, ny] == ']') // Push left
        {
            Push(grid, nx, ny - 1, dx, dy);
            char temp = grid[nx, ny - 1];
            grid[nx, ny - 1] = grid[nx, ny];
            grid[nx, ny] = grid[x, y];
            grid[x, y] = temp;
        }
        else if (dy == 1 && grid[nx, ny] == '[') // Push right
        {
            Push(grid, nx, ny + 1, dx, dy);
            char temp = grid[nx, ny + 1];
            grid[nx, ny + 1] = grid[nx, ny];
            grid[nx, ny] = grid[x, y];
            grid[x, y] = temp;
        }
    }

    private static void Swap(ref char a, ref char b)
    {
        (b, a) = (a, b);
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
                if (grid[i, j] == '[')
                {
                    sum += 100 * i + j;
                }
            }
        }

        return sum;
    }

    private static string PrintGrid(char[,] grid, string? filePath = null)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        StringBuilder sb = new();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                sb.Append(grid[i, j]);
            }
            sb.AppendLine(); // Move to the next line after each row
        }

        if (filePath != null)
            File.WriteAllText(filePath, sb.ToString());

        return sb.ToString();
    }
}
