namespace Dec24;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllLines("input.txt");
        // Parse the input into initial wire values and circuit gate definitions
        var (wireValues, circuit) = Parse(input);

        // Identify all output labels starting with 'z'
        var outputLabels = circuit.Keys.Where(label => label.StartsWith('z'));

        var result = 0L;

        // Compute the binary number represented by the 'z' outputs
        foreach (var label in outputLabels.OrderByDescending(label => label))
        {
            result = result * 2 + Evaluate(label, circuit, wireValues);
        }
        return result;
    }

    private static int Evaluate(string label, Dictionary<string, Gate> circuit, Dictionary<string, int> wireValues)
    {
        // Return the value if it exists in the wire values dictionary
        if (wireValues.TryGetValue(label, out var result))
        {
            return result;
        }

        // Otherwise, evaluate the gate connected to the label
        return circuit[label] switch
        {
            Gate(var inputValue1, "AND", var inputValue2) => Evaluate(inputValue1, circuit, wireValues) & Evaluate(inputValue2, circuit, wireValues),
            Gate(var inputValue1, "OR", var inputValue2) => Evaluate(inputValue1, circuit, wireValues) | Evaluate(inputValue2, circuit, wireValues),
            Gate(var inputValue1, "XOR", var inputValue2) => Evaluate(inputValue1, circuit, wireValues) ^ Evaluate(inputValue2, circuit, wireValues),
            _ => throw new Exception(circuit[label].ToString()),
        };
    }

    private static (Dictionary<string, int> wireValues, Dictionary<string, Gate> circuit) Parse(string[] input)
    {
        var wireValues = new Dictionary<string, int>();
        var circuit = new Dictionary<string, Gate>();

        foreach (var line in input)
        {
            if (line.Contains(':'))
            {
                // Parse initial wire values
                var parts = line.Split(": ");
                wireValues.Add(parts[0], int.Parse(parts[1]));
            }
            else if (line.Contains("->"))
            {
                // Parse gate definitions
                var parts = line.Split([' ', '-', '>'], StringSplitOptions.RemoveEmptyEntries);
                circuit.Add(parts[3], new Gate(parts[0], parts[1], parts[2]));
            }
        }

        return (wireValues, circuit);
    }
}
