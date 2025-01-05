namespace Dec09;

internal class Part2
{
    public static long Execute()
    {
        var input = File.ReadAllText(@"input.txt").Trim();
        
        // Parse the disk map into a disk representation
        List<int> disk = ParseDisk(input);

        // Compact the disk using the new method
        CompactDisk(disk);

        // Calculate the checksum
        return CalculateChecksum(disk);
    }

    private static List<int> ParseDisk(string diskMap)
    {
        var disk = new List<int>(diskMap.Length);
        int fileId = 0;

        // Parse pairs of file size and free space
        for (int i = 0; i < diskMap.Length; i += 2)
        {
            int fileSize = diskMap[i] - '0';

            // Add file blocks
            disk.AddRange(Enumerable.Repeat(fileId, fileSize));

            // Add free space blocks
            if (i + 1 < diskMap.Length)
            {
                int freeSpace = diskMap[i + 1] - '0';
                disk.AddRange(Enumerable.Repeat(-1, freeSpace));
            }

            if (fileSize > 0)
                fileId++;
        }

        return disk;
    }

    private static void CompactDisk(List<int> disk)
    {
        // Identify the start position and size of each file
        var filePositions = new Dictionary<int, (int Start, int Size)>();
        int i = 0;
        while (i < disk.Count)
        {
            int value = disk[i];
            if (value != -1)
            {
                int start = i;
                int size = 1;
                i++;
                while (i < disk.Count && disk[i] == value)
                {
                    size++;
                    i++;
                }
                filePositions[value] = (start, size);
            }
            else
            {
                i++;
            }
        }

        // Identify groups of free space
        var freeSpaces = new List<(int Start, int Size)>();
        i = 0;
        while (i < disk.Count)
        {
            if (disk[i] == -1)
            {
                int start = i;
                int size = 1;
                i++;
                while (i < disk.Count && disk[i] == -1)
                {
                    size++;
                    i++;
                }
                freeSpaces.Add((start, size));
            }
            else
            {
                i++;
            }
        }

        // Process files in reverse order of their ID
        var fileIds = filePositions.Keys.ToList();
        fileIds.Sort((a, b) => b.CompareTo(a)); // Sort in descending order

        foreach (var fileId in fileIds)
        {
            var file = filePositions[fileId];
            int fileSize = file.Size;
            int currentStart = file.Start;

            // Find the leftmost span of free space that can fit the file and is to the left
            var targetSpace = freeSpaces
                .Where(fs => fs.Size >= fileSize && fs.Start < currentStart)
                .OrderBy(fs => fs.Start)
                .FirstOrDefault();

            if (targetSpace != default)
            {
                // Move the file into the span
                for (int j = 0; j < fileSize; j++)
                {
                    disk[targetSpace.Start + j] = fileId;
                }

                // Clear the old file position
                for (int j = currentStart; j < currentStart + fileSize; j++)
                {
                    disk[j] = -1;
                }

                // Update freeSpaces
                UpdateFreeSpaces(freeSpaces, targetSpace, fileSize, targetSpace.Start + fileSize);
                UpdateFreeSpaces(freeSpaces, (currentStart, fileSize), 0, currentStart);

                // Re-sort freeSpaces after updates
                freeSpaces.Sort((a, b) => a.Start.CompareTo(b.Start));
            }
        }
    }

    private static void UpdateFreeSpaces(List<(int Start, int Size)> freeSpaces, (int Start, int Size) space, int usedSize, int newStart)
    {
        freeSpaces.Remove(space);

        int remainingSize = space.Size - usedSize;
        if (remainingSize > 0)
        {
            freeSpaces.Add((newStart, remainingSize));
        }
    }

    private static long CalculateChecksum(List<int> disk)
    {
        long checksum = 0;

        for (int i = 0; i < disk.Count; i++)
        {
            if (disk[i] != -1) // Skip free space
            {
                checksum += i * (long)disk[i];
            }
        }

        return checksum;
    }
}
