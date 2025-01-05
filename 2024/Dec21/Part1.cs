namespace Dec21;

internal class Part1
{
    // Represents a keypad layout as a dictionary of keys and their coordinates
    private class Keypad : Dictionary<char, (int row, int col)>;
    
    // Represents all possible movement sequences between keys on a keypad
    private class KeypadMoves : Dictionary<(char from, char to), string[]>;

    // Numeric keypad layout with coordinates
    private static readonly Keypad NumericKeypad = new()
    {
        {'7', (0, 0)}, {'8', (0, 1)}, {'9', (0, 2)},
        {'4', (1, 0)}, {'5', (1, 1)}, {'6', (1, 2)},
        {'1', (2, 0)}, {'2', (2, 1)}, {'3', (2, 2)},
                       {'0', (3, 1)}, {'A', (3, 2)}
    };

    // Directional keypad layout with coordinates
    private static readonly Keypad DirectionalKeypad = new()
    {
                       {'^', (0, 1)}, {'A', (0, 2)},
        {'<', (1, 0)}, {'v', (1, 1)}, {'>', (1, 2)}
    };

    // Direction mappings to represent movements on the keypad
    private static readonly Dictionary<(int row, int col), string> Directions = new()
    {
        {(-1, 0), "^"}, // Up
        {(1, 0), "v"},  // Down
        {(0, -1), "<"}, // Left
        {(0, 1), ">"},  // Right
    };

    private static readonly KeypadMoves NumericKeypadMoves = GenerateAllMoves(NumericKeypad);
    private static readonly KeypadMoves DirectionalKeypadMoves = GenerateAllMoves(DirectionalKeypad);

    public static long Execute()
    {
        var input = File.ReadAllLines("input.txt");
        return CalculateRobotChainComplexity(input, 2);
    }

    // Calculates the total complexity for a chain of robots
    public static long CalculateRobotChainComplexity(string[] codes, int robotCount, bool showResults = false)
    {
        long totalComplexity = 0;

        List<KeypadMoves> robotAllowedMovements = [NumericKeypadMoves];

        // Define allowed movements for each robot in the chain
        for (var robotIdx = 1; robotIdx <= robotCount; robotIdx++)
            robotAllowedMovements.Add(DirectionalKeypadMoves);

        // Resolve the complexity for each code
        foreach (var code in codes)
        {
            Dictionary<(int robotIndex, string movements), long> movementCache = [];
            var complexity = ResolveRobotMovements(code, 0, robotCount, robotAllowedMovements, movementCache);

            var numericValue = int.Parse(new string(code.Where(char.IsDigit).ToArray()));
            totalComplexity += complexity * numericValue;

            if (showResults)
            {
                Console.WriteLine($"*** Code: {code}");
                Console.WriteLine($"{complexity} * {numericValue} = {complexity * numericValue}");
                Console.WriteLine();
            }
        }

        return totalComplexity;
    }

    // Recursively calculates the shortest path for the robots to process the given code
    private static long ResolveRobotMovements(
        string code, 
        int currentRobot, 
        int totalRobots, 
        List<KeypadMoves> robotAllowedMovements, 
        Dictionary<(int robotIndex, string movements), long> movementCache
        )
    {
        long shortestPath = long.MaxValue;

        // Check if the result for this robot and code combination is already cached
        if (movementCache.TryGetValue((currentRobot, code), out var cachedMovement))
        {
            return cachedMovement;
        }

        // If we are at the last robot in the chain, calculate the shortest path directly
        if (currentRobot == totalRobots)
        {
            // Get all possible movements for the code on this robot's keypad
            shortestPath = GetPossibleMovements(code, robotAllowedMovements[currentRobot])
                          .Select(m => m.Length)
                          .Min();

            // Cache the result for future lookups
            movementCache[(currentRobot, code)] = shortestPath;
            return shortestPath;
        }
                
        // Split the code at the first occurrence of 'A'. This is how we distribute tasks between robots.
        // 'A' acts as a logical delimiter, marking where the responsibility of one robot ends and the next begins.
        // This ensures each robot handles a manageable portion of the sequence.
        // This allows us to cache the results for each robot and code combination.
        var splitIndex = code.IndexOf('A');
        var firstSegment = code[..(splitIndex + 1)];
        var remainingSegment = code[(splitIndex + 1)..];

        // get the possible movements for the first part of the code
        var possibleMovements = GetPossibleMovements(firstSegment, robotAllowedMovements[currentRobot]);
        foreach (var possibleMovement in possibleMovements)
        {
            // Recursively solve for the next robot in the chain with the movement sequence
            var currentPathCount = ResolveRobotMovements(possibleMovement, currentRobot + 1, totalRobots, robotAllowedMovements, movementCache);
            shortestPath = Math.Min(shortestPath, currentPathCount);
        }

        // If there is a remaining portion, calculate its complexity for the current robot
        if (!string.IsNullOrEmpty(remainingSegment))
        {
            // Add the complexity of solving the remaining portion to the current shortest path
            shortestPath += ResolveRobotMovements(remainingSegment, currentRobot, totalRobots, robotAllowedMovements, movementCache);
        }

        // Cache the result for the current robot and code combination
        movementCache.Add((currentRobot, code), shortestPath);

        return shortestPath;
    }

