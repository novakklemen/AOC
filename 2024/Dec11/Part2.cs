namespace Dec11;

internal class Part2
{
    internal static readonly char[] separator = [' ', '\n', '\r'];

    public static long Execute()
    {
        var input = File.ReadAllText(@"input.txt");
        List<string> stones = [.. input.Split(separator, StringSplitOptions.RemoveEmptyEntries)];

        return SimulateBlinksOptimized(stones, 75);
    }

    private static long SimulateBlinksOptimized(List<string> stones, int blinks)
    {
        // Use a dictionary to store stone values and their counts
        Dictionary<string, long> stoneCounts = stones.GroupBy(s => s).ToDictionary(g => g.Key, g => (long)g.Count());

        for (int i = 0; i < blinks; i++)
        {
            Dictionary<string, long> nextStoneCounts = [];

            foreach (var kvp in stoneCounts)
            {
                string stone = kvp.Key;
                long count = kvp.Value;

                if (stone == "0")
                {
                    // Rule 1: Replace with "1"
                    AddToDictionary(nextStoneCounts, "1", count);
                }
                else if (stone.Length % 2 == 0)
                {
                    // Rule 2: Split into two stones
                    int mid = stone.Length / 2;
                    string left = stone[..mid].TrimStart('0');
                    string right = stone[mid..].TrimStart('0');
                    AddToDictionary(nextStoneCounts, string.IsNullOrEmpty(left) ? "0" : left, count);
                    AddToDictionary(nextStoneCounts, string.IsNullOrEmpty(right) ? "0" : right, count);
                }
                else
                {
                    // Rule 3: Multiply by 2024
                    long newStoneValue = long.Parse(stone) * 2024;
                    AddToDictionary(nextStoneCounts, newStoneValue.ToString(), count);
                }
            }

            stoneCounts = nextStoneCounts;
        }

        // Sum up all stone counts
        return stoneCounts.Values.Sum();
    }

    private static void AddToDictionary(Dictionary<string, long> dict, string key, long value)
    {
        if (dict.ContainsKey(key))
            dict[key] += value;
        else
             dict[key] = value;
    }
}
