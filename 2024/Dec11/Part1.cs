namespace Dec11;

internal class Part1
{
    internal static readonly char[] separator = [' ', '\n', '\r'];

    public static long Execute()
    {
        var input = File.ReadAllText(@"input.txt");
        List<string> stones = [.. input.Split(separator, StringSplitOptions.RemoveEmptyEntries)];

        int blinks = 25; // Number of blinks

        return SimulateBlinks(stones, blinks);
    }

    // initial implementation
    private static long SimulateBlinks(List<string> stones, int blinks)
    {
        for (int i = 0; i < blinks; i++)
        {
            List<string> nextStones = [];

            foreach (string stone in stones)
            {
                if (stone == "0")
                {
                    // Rule 1: Stone becomes "1"
                    nextStones.Add("1");
                }
                else if (stone.Length % 2 == 0)
                {
                    // Rule 2: Split the stone into two stones
                    int mid = stone.Length / 2;
                    string left = stone[..mid].TrimStart('0');
                    string right = stone[mid..].TrimStart('0');
                    nextStones.Add(string.IsNullOrEmpty(left) ? "0" : left);
                    nextStones.Add(string.IsNullOrEmpty(right) ? "0" : right);
                }
                else
                {
                    // Rule 3: Multiply the stone by 2024
                    long stoneValue = long.Parse(stone);
                    nextStones.Add((stoneValue * 2024).ToString());
                }
            }

            stones = nextStones;
        }

        return stones.Count;
    }
}
