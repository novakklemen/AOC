using Dec25;
using System.Diagnostics;

var stopWatch = Stopwatch.StartNew();

var result = Part1.Execute();
Console.WriteLine($"Task1 result:{result}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);

stopWatch.Restart();
var result2 = Part2.Execute();
Console.WriteLine($"Task2 result:{result2}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);
