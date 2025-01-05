namespace Dec24;

internal class Part2
{
    public static string Execute()
    {
        var input = File.ReadAllLines("input.txt");
        var circuit = Parse(input).circuit;
        // Executes the process of identifying swapped gates and returning the sorted labels of swapped wires.
        return string.Join(",", FixCircuit(circuit).OrderBy(label => label));
    }

    // Fixes the circuit by identifying and swapping incorrect gate connections.
    private static IEnumerable<string> FixCircuit(Dictionary<string, Gate> circuit)
    {
        // The carry input is the output of the first AND gate in the circuit.
        var carryInput = FindOutput(circuit, "x00", "AND", "y00");

        // This loop iterates through all bit positions, from 1 (the second least significant bit) to 44 (the most significant bit).
        for (var bitPosition = 1; bitPosition < 45; bitPosition++)
        {
            var x = $"x{bitPosition:D2}";
            var y = $"y{bitPosition:D2}";
            var z = $"z{bitPosition:D2}";

            var xor1Output = FindOutput(circuit, x, "XOR", y);
            var and1Output = FindOutput(circuit, x, "AND", y);
            var xor2Output = FindOutput(circuit, carryInput, "XOR", xor1Output);
            var and2Output = FindOutput(circuit, carryInput, "AND", xor1Output);

            if (xor2Output == null && and2Output == null)
            {
                return SwapAndFix(circuit, xor1Output, and1Output);
            }

            var carryOutput = FindOutput(circuit, and1Output, "OR", and2Output);

            if (xor2Output != null && xor2Output != z)
            {
                return SwapAndFix(circuit, z, xor2Output);
            }
            else
            {
                carryInput = carryOutput;
            }
        }

        return [];
    }

    // Swaps two outputs in the circuit and recursively fixes the circuit.
    private static IEnumerable<string> SwapAndFix(Dictionary<string, Gate> circuit, string output1, string output2)
    {
        (circuit[output1], circuit[output2]) = (circuit[output2], circuit[output1]);
        return FixCircuit(circuit).Concat([output1, output2]);
    }

    // Finds the output wire label for a given gate type and input wires.
    private static string FindOutput(Dictionary<string, Gate> circuit, string x, string gateType, string y)
    {
        return circuit.SingleOrDefault(pair =>
            (pair.Value.Input1 == x && pair.Value.GateType == gateType && pair.Value.Input2 == y) ||
            (pair.Value.Input1 == y && pair.Value.GateType == gateType && pair.Value.Input2 == x)).Key;
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
