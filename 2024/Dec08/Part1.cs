namespace Dec08;

internal class Part1
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");
        return CountUniqueAntinodes(input);
    }

    private static int CountUniqueAntinodes(string[] map)
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

        for (int i = 0; i < antennas.Count; i++)
        {
            for (int j = i + 1; j < antennas.Count; j++)
            {
                var a1 = antennas[i];
                var a2 = antennas[j];

                // Same frequency?
                if (a1.frequency == a2.frequency)
                {
                    // Calculate potential antinodes
                    int dx = a2.x - a1.x;
                    int dy = a2.y - a1.y;

                    // Antinode 1
                    var antinode1 = (x: a1.x - dx, y: a1.y - dy);
                    if (IsValidAntinode(antinode1, rows, cols)) antinodes.Add(antinode1);

                    // Antinode 2
                    var antinode2 = (x: a2.x + dx, y: a2.y + dy);
                    if (IsValidAntinode(antinode2, rows, cols)) antinodes.Add(antinode2);
                }
            }
        }

        // Step 3: Count unique antinodes
        return antinodes.Count;
    }

    private static bool IsValidAntinode((int x, int y) point, int rows, int cols)
    {
        return point.x >= 0 && point.x < cols && point.y >= 0 && point.y < rows;
    }
}
