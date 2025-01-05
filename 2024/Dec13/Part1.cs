namespace Dec13;

internal class Part1
{
    public static long Execute()
    {
        var machines = ParseInput(@"input.txt");

        int maxPresses = 100;
        return FindMinimumTokensToWinPrizes(machines, maxPresses);
    }

    public class Machine
    {
        public (int x, int y) ButtonA { get; set; }
        public (int x, int y) ButtonB { get; set; }
        public (int x, int y) Prize { get; set; }
    }

    public static List<Machine> ParseInput(string filePath)
    {
        var machines = new List<Machine>();
        var lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i += 4)
        {
            var buttonALine = lines[i];
            var buttonBLine = lines[i + 1];
            var prizeLine = lines[i + 2];

            var buttonA = ParseString(buttonALine);
            var buttonB = ParseString(buttonBLine);
            var prize = ParseString(prizeLine);

            machines.Add(new Machine { ButtonA = buttonA, ButtonB = buttonB, Prize = prize });
        }

        return machines;
    }

    internal static readonly char[] separator = ['X', 'Y', '+', ',', '='];

    private static (int x, int y) ParseString(string line)
    {
        var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        return (int.Parse(parts[1]), int.Parse(parts[3]));
    }

    public static int FindMinimumTokensToWinPrizes(List<Machine> machines, int maxPresses)
    {
        int totalTokens = 0;

        foreach (var machine in machines)
        {
            int minTokens = int.MaxValue;
            bool canWin = false;

            for (int pressesA = 0; pressesA <= maxPresses; pressesA++)
            {
                for (int pressesB = 0; pressesB <= maxPresses; pressesB++)
                {
                    int x = pressesA * machine.ButtonA.x + pressesB * machine.ButtonB.x;
                    int y = pressesA * machine.ButtonA.y + pressesB * machine.ButtonB.y;

                    if (x == machine.Prize.x && y == machine.Prize.y)
                    {
                        int tokens = pressesA * 3 + pressesB;
                        minTokens = Math.Min(minTokens, tokens);
                        canWin = true;
                    }
                }
            }

            if (canWin)
                totalTokens += minTokens;
        }

        return totalTokens;
    }
}
