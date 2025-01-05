namespace Dec25;

internal class Part2
{
    public static string Execute()
    {
        Console.WriteLine();
        DrawTree(15);
        DrawTrunk(15);
        Console.WriteLine();

        return "Marry Christmas!";
    }

    static void DrawTree(int height)
    {
        Random rand = new();

        for (int i = 0; i < height; i++)
        {
            // Draw spaces for alignment
            for (int j = 0; j < height - i - 1; j++)
            {
                Console.Write(" ");
            }

            // Draw tree layer
            for (int j = 0; j < (2 * i) + 1; j++)
            {
                // Randomly color decorations
                if (rand.Next(4) == 0) // 1 in 4 chance of decoration
                {
                    Console.ForegroundColor = (ConsoleColor)rand.Next(9, 15); // Bright colors
                    Console.Write("o");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("*");
                }
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }

    static void DrawTrunk(int height)
    {
        int trunkWidth = 3;
        int trunkHeight = 2;

        for (int i = 0; i < trunkHeight; i++)
        {
            for (int j = 0; j < height - trunkWidth / 2 - 1; j++)
            {
                Console.Write(" ");
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int j = 0; j < trunkWidth; j++)
            {
                Console.Write("|");
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
