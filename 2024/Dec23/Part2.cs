namespace Dec23;

internal class Part2
{
    public static string Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        // Step 1: Build the graph
        var graph = new Dictionary<string, HashSet<string>>();
        foreach (var line in input)
        {
            var nodes = line.Split('-');
            if (!graph.ContainsKey(nodes[0]))
                graph[nodes[0]] = [];
            if (!graph.ContainsKey(nodes[1]))
                graph[nodes[1]] = [];

            graph[nodes[0]].Add(nodes[1]);
            graph[nodes[1]].Add(nodes[0]);
        }

        // Step 2: Find the largest clique
        var largestClique = FindLargestClique(graph);

        // Step 3: Generate the password
        return string.Join(",", largestClique.OrderBy(name => name));
    }

    private static List<string> FindLargestClique(Dictionary<string, HashSet<string>> graph)
    {
        var nodes = graph.Keys.ToList();
        var maxClique = new List<string>();

        // Helper function to check if a set of nodes is a clique
        bool IsClique(List<string> nodesToCheck)
        {
            foreach (var node in nodesToCheck)
            {
                foreach (var other in nodesToCheck)
                {
                    if (node != other && !graph[node].Contains(other))
                        return false;
                }
            }
            return true;
        }

        // Perform a depth-first search (DFS) to find all cliques
        void DFS(List<string> currentClique, List<string> remainingNodes)
        {
            if (remainingNodes.Count == 0)
            {
                if (currentClique.Count > maxClique.Count)
                    maxClique = new List<string>(currentClique);
                return;
            }

            for (int i = 0; i < remainingNodes.Count; i++)
            {
                var nextNode = remainingNodes[i];
                var newClique = new List<string>(currentClique) { nextNode };
                var newRemainingNodes = remainingNodes.Skip(i + 1)
                    .Where(n => graph[nextNode].Contains(n))
                    .ToList();

                if (IsClique(newClique))
                    DFS(newClique, newRemainingNodes);
            }
        }

        DFS([], nodes);
        return maxClique;
    }
}
