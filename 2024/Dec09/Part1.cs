namespace Dec09;

internal class Part1
{
    public static long Execute()
    {
        var input = File.ReadAllText(@"input.txt").Trim();

        // Parse the disk map into a disk representation
        List<int> disk = ParseDisk(input);

        // Compact the disk
        CompactDisk(disk);

        // Calculate the checksum
        long checksum = CalculateChecksum(disk);

        // Calculate checksum
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
        int writeIndex = FindNextFreeSpace(disk, 0); // first free space

        // Iterate through the disk to move non-free blocks to the left
        for (int readIndex = disk.Count-1; readIndex >= 0; readIndex--)
        {
            if (disk[readIndex] != -1) // Non-free space
            {
                // Move block to the writeIndex and increment writeIndex
                if (readIndex > writeIndex)
                {
                    disk[writeIndex] = disk[readIndex];
                    disk[readIndex] = -1; // Mark the original position as free
                    writeIndex = FindNextFreeSpace(disk, writeIndex + 1);
                }
            }
        }
    }

    private static int FindNextFreeSpace(List<int> disk, int startPosition)
    {
        if (startPosition >= disk.Count-1) // we are out of free space
            return -1;

        for (var i = startPosition; i < disk.Count; i++)
        {
            if (disk[i] == -1)
            {
                return i;
            }
        }

        return -1;
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
