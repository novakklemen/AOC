namespace Dec02;

internal class Part2
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        return input.Count(IsSafeWithDampener);
    }

    private static bool IsSafeWithDampener(string report)
    {
        var levels = report.Split(' ').Select(int.Parse).ToList();

        if (IsSafe(levels)) return true;

        for (int i = 0; i < levels.Count; i++)
        {
            var reducedLevels = levels.Take(i).Concat(levels.Skip(i + 1)).ToList();
            if (IsSafe(reducedLevels))
                return true;
        }

        return false;
    }

    private static bool IsSafe(List<int> levels)
    {
        bool isIncreasing = true, isDecreasing = true;

        for (int i = 1; i < levels.Count; i++)
        {
            int difference = levels[i] - levels[i - 1];

            if (Math.Abs(difference) < 1 || Math.Abs(difference) > 3)
                return false;

            if (difference < 0) isIncreasing = false;
            if (difference > 0) isDecreasing = false;
        }

        return isIncreasing || isDecreasing;
    }
}
