namespace Dec20;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllLines("input.txt");

        var map = input.Select(line => line.ToCharArray()).ToArray();
        var rows = map.Length;
        var cols = map[0].Length;

        // Locate start (S) and end (E) positions
        (int x, int y) start = (0, 0);
        (int x, int y) end = (0, 0);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (map[r][c] == 'S') start = (r, c);
                if (map[r][c] == 'E') end = (r, c);
            }
        }

        var steps = new Dictionary<(int x, int y), int>();
        var curLoc = start;
        steps[start] = 0;
        int stepCount = 0;
        int savingsCount = 0;

        do
        {
            curLoc = Neighbors(curLoc, rows, cols)
                .FirstOrDefault(a => map[a.x][a.y] != '#' && !steps.ContainsKey(a));
            steps[curLoc] = ++stepCount;
        } while (curLoc != end);

        foreach (var (loc, s) in steps)
        {
            foreach (var n in Neighbors(loc, rows, cols, distance: 2))
            {
                if (steps.TryGetValue(n, out int value))
                {
                    int saved = value - steps[loc] - 2;
                    if (saved >= 100) savingsCount++;
                }
            }
        }

        return savingsCount;
    }

    private static IEnumerable<(int x, int y)> Neighbors((int x, int y) loc, int rows, int cols, int distance = 1)
    {
        var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        foreach (var (dx, dy) in directions)
        {
            for (int d = 1; d <= distance; d++)
            {
                var nx = loc.x + dx * d;
                var ny = loc.y + dy * d;
                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols)
                {
                    yield return (nx, ny);
                }
            }
        }
    }
}
