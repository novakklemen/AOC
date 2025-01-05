namespace Dec19;

internal class Part2
{
    private static readonly List<string> patterns = [];

    public static long Execute()
    {
        var designs = new List<string>();

        var input = File.ReadAllLines("input.txt");
        bool foundBlankLine = false;

        foreach (var line in input)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine))
            {
                foundBlankLine = true;
                continue;
            }

            if (!foundBlankLine)
            {
                patterns.AddRange(trimmedLine.Split(", ").Select(p => p.Trim()));
            }
            else
            {
                designs.Add(trimmedLine);
            }
        }

        long total = 0;
        foreach (var design in designs)
        {
            total += WaysPossible(design);
        }

        return total;
    }

    private static readonly Dictionary<string, long> cache = [];

    private static long WaysPossible(string design)
    {
        if (cache.TryGetValue(design, out long value))
        {
            return value;
        }

        long ways = 0;

        foreach (var pattern in patterns)
        {
            if (design == pattern)
            {
                ways++;
            }
            else if (design.StartsWith(pattern))
            {
                ways += WaysPossible(design[pattern.Length..]);
            }
        }

        cache[design] = ways;
        return ways;
    }
}
