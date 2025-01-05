namespace Dec14;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllLines(@"input.txt").ToList();

        int width = 101;
        int height = 103;
        int seconds = 100;

        var robots = ParseInput(input);

        Simulate(robots, width, height, seconds);

        return CalculateSafetyFactor(robots, width, height);
    }

    internal static readonly string[] separator = ["p=", " v=", ","];

    static List<Robot> ParseInput(List<string> input)
    {
        var robots = new List<Robot>();
        foreach (var line in input)
        {
            var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var position = (x: int.Parse(parts[0]), y: int.Parse(parts[1]));
            var velocity = (x: int.Parse(parts[2]), y: int.Parse(parts[3]));
            robots.Add(new Robot { Position = position, Velocity = velocity });
        }
        return robots;
    }

    static void Simulate(List<Robot> robots, int width, int height, int seconds)
    {
        foreach (var robot in robots)
        {
            var x = robot.Position.x;
            var y = robot.Position.y;

            x = (x + robot.Velocity.x * seconds) % width;
            y = (y + robot.Velocity.y * seconds) % height;

            // Handle wrapping for negative positions
            if (x < 0) x += width;
            if (y < 0) y += height;

            robot.Position = (x, y);
        }
    }

    static int CalculateSafetyFactor(List<Robot> robots, int width, int height)
    {
        int midX = width / 2;
        int midY = height / 2;

        int[] quadrants = new int[4]; // Q1, Q2, Q3, Q4

        foreach (var robot in robots)
        {
            if (robot.Position.x == midX || robot.Position.y == midY)
                continue; // Ignore robots on the middle lines

            if (robot.Position.x > midX && robot.Position.y < midY)
                quadrants[0]++; // Q1
            else if (robot.Position.x < midX && robot.Position.y < midY)
                quadrants[1]++; // Q2
            else if (robot.Position.x < midX && robot.Position.y > midY)
                quadrants[2]++; // Q3
            else if (robot.Position.x > midX && robot.Position.y > midY)
                quadrants[3]++; // Q4
        }

        return quadrants.Aggregate(1, (acc, val) => acc * val); // Multiply quadrant counts
    }

    class Robot
    {
        public (int x, int y) Position { get; set; }
        public (int x, int y) Velocity { get; set; }
    }
}
