namespace Dec05;

internal class Part2
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

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
                // Parse rules into a dictionary for quick lookups
                var parts = line.Split('|').Select(int.Parse).ToArray();
                rules.Add((parts[0], parts[1]));
            }
            else
            {
                // Parse updates
                var update = line.Split(',').Select(int.Parse).ToList();
                updates.Add(update);
            }
        }


        int sumOfMiddlePagesIncorrectlyOrdered = 0;

        // Process each update
        foreach (var update in updates)
        {
            if (!IsUpdateInOrder(update, rules))
            {
                // Fix the update order and calculate the middle page
                var correctedUpdate = FixUpdateOrder(update, rules);
                int middleIndex = correctedUpdate.Count / 2;
                sumOfMiddlePagesIncorrectlyOrdered += correctedUpdate[middleIndex];
            }
        }

        return sumOfMiddlePagesIncorrectlyOrdered;
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

    static List<int> FixUpdateOrder(List<int> update, HashSet<(int, int)> rules)
    {
        // Use a topological sort to reorder the pages
        var dependencies = new Dictionary<int, List<int>>();
        foreach (var page in update)
        {
            dependencies[page] = [];
        }

        foreach (var (x, y) in rules)
        {
            if (dependencies.TryGetValue(x, out List<int>? xDependencies) && dependencies.ContainsKey(y))
            {
                xDependencies.Add(y);
            }
        }

        HashSet<int> visited = [];
        Stack<int> result = new();

        void TopologicalSort(int node)
        {
            if (visited.Contains(node)) return;
            visited.Add(node);

            foreach (var neighbor in dependencies[node])
            {
                TopologicalSort(neighbor);
            }

            result.Push(node);
        }

        foreach (var page in update)
        {
            if (!visited.Contains(page))
            {
                TopologicalSort(page);
            }
        }

        return [.. result];
    }
}
