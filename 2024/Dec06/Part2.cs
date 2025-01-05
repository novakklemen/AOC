namespace Dec06;

internal class Part2
{
    private const string GUARDS = "^>v<";
    private const char OBSTACLE = '#';

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
        char startDirection = ' ';

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (GUARDS.Contains(input[r][c]))
                {
                    guardX = c;
                    guardY = r;
                    startDirection = input[r][c];
                    break;
                }
            }
        }

        // Find all possible positions for the new obstruction
        List<(int x, int y)> validObstructionPositions = [];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                // Skip the starting position and existing obstacles
                if ((c == guardX && r == guardY) || input[r][c] == OBSTACLE)
                    continue;

                // Check if placing an obstacle here causes a loop
                if (SimulateWithObstacle(c, r, input, guardX, guardY, startDirection, rows, cols))
                {
                    validObstructionPositions.Add((c, r));
                }
            }
        }

        return validObstructionPositions.Count;
    }

    // Function to simulate the guard's movement
    private static bool SimulateWithObstacle(int obsX, int obsY, string[] currentInput, int guardStartX, int guardStartY, char startDirection, int rows, int cols)
    {
        int x = guardStartX, y = guardStartY;
        char dir = startDirection;
        HashSet<(int x, int y, char dir)> visitedStates = [];

        while (true)
        {
            // Add current state to visited
            if (!visitedStates.Add((x, y, dir)))
            {
                // If this state is revisited, we are in a loop
                return true;
            }

            int nextX = x + movements[dir].dx;
            int nextY = y + movements[dir].dy;

            // Check if the next position is out of bounds
            if (nextX < 0 || nextX >= cols || nextY < 0 || nextY >= rows)
                return false;

            // Check if the next position is an obstacle
            if (currentInput[nextY][nextX] == OBSTACLE || (nextX == obsX && nextY == obsY))
            {
                dir = rightTurn[dir]; // Turn right
            }
            else
            {
                // Move forward
                x = nextX;
                y = nextY;
            }
        }
    }
}
