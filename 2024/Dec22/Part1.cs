namespace Dec22;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllLines("input.txt");

        // Parse the initial secret numbers from the input
        var initialSecrets = input.Select(long.Parse).ToList();

        long total = 0;

        foreach (var secret in initialSecrets)
        {
            long current = secret;

            // Generate 2000 secret numbers
            for (int i = 0; i < 2000; i++)
            {
                current = EvolveSecret(current);
            }

            // Add the 2000th secret number to the total
            total += current;
        }

        return total;
    }

    private static long EvolveSecret(long secret)
    {
        // Step 1: Multiply by 64, mix, and prune
        secret = Prune(secret ^ (secret * 64));

        // Step 2: Divide by 32, round down, mix, and prune
        secret = Prune(secret ^ (secret / 32));

        // Step 3: Multiply by 2048, mix, and prune
        secret = Prune(secret ^ (secret * 2048));

        return secret;
    }

    private static long Prune(long secret)
    {
        // Prune the secret number to modulo 16777216
        return secret % 16777216;
    }
}
