namespace Dec22;

internal class Part2
{
    public static long Execute()
    {
        var input = File.ReadAllLines("input.txt");

        // Parse the initial secret numbers from the input
        var initialSecrets = input.Select(long.Parse).ToList();

        // Dictionary to store patterns and their total points
        Dictionary<string, long> patternPoints = [];

        foreach (var secret in initialSecrets)
        {
            var (sequences, differences) = CalculatePrices(secret, 2000);

            HashSet<string> seenPatterns = [];

            // we are taking always 4 prices
            for (int i = 0; i < differences.Count - 3; i++)
            {
                // we are counting the bananas to 4 sequences
                var pattern = string.Join(",", differences.Skip(i).Take(4));

                // for each pattern we need to remember how many bananas we will get
                // and skip the ones already processed
                if (seenPatterns.Add(pattern))
                {
                    if (!patternPoints.ContainsKey(pattern))
                    {
                        patternPoints[pattern] = 0;
                    }

                    // this is the number of bananas based on the sequence
                    patternPoints[pattern] += sequences[i+3] % 10;
                }
            }
        }

        // get the pattern with the max bananas
        var maxBananas = patternPoints.Values.Max();
        return maxBananas;
    }

    private static (List<long> Sequences, List<long> Differences) CalculatePrices(long secret, long numberOfRounds)
    {
        List<long> sequences = [];
        List<long> differences = [];

        for (int i = 0; i < numberOfRounds; i++)
        {
            // Calculate the next secret
            var nextSecret = EvolveSecret(secret);
            // The price for the current secret
            var currentPrice = secret % 10;
            // The price for the next secret
            var newPrice = nextSecret % 10;
            // Remember the price difference
            differences.Add(newPrice - currentPrice);
            // Remember the calculated secret for later
            sequences.Add(secret = nextSecret);
        }

        return (sequences, differences);
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
