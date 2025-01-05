using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text;

namespace Dec14;

internal class Part2
{
    public static long Execute()
    {
        var input = File.ReadAllLines(@"input.txt").ToList();

        int width = 101;
        int height = 103;

        var robots = ParseInput(input);

        int steps = 0;
        while (true)
        {
            StepRobots(robots, width, height);
            steps++;

            // Uncomment this to see how the locations are changing
            //DrawPositions(robots, width, height, $"robots_{steps-1:D4}.jpg");

            // we need to stop somewhere; why not once they are at unique positions ;)
            if (AllRobotsHaveUniquePositions(robots))
            {
                // Once all positions are unique, draw the final positions
                DrawPositions(robots, width, height, "robots_easter_egg.jpg");
                DrawPositionsToSvg(robots, width, height, "robots_easter_egg.svg");
                Console.WriteLine("Check your easter egg in the program output directory (robots_easter_egg.jpg or robots_easter_egg.svg).");
                return steps;
            }
        }
    }

    internal static readonly string[] separator = ["p=", " v=", ","];

    static List<Robot> ParseInput(List<string> input)
    {
        var robots = new List<Robot>();
        foreach (var line in input)
        {
            var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var position = (x: int.Parse(parts[0]), y: int.Parse(parts[1]));
            var velocity = (x: int.Parse(parts[2]), y: int.Parse(parts[3]));
            robots.Add(new Robot { Position = position, Velocity = velocity });
        }
        return robots;
    }

    static void StepRobots(List<Robot> robots, int width, int height)
    {
        foreach (var robot in robots)
        {
            var x = (robot.Position.x + robot.Velocity.x) % width;
            var y = (robot.Position.y + robot.Velocity.y) % height;

            if (x < 0) x += width;
            if (y < 0) y += height;

            robot.Position = (x, y);
        }
    }

    static bool AllRobotsHaveUniquePositions(List<Robot> robots)
    {
        var seenPositions = new HashSet<(int x, int y)>();
        foreach (var robot in robots)
        {
            if (!seenPositions.Add(robot.Position))
            {
                // Found a duplicate, not all are unique
                return false;
            }
        }
        return true; // All positions were unique
    }

    static void DrawPositionsToSvg(List<Robot> robots, int width, int height, string outputFile)
    {
        // Create a StringBuilder for SVG content
        var svgBuilder = new StringBuilder();

        // Start the SVG document
        svgBuilder.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\" viewBox=\"0 0 {width} {height}\">");
        svgBuilder.AppendLine($"<rect width=\"{width}\" height=\"{height}\" fill=\"white\" />"); // Background

        // Add each robot as a red circle
        foreach (var robot in robots)
        {
            int x = robot.Position.x;
            int y = robot.Position.y;

            // Ensure coordinates are within the bounds
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                svgBuilder.AppendLine($"<circle cx=\"{x}\" cy=\"{y}\" r=\"1\" fill=\"red\" />");
            }
        }

        // Close the SVG document
        svgBuilder.AppendLine("</svg>");

        // Save to file
        File.WriteAllText(outputFile, svgBuilder.ToString());
    }

    static void DrawPositions(List<Robot> robots, int width, int height, string outputFile)
    {
        // Create a new image with the specified dimensions and white background
        using var image = new Image<Rgba32>(width, height, Color.White);

        // Draw each robot as a red pixel
        foreach (var robot in robots)
        {
            int x = robot.Position.x;
            int y = robot.Position.y;

            // Ensure coordinates are within the image bounds
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                image[x, y] = Color.Red;
            }
        }

        // Save the image to the specified file
        image.Save(outputFile);

        // Alternative method using System.Drawing (requires System.Drawing.Common package), but only works on Windows
        //using var bmp = new Bitmap(width, height);
        //// Set the background to white
        //using (var g = Graphics.FromImage(bmp))
        //    g.Clear(Color.White);

        //// Draw each robot as a red pixel
        //foreach (var robot in robots)
        //{
        //    bmp.SetPixel(robot.Position.x, robot.Position.y, Color.Red);
        //}

        //bmp.Save(outputFile, System.Drawing.Imaging.ImageFormat.Jpeg);
    }

    class Robot
    {
        public (int x, int y) Position { get; set; }
        public (int x, int y) Velocity { get; set; }
    }
}
