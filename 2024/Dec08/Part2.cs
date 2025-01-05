namespace Dec08;

internal class Part2
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");
        return CountUniqueAntinodes(input);
    }

    static int CountUniqueAntinodes(string[] map)
    {
        int rows = map.Length;
        int cols = map[0].Length;

        // Step 1: Parse the input to extract antenna positions and frequencies
        List<(int x, int y, char frequency)> antennas = [];
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                char c = map[y][x];
                if (char.IsLetterOrDigit(c))
                {
                    antennas.Add((x, y, c));
                }
            }
        }

        // Step 2: Find antinodes
        HashSet<(int x, int y)> antinodes = [];

        // Group antennas by frequency
        Dictionary<char, List<(int x, int y)>> frequencyGroups = [];
        foreach (var antenna in antennas)
        {
            if (!frequencyGroups.ContainsKey(antenna.frequency))
                frequencyGroups[antenna.frequency] = [];
            frequencyGroups[antenna.frequency].Add((antenna.x, antenna.y));
        }

        // Calculate antinodes for each frequency group
        foreach (var group in frequencyGroups.Values)
        {
            int count = group.Count;
            for (int i = 0; i < count; i++)
            {
                var a1 = group[i];

                // Add the position of the antenna itself as an antinode
                antinodes.Add((a1.x, a1.y));

                for (int j = i + 1; j < count; j++)
                {
                    var a2 = group[j];

                    // Check all positions exactly in line with a1 and a2
                    int dx = a2.x - a1.x;
                    int dy = a2.y - a1.y;

                    // Extend in both directions along the line
                    for (int k = 1; ; k++)
                    {
                        var antinode1 = (x: a1.x + k * dx, y: a1.y + k * dy);
                        var antinode2 = (x: a1.x - k * dx, y: a1.y - k * dy);

                        if (IsValidPosition(antinode1, rows, cols)) antinodes.Add(antinode1);
                        if (IsValidPosition(antinode2, rows, cols)) antinodes.Add(antinode2);

                        // Stop when moving beyond the bounds
                        if (!IsValidPosition(antinode1, rows, cols) && !IsValidPosition(antinode2, rows, cols))
                            break;
                    }
                }
            }
        }

        // Step 3: Count unique antinodes
        return antinodes.Count;
    }

    static bool IsValidPosition((int x, int y) point, int rows, int cols)
    {
        return point.x >= 0 && point.x < cols && point.y >= 0 && point.y < rows;
    }
}
