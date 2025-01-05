namespace Dec18;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllLines("input.txt");

        // Parse the input into a list of coordinates
        var fallingBytes = input.Select(line => line.Split(',').Select(int.Parse).ToArray())
                                 .Select(coordinates => (X: coordinates[0], Y: coordinates[1]))
                                 .ToList();

        // to get the size of the grid
        int gridSize = fallingBytes.Max(coordinates => coordinates.X) + 1;
        var grid = new bool[gridSize, gridSize]; // False means safe, True means corrupted

        // Simulate the first 1024 bytes falling
        for (int i = 0; i < Math.Min(1024, fallingBytes.Count); i++)
        {
            var (x, y) = fallingBytes[i];
            grid[x, y] = true; // Mark the cell as corrupted
        }

        // Use BFS to find the shortest path
        return FindShortestPath(grid, gridSize);
    }

    private static int FindShortestPath(bool[,] grid, int gridSize)
    {
        var directions = new (int dx, int dy)[]
        {
            (0, 1),  // Down
            (1, 0),  // Right
            (0, -1), // Up
            (-1, 0)  // Left
        };

        var visited = new bool[gridSize, gridSize];
        var queue = new Queue<(int x, int y, int steps)>();
        queue.Enqueue((0, 0, 0));
        visited[0, 0] = true;

        while (queue.Count > 0)
        {
            var (x, y, steps) = queue.Dequeue();

            // If we reached the exit
            if (x == gridSize - 1 && y == gridSize - 1)
            {
                return steps;
            }

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx, ny = y + dy;

                if (nx >= 0 && nx < gridSize && ny >= 0 && ny < gridSize &&
                    !grid[nx, ny] && !visited[nx, ny])
                {
                    visited[nx, ny] = true;
                    queue.Enqueue((nx, ny, steps + 1));
                }
            }
        }

        return -1; // No path found
    }
}
