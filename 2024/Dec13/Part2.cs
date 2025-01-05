namespace Dec13;

internal class Part2
{
    public static long Execute()
    {
        var machines = ParseInput(@"input.txt");

        return FindMinimumTokensToWinPrizes(machines, (long)Math.Pow(10, 13));
    }

    public class Machine
    {
        public (int x, int y) ButtonA { get; set; }
        public (int x, int y) ButtonB { get; set; }
        public (long x, long y) Prize { get; set; }
    }

    public static List<Machine> ParseInput(string filePath)
    {
        var machines = new List<Machine>();
        var lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i += 4)
        {
            var buttonALine = lines[i];
            var buttonBLine = lines[i + 1];
            var prizeLine = lines[i + 2];

            var buttonA = ParseString(buttonALine);
            var buttonB = ParseString(buttonBLine);
            var prize = ParseString(prizeLine);

            machines.Add(new Machine { ButtonA = buttonA, ButtonB = buttonB, Prize = prize });
        }

        return machines;
    }

    internal static readonly char[] separator = ['X', 'Y', '+', ',', '='];

    private static (int x, int y) ParseString(string line)
    {
        var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        return (int.Parse(parts[1]), int.Parse(parts[3]));
    }

    private static long FindMinimumTokensToWinPrizes(List<Machine> machines, long shift = 0)
    {
        long totalTokens = 0;

        foreach (Machine machine in machines)
        {
            var px = shift + machine.Prize.x;
            var py = shift + machine.Prize.y;

            // Try solve the linear system of 2 equations where a and b are unknowns
            // a * ButtonA.X + b * ButtonB.X = TargetX
            // a * ButtonA.Y + b * ButtonB.Y = TargetY

            if (TrySolveLinearSystem(
                machine.ButtonA.x, machine.ButtonB.x, px,
                machine.ButtonA.y, machine.ButtonB.y, py,
                out double a, out double b))
            {
                long x = (long)a * machine.ButtonA.x + (long)b * machine.ButtonB.x;
                long y = (long)a * machine.ButtonA.y + (long)b * machine.ButtonB.y;

                if (x == px && y == py)
                {
                    long tokens = (long)a * 3 + (long)b;
                    totalTokens += tokens;
                }
            }
        }

        return totalTokens;
    }

    public static bool TrySolveLinearSystem(
        double ax, double bx, double px,
        double ay, double by, double py,
        out double a, out double b)
    {
        // Initialize output variables
        a = 0;
        b = 0;

        // Calculate the determinant
        double determinant = ax * by - ay * bx;

        // Check if determinant is zero
        if (Math.Abs(determinant) < 1e-10)
        {
            return false; // No unique solution
        }

        // Cramer's Rule
        a = (px * by - py * bx) / determinant;
        b = (ax * py - ay * px) / determinant;

        return true; // Solution found
    }
}
