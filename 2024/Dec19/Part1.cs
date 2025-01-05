namespace Dec19;

internal class Part1
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
            if (IsPossible(design))
            {
                total++;
            }
        }

        return total;
    }

    private static readonly Dictionary<string, bool> cache = [];

    private static bool IsPossible(string design)
    {
        if (cache.TryGetValue(design, out bool value))
        {
            return value;
        }

        foreach (var pattern in patterns)
        {
            if (design == pattern)
            {
                cache[design] = true;
                return true;
            }

            if (design.StartsWith(pattern) && IsPossible(design[pattern.Length..]))
            {
                cache[design] = true;
                return true;
            }
        }

        cache[design] = false;
        return false;
    }
}
