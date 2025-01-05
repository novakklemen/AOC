namespace Dec05;

internal class Part1
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        // Parse rules into a dictionary for quick lookups
        HashSet<(int, int)> rules = [];
        List<List<int>> updates = [];
        var readingRules = true;
        foreach (var line in input)
        {
            if (line == "")
            {
                readingRules = false;
                continue;
            }

            if (readingRules)
            {
                var parts = line.Split('|').Select(int.Parse).ToArray();
                rules.Add((parts[0], parts[1]));
            } 
            else
            {
                var update = line.Split(',').Select(int.Parse).ToList();
                updates.Add(update);
            }
        }


        int sumOfMiddlePages = 0;

        // Process each update
        foreach (var update in updates)
        {
            if (IsUpdateInOrder(update, rules))
            {
                // Find the middle page of the correctly ordered update
                int middleIndex = update.Count / 2;
                sumOfMiddlePages += update[middleIndex];
            }
        }

        return sumOfMiddlePages;
    }
    static bool IsUpdateInOrder(List<int> update, HashSet<(int, int)> rules)
    {
        // Create a dictionary to map page numbers to their positions in the update
        var positionMap = update.Select((value, index) => (value, index))
                                .ToDictionary(pair => pair.value, pair => pair.index);

        // Validate all rules that apply to pages present in the update
        foreach (var (x, y) in rules)
        {
            if (positionMap.ContainsKey(x) && positionMap.ContainsKey(y))
            {
                if (positionMap[x] >= positionMap[y])
                {
                    return false; // Rule is violated
                }
            }
        }

        return true; // All applicable rules are satisfied
    }
}
