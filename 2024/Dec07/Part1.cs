namespace Dec07;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        long totalSum = 0;

        foreach (var line in input)
        {
            // Parse input line
            var parts = line.Split(':');
            long target = long.Parse(parts[0].Trim());
            var numbers = parts[1]
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            // Check if we can form the target by inserting + or *
            if (CanProduceTarget(target, numbers))
            {
                totalSum += target;
            }
        }

        return totalSum;
    }

    private static bool CanProduceTarget(long target, int[] numbers)
    {
        // If there's only one number, just check it directly.
        if (numbers.Length == 1)
            return numbers[0] == target;

        // Use DFS (Depth-First Search) to try all combinations of operations
        return DFS(numbers, 1, numbers[0], target);
    }

    /// <summary>
    /// Recursively tries both '+' and '*' at the current index to see if it can reach the target.
    /// Operators are evaluated left-to-right in the given sequence.
    /// </summary>
    private static bool DFS(int[] numbers, int index, long currentValue, long target)
    {
        // If we've used all numbers, check if the result equals the target
        if (index == numbers.Length)
            return currentValue == target;

        long nextNum = numbers[index];

        // Try '+'
        if (DFS(numbers, index + 1, currentValue + nextNum, target))
            return true;

        // Try '*'
        if (DFS(numbers, index + 1, currentValue * nextNum, target))
            return true;

        return false;
    }
}
