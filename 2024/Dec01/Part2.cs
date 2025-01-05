namespace Dec01;

internal class Part2
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt");

        var list1 = new List<int>();
        var list2 = new List<int>();

        foreach (var line in input)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            list1.Add(int.Parse(parts[0]));
            list2.Add(int.Parse(parts[1]));
        }

        list1.Sort();
        list2.Sort();

        return CalculateSimilarityScore(list1, list2);
    }

    private static int CalculateSimilarityScore(List<int> leftList, List<int> rightList)
    {
        // Count occurrences in the right list
        var rightCounts = rightList
            .GroupBy(x => x)
            .ToDictionary(g => g.Key, g => g.Count());

        int similarityScore = 0;

        // Calculate similarity score based on left list
        foreach (var number in leftList)
        {
            if (rightCounts.TryGetValue(number, out int count))
            {
                similarityScore += number * count;
            }
        }

        return similarityScore;
    }
}
