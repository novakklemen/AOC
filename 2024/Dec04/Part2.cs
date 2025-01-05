namespace Dec04;

internal class Part2
{
    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt").ToList();

        char[,] grid = new char[input.Count, input[0].Length];

        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                grid[row, col] = input[row][col];
            }
        }

        return CountXMasPatterns(grid);
    }

    private static int CountXMasPatterns(char[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int count = 0;

        for (int row = 1; row < rows-1; row++)
        {
            for (int col = 1; col < cols-1; col++)
            {
                //count += CountPatternsAt(grid, row, col);
                if (IsXMasPatternAt(grid, row, col))
                {
                    count++;
                }
            }
        }

        return count;
    }

    static bool IsXMasPatternAt(char[,] grid, int row, int col)
    {
        const string word = "MAS";
        const string reverseWord = "SAM";

        char char100 = grid[row-1, col-1];
        char char001 = grid[row - 1, col + 1];
        char char020 = grid[row, col];
        char char300 = grid[row+1, col-1];
        char char003 = grid[row + 1, col + 1];

        var pattern1 = word.Equals($"{char100}{char020}{char003}") && word.Equals($"{char003}{char020}{char300}");
        var pattern2 = word.Equals($"{char001}{char020}{char300}") && word.Equals($"{char003}{char020}{char100}");
        var pattern3 = word.Equals($"{char003}{char020}{char100}") && word.Equals($"{char300}{char020}{char001}");
        var pattern4 = word.Equals($"{char300}{char020}{char001}") && word.Equals($"{char100}{char020}{char003}");

        var pattern5 = reverseWord.Equals($"{char100}{char020}{char003}") && reverseWord.Equals($"{char003}{char020}{char300}");
        var pattern6 = reverseWord.Equals($"{char001}{char020}{char300}") && reverseWord.Equals($"{char003}{char020}{char100}");
        var pattern7 = reverseWord.Equals($"{char003}{char020}{char100}") && reverseWord.Equals($"{char300}{char020}{char001}");
        var pattern8 = reverseWord.Equals($"{char300}{char020}{char001}") && reverseWord.Equals($"{char100}{char020}{char003}");

        return pattern1 || pattern2 || pattern3 || pattern4 || pattern5 || pattern6 || pattern7 || pattern8;
    }
}
