namespace Dec01;

internal class Part1
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

        var totalDistance = 0;
        for (var idx = 0; idx < list1.Count; idx++)
        {
            var distance = list2[idx] >= list1[idx] ? list2[idx] - list1[idx] : list1[idx] - list2[idx];
            totalDistance += distance;
        }

        return totalDistance;
    }
}
