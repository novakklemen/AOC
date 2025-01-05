namespace Dec23;

internal class Part1
{
    public static long Execute()
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

        // Step 2: Find all triangles
        var triangles = new HashSet<HashSet<string>>(new HashSetEqualityComparer());
        foreach (var node in graph.Keys)
        {
            foreach (var neighbor in graph[node])
            {
                foreach (var mutual in graph[node].Intersect(graph[neighbor]))
                {
                    var triangle = new HashSet<string> { node, neighbor, mutual };
                    triangles.Add(triangle);
                }
            }
        }

        // Step 3: Filter triangles where at least one name starts with 't'
        var filteredTriangles = triangles
            .Where(triangle => triangle.Any(name => name.StartsWith('t')))
            .ToList();

        // Step 4: Return the count
        return filteredTriangles.Count;
    }
}

internal class HashSetEqualityComparer : IEqualityComparer<HashSet<string>>
{
    public bool Equals(HashSet<string>? x, HashSet<string>? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;

        return x.SetEquals(y);
    }

    public int GetHashCode(HashSet<string> obj)
    {
        int hash = 17;
        foreach (var item in obj.OrderBy(e => e))
            hash = hash * 23 + item.GetHashCode();
        return hash;
    }
}
