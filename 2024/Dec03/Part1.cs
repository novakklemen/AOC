using System.Text.RegularExpressions;

namespace Dec03;

internal class Part1
{
    private static readonly Regex _regex = new("mul\\(\\d{1,3},\\d{1,3}\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt").ToList();

        var result = 0;

        foreach (var line in input)
        {
            MatchCollection matches = _regex.Matches(line);

            foreach (Match match in matches)
            {
                var numbers = match.Value.Replace("mul(", "").Replace(")", "").Split(',', StringSplitOptions.RemoveEmptyEntries);
                result += int.Parse(numbers[0]) * int.Parse(numbers[1]);
            }
        }

        return result;
    }

    
}
