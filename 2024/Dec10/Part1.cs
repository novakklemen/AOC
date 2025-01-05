namespace Dec10;

internal class Part1
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        int[,] map = ParseInput(input);
        return CalculateTrailheadScores(map);
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

    private static int CalculateTrailheadScores(int[,] map)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        int totalScore = 0;

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
            totalScore += CalculateScoreForTrailhead(map, trailhead);
        }

        return totalScore;
    }

    private static int CalculateScoreForTrailhead(int[,] map, (int x, int y) trailhead)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        // BFS to explore all possible hiking trails
        Queue<(int x, int y, int height)> queue = new();
        HashSet<(int x, int y)> visited = [];

        queue.Enqueue((trailhead.x, trailhead.y, 0));
        visited.Add(trailhead);

        int score = 0;

        // Directions for up, down, left, right
        int[] dx = [-1, 1, 0, 0];
        int[] dy = [0, 0, -1, 1];

        while (queue.Count > 0)
        {
            var (x, y, height) = queue.Dequeue();

            // If height 9 is reached, count it for the score
            if (height == 9)
            {
                score++;
                continue;
            }

            // Explore neighbors
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && !visited.Contains((nx, ny)))
                {
                    int nextHeight = map[nx, ny];
                    if (nextHeight == height + 1)
                    {
                        queue.Enqueue((nx, ny, nextHeight));
                        visited.Add((nx, ny));
                    }
                }
            }
        }

        return score;
    }
}
