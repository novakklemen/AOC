namespace Dec04;

internal class Part1
{
    private const string targetWord = "XMAS";

    public static int Execute()
    {
        var input = File.ReadAllLines(@"input.txt").ToList();

        char[,] grid = new char[input.Count, input[0].Length];

        for(var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                grid[row, col] = input[row][col];
            }
        }

        return CountOccurrences(grid, targetWord);
    }

    private static int CountOccurrences(char[,] grid, string word)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int count = 0;

        int[,] directions =
        {
            { 0, 1 },   // Right
            { 1, 0 },   // Down
            { 1, 1 },   // Diagonal Down-Right
            { 1, -1 },  // Diagonal Down-Left
            { 0, -1 },  // Left
            { -1, 0 },  // Up
            { -1, -1 }, // Diagonal Up-Left
            { -1, 1 }   // Diagonal Up-Right
        };           

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                for(int idxDirection = 0; idxDirection < directions.GetLength(0); idxDirection++)
                {
                    int dx = directions[idxDirection, 0];
                    int dy = directions[idxDirection, 1];
                    if (IsWordAt(grid, word, row, col, dx, dy))
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    private static bool IsWordAt(char[,] grid, string word, int startRow, int startCol, int rowOffset, int colOffset)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int wordLength = word.Length;

        for (int i = 0; i < wordLength; i++)
        {
            int newRow = startRow + i * rowOffset;
            int newCol = startCol + i * colOffset;

            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                return false;

            if (grid[newRow, newCol] != word[i])
                return false;
        }

        return true;
    }
}
