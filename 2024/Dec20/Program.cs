using Dec20;
using System.Diagnostics;

var stopWatch = Stopwatch.StartNew();

var result = Part1.Execute();
Console.WriteLine($"Task1 result:{result}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);

stopWatch.Restart();
result = Part2.Execute();
Console.WriteLine($"Task2 result:{result}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);
