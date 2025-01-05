namespace Dec12;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllLines(@"input.txt");
        return CalculateTotalPrice(input);
    }

    static int CalculateTotalPrice(string[] grid)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;
        bool[,] visited = new bool[rows, cols];
        int totalPrice = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (!visited[i, j])
                {
                    (int area, int perimeter) = BFS(grid, visited, i, j);
                    totalPrice += area * perimeter;
                }
            }
        }

        return totalPrice;
    }

    static (int, int) BFS(string[] grid, bool[,] visited, int startX, int startY)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;
        char regionType = grid[startX][startY];
        int area = 0;
        int perimeter = 0;

        Queue<(int, int)> queue = new();
        queue.Enqueue((startX, startY));
        visited[startX, startY] = true;

        // Directions for moving up, down, left, right
        int[] dx = [-1, 1, 0, 0];
        int[] dy = [0, 0, -1, 1];

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            area++;

            for (int d = 0; d < 4; d++)
            {
                int nx = x + dx[d];
                int ny = y + dy[d];

                if (nx < 0 || ny < 0 || nx >= rows || ny >= cols || grid[nx][ny] != regionType)
                {
                    // Edge contributes to perimeter
                    perimeter++;
                }
                else if (!visited[nx, ny])
                {
                    visited[nx, ny] = true;
                    queue.Enqueue((nx, ny));
                }
            }
        }

        return (area, perimeter);
    }
}
