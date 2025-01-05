namespace Dec17;

internal class Part2
{
    public const char REGISTER_A = 'A';
    public const char REGISTER_B = 'B';
    public const char REGISTER_C = 'C';

    public static ulong Execute()
    {
        var input = File.ReadAllLines("input.txt");         

        // Parse program instructions
        var program = input[4].Split(": ")[1].Split(',').Select(ulong.Parse).ToArray();

        ulong registerASeed = 0;

        // Reverse engineer to find the smallest valid seed
        for (int iteration = program.Length - 1; iteration >= 0; iteration--)
        {
            // Shift seed left by 3 bits, because we use below modulo 8 operation
            // This is equivalent to multiplying by 8, however, it is more efficient to use left shift
            registerASeed <<= 3;

            // Increment seed until the output matches the expected program slice
            // One we have larger registerA, the last slices of the program will be the same
            while (!AreProgramsEqual(SolveProgramWithSeed(program, registerASeed), program.AsSpan(iteration)))
            {
                registerASeed++;
            }
        }

        return registerASeed;
    }

    static bool AreProgramsEqual(List<ulong> actualProgram, ReadOnlySpan<ulong> expectedProgram)
    {
        if (actualProgram.Count != expectedProgram.Length)
            return false;

        for (int i = 0; i < actualProgram.Count; i++)
        {
            if (actualProgram[i] != expectedProgram[i])
                return false;
        }

        return true;
    }

    // Helper method to solve the program with a given seed
    private static List<ulong> SolveProgramWithSeed(ulong[] program, ulong seed)
    {
        return SolveProgram(program, new() { 
            { REGISTER_A, seed }, 
            { REGISTER_B, 0 }, 
            { REGISTER_C, 0 } 
        });
    }

    // Helper method to solve the program
    private static List<ulong> SolveProgram(ulong[] program, Dictionary<char, ulong> registers)
    {
        // Initialize program state
        int instructionPointer = 0;
        List<ulong> output = [];

        while (instructionPointer < program.Length)
        {
            var operationCode = program[instructionPointer];
            var operand = program[instructionPointer + 1];
            instructionPointer += 2;

            switch (operationCode)
            {
                case 0: // adv: Divide A by 2^operand (combo operand)
                    // original code
                    //registers[REGISTER_A] /= (ulong)Math.Pow(2, GetComboValue(operand, registers));
                    // more efficient to use right shift which does the same trick
                    registers[REGISTER_A] >>= (int)GetComboValue(operand, registers);
                    break;
                case 1: // bxl: XOR B with operand (literal)
                    registers[REGISTER_B] ^= operand;
                    break;
                case 2: // bst: Set B to combo operand % 8
                    //registers[REGISTER_B] = GetComboValue(operand, registers) % 8;
                    // more efficient to use bitwise AND which does the same trick
                    // 7 is the binary mask 0b0111, which corresponds to the lowest 3 bits of an integer.
                    // This is equivalent to taking the operand modulo 8.
                    registers[REGISTER_B] = GetComboValue(operand, registers) & 7;
                    break;
                case 3: // jnz: Jump if A != 0
                    if (registers[REGISTER_A] != 0)
                        instructionPointer = (int)operand;
                    break;

                case 4: // bxc: XOR B with C
                    registers[REGISTER_B] ^= registers[REGISTER_C];
                    break;
                case 5: // out: Output combo operand % 8
                    //output.Add(GetComboValue(operand, registers) % 8);
                    // more efficient to use bitwise AND which does the same trick
                    // 7 is the binary mask 0b0111, which corresponds to the lowest 3 bits of an integer.
                    // This is equivalent to taking the operand modulo 8.
                    output.Add(GetComboValue(operand, registers) & 7);
                    break;

                case 6: // bdv: Divide A by 2^operand, store in B
                    //registers[REGISTER_B] = registers[REGISTER_A] / (ulong)Math.Pow(2, GetComboValue(operand, registers));
                    // more efficient to use right shift which does the same trick
                    registers[REGISTER_B] = registers[REGISTER_A] >> (int)GetComboValue(operand, registers);
                    break;
                case 7: // cdv: Divide A by 2^operand, store in C
                    //registers[REGISTER_C] = registers[REGISTER_A] / (ulong)Math.Pow(2, GetComboValue(operand, registers));
                    // more efficient to use right shift which does the same trick
                    registers[REGISTER_C] = registers[REGISTER_A] >> (int)GetComboValue(operand, registers);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown operationCode: {operationCode}");
            }
        }

        return output;
    }

    // Helper method to get combo operand value
    private static ulong GetComboValue(ulong operand, Dictionary<char, ulong> registers)
    {
        return operand switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => registers[REGISTER_A],
            5 => registers[REGISTER_B],
            6 => registers[REGISTER_C],
            _ => throw new InvalidOperationException($"Invalid combo operand: {operand}")
        };
    }
}
