namespace Dec16;

internal class Part2
{
    public static long Execute()
    {
        // Read the input file and process it to find the best spots
        var input = File.ReadAllText("input.txt");
        return FindBestSpots(GetMap(input));
    }

    // Define directions using integer tuples
    private static readonly (int dx, int dy) North = (0, -1);
    private static readonly (int dx, int dy) South = (0, 1);
    private static readonly (int dx, int dy) West = (-1, 0);
    private static readonly (int dx, int dy) East = (1, 0);

    // Find the number of best spots by tracking shortest paths
    private static int FindBestSpots(Dictionary<(int x, int y), char> map)
    {
        var shortestPathCosts = Dijkstra(map, Goal(map));
        var start = Start(map);

        var priorityQueue = new PriorityQueue<((int x, int y) pos, (int dx, int dy) dir), int>();
        priorityQueue.Enqueue(start, shortestPathCosts[start]);

        var bestSpots = new HashSet<((int x, int y) pos, (int dx, int dy) dir)> { start };

        // Process the queue to track the best spots
        while (priorityQueue.TryDequeue(out var state, out var remainingScore))
        {
            foreach (var (next, score) in Steps(map, state, true))
            {
                var nextRemainingScore = remainingScore - score;
                if (!bestSpots.Contains(next) && shortestPathCosts[next] == nextRemainingScore)
                {
                    bestSpots.Add(next);
                    priorityQueue.Enqueue(next, nextRemainingScore);
                }
            }
        }

        // Return the count of unique positions in the best spots
        return bestSpots.Select(state => state.pos).Distinct().Count();
    }

    // Locate the goal (E) in the map
    private static (int x, int y) Goal(Dictionary<(int x, int y), char> map)
    {
        foreach (var kvp in map)
        {
            if (kvp.Value == 'E') return kvp.Key;
        }
        throw new InvalidOperationException("Goal not found.");
    }

    // Locate the start (S) in the map and set the initial direction to East
    private static ((int x, int y) pos, (int dx, int dy) dir) Start(Dictionary<(int x, int y), char> map)
    {
        foreach (var kvp in map)
        {
            if (kvp.Value == 'S') return (kvp.Key, East);
        }
        throw new InvalidOperationException("Start not found.");
    }

    // Implementation of Dijkstra's algorithm
    private static Dictionary<((int x, int y) pos, (int dx, int dy) dir), int> Dijkstra(Dictionary<(int x, int y), char> map, (int x, int y) goal)
    {
        var shortestPathCosts = new Dictionary<((int x, int y) pos, (int dx, int dy) dir), int>();
        var priorityQueue = new PriorityQueue<((int x, int y) pos, (int dx, int dy) dir), int>();

        // Initialize the priority queue with the goal and all possible directions
        foreach (var dir in new[] { North, East, West, South })
        {
            priorityQueue.Enqueue((goal, dir), 0);
            shortestPathCosts[(goal, dir)] = 0;
        }

        // Process the priority queue to find the shortest path
        while (priorityQueue.TryDequeue(out var cur, out var totalDistance))
        {
            foreach (var (next, score) in Steps(map, cur, false))
            {
                var nextCost = totalDistance + score;
                if (nextCost < shortestPathCosts.GetValueOrDefault(next, int.MaxValue))
                {
                    shortestPathCosts[next] = nextCost;
                    priorityQueue.Enqueue(next, nextCost);
                }
            }
        }

        return shortestPathCosts;
    }

    // Generate possible steps (next states and costs) from the current state
    private static IEnumerable<(((int x, int y) pos, (int dx, int dy) dir) state, int cost)> Steps(Dictionary<(int x, int y), char> map, ((int x, int y) pos, (int dx, int dy) dir) state, bool forward)
    {
        foreach (var dir in new[] { North, East, West, South })
        {
            if (dir == state.dir)
            {
                // Move forward or backward based on the direction
                var pos = forward ? (state.pos.x + dir.dx, state.pos.y + dir.dy) : (state.pos.x - dir.dx, state.pos.y - dir.dy);
                if (map.GetValueOrDefault(pos) != '#')
                {
                    yield return ((pos, dir), 1);
                }
            }
            else if (dir != (-state.dir.dx, -state.dir.dy))
            {
                // Turning costs 1000 points
                yield return ((state.pos, dir), 1000);
            }
        }
    }

    // Parse the input map into a dictionary for easier navigation
    private static Dictionary<(int x, int y), char> GetMap(string input)
    {
        var map = new Dictionary<(int x, int y), char>();
        var rows = input.Split('\n');

        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                map[(x, y)] = rows[y][x];
            }
        }

        return map;
    }
}