    // Generates all possible movement paths for a given keypad
    private static KeypadMoves GenerateAllMoves(Keypad keypad)
    {
        KeypadMoves bestPaths = [];

        foreach (var startKey in keypad.Keys)
        {
            foreach (var targetKey in keypad.Keys)
            {
                if (startKey == targetKey)
                {
                    bestPaths[(startKey, targetKey)] = [""]; // Empty path for no move
                    continue;
                }

                var path = FindShortestPaths(keypad, startKey, targetKey);
                bestPaths[(startKey, targetKey)] = [.. path];
            }
        }

        return bestPaths;
    }

    // Finds the shortest paths between two keys using BFS
    private static List<string> FindShortestPaths(Keypad keypad, char fromKey, char toKey)
    {
        var startLocation = keypad[fromKey];
        var endLocation = keypad[toKey];

        var queue = new Queue<((int row, int col) position, List<string> path)>();
        var visited = new Dictionary<(int row, int col), int>();
        var shortestPaths = new List<string>();

        queue.Enqueue((startLocation, new List<string>()));
        visited[startLocation] = 0;

        int shortestPathLength = int.MaxValue;

        while (queue.Count > 0)
        {
            var (currentPos, path) = queue.Dequeue();

            // If we reached the target
            if (currentPos == endLocation)
            {
                if (path.Count < shortestPathLength)
                {
                    shortestPathLength = path.Count;
                    shortestPaths.Clear();
                }

                if (path.Count == shortestPathLength)
                {
                    shortestPaths.Add(string.Join("", path));
                }

                continue;
            }

            // Explore neighbors
            foreach (var (move, direction) in Directions)
            {
                var neighbor = (currentPos.row + move.row, currentPos.col + move.col);

                // Check if neighbor is valid
                if (keypad.ContainsValue(neighbor))
                {
                    int newPathLength = path.Count;

                    if (!visited.TryGetValue(neighbor, out int value) || value >= newPathLength)
                    {
                        value = newPathLength;
                        visited[neighbor] = value;
                        var newPath = new List<string>(path) { direction };
                        queue.Enqueue((neighbor, newPath));
                    }
                }
            }
        }

        return shortestPaths;
    }

    // Retrieves all possible movements for a given code
    private static List<string> GetPossibleMovements(string code, KeypadMoves allowedMoves)
    {
        List<string> possiblePaths = [""];
        char currentKey = 'A';

        foreach (var targetKey in code)
        {
            // Get the possible movements between the current key and the target key
            var moves = allowedMoves[(currentKey, targetKey)];
            List<string> newPossibleMovements = [];
            foreach (var prevMovements in possiblePaths)
            {                
                foreach (var newMove in moves)
                {
                    newPossibleMovements.Add(prevMovements + newMove + 'A');
                }
            }
            possiblePaths = newPossibleMovements;
            currentKey = targetKey;
        }

        return possiblePaths;
    }
}
