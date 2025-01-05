namespace Dec18;

internal class Part2
{
    public static string Execute()
    {
        var input = File.ReadAllLines("input.txt");

        // Parse the input into a list of coordinates
        var fallingBytes = input.Select(line => line.Split(',').Select(int.Parse).ToArray())
                                 .Select(coordinates => (X: coordinates[0], Y: coordinates[1]))
                                 .ToList();

        // to get the size of the grid
        int gridSize = fallingBytes.Max(coordinates => coordinates.X) + 1;
        var grid = new bool[gridSize, gridSize]; // False means safe, True means corrupted

        // Simulate the falling bytes and check connectivity
        for (int i = 0; i < fallingBytes.Count; i++)
        {
            var (x, y) = fallingBytes[i];
            grid[x, y] = true; // Mark the cell as corrupted

            // Check if the path to the exit is still valid
            if (!IsPathToExit(grid, gridSize))
            {
                return $"{x},{y}"; // Return the first byte that blocks the path
            }
        }

        return "No blocking byte found"; // Fallback (shouldn't occur with valid input)
    }

    private static bool IsPathToExit(bool[,] grid, int gridSize)
    {
        var directions = new (int dx, int dy)[]
        {
            (0, 1),  // Down
            (1, 0),  // Right
            (0, -1), // Up
            (-1, 0)  // Left
        };

        var visited = new bool[gridSize, gridSize];
        var queue = new Queue<(int x, int y)>();
        queue.Enqueue((0, 0));
        visited[0, 0] = true;

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            // If we reached the exit
            if (x == gridSize - 1 && y == gridSize - 1)
            {
                return true;
            }

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx, ny = y + dy;

                if (nx >= 0 && nx < gridSize && ny >= 0 && ny < gridSize &&
                    !grid[nx, ny] && !visited[nx, ny])
                {
                    visited[nx, ny] = true;
                    queue.Enqueue((nx, ny));
                }
            }
        }

        return false; // No path found
    }
}
