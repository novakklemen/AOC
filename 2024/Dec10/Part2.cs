namespace Dec10;

internal class Part2
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        int[,] map = ParseInput(input);
        var totalRating = CalculateTrailheadMetrics(map);

        return totalRating;
    }
    
    private static int[,] ParseInput(string[] input)
    {
        int rows = input.Length;
        int cols = input[0].Length;
        int[,] map = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                map[i, j] = input[i][j] - '0';
            }
        }

        return map;
    }

    private static int CalculateTrailheadMetrics(int[,] map)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        int totalRating = 0;

        // Find all trailheads (positions with height 0)
        List<(int x, int y)> trailheads = [];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (map[i, j] == 0)
                {
                    trailheads.Add((i, j));
                }
            }
        }

        foreach (var trailhead in trailheads)
        {
            totalRating += CalculateRatingForTrailhead(map, trailhead);
        }

        return totalRating;
    }

    private static int CalculateRatingForTrailhead(int[,] map, (int x, int y) trailhead)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        // Queue to explore paths, each path is tracked
        Queue<(int x, int y, List<(int, int)>)> queue = new();

        // Keep track of distinct paths ending at height 9
        HashSet<string> distinctPaths = [];

        // Start from the trailhead
        queue.Enqueue((trailhead.x, trailhead.y, [trailhead]));

        // Directions for up, down, left, right
        int[] dx = [-1, 1, 0, 0];
        int[] dy = [0, 0, -1, 1];

        while (queue.Count > 0)
        {
            var (x, y, path) = queue.Dequeue();
            int height = map[x, y];

            // If height 9 is reached, store the distinct path
            if (height == 9)
            {
                // Represent the path as a string for uniqueness
                string pathKey = string.Join("->", path);
                distinctPaths.Add(pathKey);
                continue;
            }

            // Explore neighbors
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols)
                {
                    int nextHeight = map[nx, ny];
                    if (nextHeight == height + 1)
                    {
                        // Create a new path by adding the current position
                        List<(int, int)> newPath = new(path) { (nx, ny) };
                        queue.Enqueue((nx, ny, newPath));
                    }
                }
            }
        }

        // The size of distinctPaths gives the total distinct trails
        return distinctPaths.Count;
    }
}
