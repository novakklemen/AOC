namespace Dec21;

internal class Part2
{
    public static long Execute()
    {
        var input = File.ReadAllLines("input.txt");
        return Part1.CalculateRobotChainComplexity(input, 25);
    }
}
