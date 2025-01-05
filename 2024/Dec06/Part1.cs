namespace Dec06;

internal class Part1
{
    private const char OBSTACLE = '#';
    private const string GUARDS = "^>v<";

    // Define movement directions
    private static readonly Dictionary<char, (int dx, int dy)> movements = new()
    {
        { '^', (0, -1) },
        { '>', (1, 0) },
        { 'v', (0, 1) },
        { '<', (-1, 0) }
    };

    // Define right-turn transitions
    private static readonly Dictionary<char, char> rightTurn = new()
    {
        { '^', '>' },
        { '>', 'v' },
        { 'v', '<' },
        { '<', '^' }
    };

    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        // Parse the input to find the guard's starting position and direction
        int rows = input.Length;
        int cols = input[0].Length;
        int guardX = 0, guardY = 0;
        char direction = ' ';

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (GUARDS.Contains(input[r][c]))
                {
                    guardX = c;
                    guardY = r;
                    direction = input[r][c];
                    break;
                }
            }
        }

        // Set to track visited positions
        HashSet<(int x, int y)> visited = [(guardX, guardY)];

        // Simulate guard's movement
        while (true)
        {
            int nextX = guardX + movements[direction].dx;
            int nextY = guardY + movements[direction].dy;

            // Check if next position is out of bounds
            if (nextX < 0 || nextX >= cols || nextY < 0 || nextY >= rows)
                break;

            // Check if next position is an obstacle
            if (input[nextY][nextX] == OBSTACLE)
            {
                direction = rightTurn[direction]; // Turn right
            }
            else
            {
                // Move forward
                guardX = nextX;
                guardY = nextY;
                visited.Add((guardX, guardY));
            }
        }

        return visited.Count;
    }
}
