namespace Dec02;

internal class Part1
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt").ToList();

        return input.Count(IsSafeReport);
    }

    private static bool IsSafeReport(string report)
    {
        var levels = report.Split(' ').Select(int.Parse).ToList();

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
