using Dec02;
using System.Diagnostics;

var stopWatch = new Stopwatch();
stopWatch.Start();

var result = Part1.Execute();
int safeReportsCount = Part1.Execute();
Console.WriteLine($"Number of safe reports: {safeReportsCount}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);

stopWatch.Restart();
safeReportsCount = Part2.Execute();
Console.WriteLine($"Number of safe reports: {safeReportsCount}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);