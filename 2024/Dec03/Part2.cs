using System.Text.RegularExpressions;

namespace Dec03;

internal class Part2
{
    private static readonly Regex _regex = new("mul\\(\\d{1,3},\\d{1,3}\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex _instructionGroups = new("do\\(\\)|don't\\(\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt").ToList();
        var allLines = string.Join("", input);

        var instructionGroups = _instructionGroups.Matches(allLines);

        var result = FindInstructions(allLines, 0, instructionGroups[0].Index);

        for (var idx = 0; idx < instructionGroups.Count; idx++)
        {
            var startIndex = instructionGroups[idx].Index;
            var endIndex = idx < instructionGroups.Count-1 ? instructionGroups[idx+1].Index : allLines.Length-1;

            if (!instructionGroups[idx].Value.Equals("do()"))
                continue;

            result += FindInstructions(allLines, startIndex, endIndex);
        }

        return result;
    }

    private static int FindInstructions(string instructions, int startIndex, int endIndex)
    {
        var textRange = instructions.Substring(startIndex, endIndex - startIndex);

        MatchCollection matches = _regex.Matches(textRange);

        var result = 0;

        foreach (Match match in matches)
        {
            var numbers = match.Value.Replace("mul(", "").Replace(")", "").Split(',', StringSplitOptions.RemoveEmptyEntries);
            result += int.Parse(numbers[0]) * int.Parse(numbers[1]);
        }

        return result;
    }
}
