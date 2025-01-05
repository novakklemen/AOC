namespace Dec07;

internal class Part2
{
    internal static readonly char[] separator = [' '];

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
                .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

            // Check if we can form the target by inserting '+', '*', or '||'
            if (CanProduceTarget(target, numbers))
            {
                totalSum += target;
            }
        }

        return totalSum;
    }

    private static bool CanProduceTarget(long target, long[] numbers)
    {
        // If there's only one number, just check it directly.
        if (numbers.Length == 1)
            return numbers[0] == target;

        // Use DFS (Depth-First Search) to try all combinations of operations
        return DFS(numbers, 0, numbers[0], target);
    }

    private static bool DFS(long[] numbers, int index, long current, long target)
    {
        // if we've reached the end, check if current equals target
        if (index == numbers.Length - 1)
        {
            return current == target;
        }

        // Get the next number
        long nextNumber = numbers[index + 1];

        // Recursive calls for each operation
        // Addition
        if (DFS(numbers, index + 1, current + nextNumber, target))
            return true;

        // Multiplication
        if (DFS(numbers, index + 1, current * nextNumber, target))
            return true;

        // Concatenation
        if (DFS(numbers, index + 1, long.Parse(current.ToString() + nextNumber.ToString()), target))
            return true;

        // If none of the operations work, return false
        return false;
    }
}
