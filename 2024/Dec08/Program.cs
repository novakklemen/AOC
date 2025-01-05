using Dec08;
using System.Diagnostics;

var stopWatch = new Stopwatch();
stopWatch.Start();

var result = Part1.Execute();
Console.WriteLine($"Task1 result:{result}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);

stopWatch.Restart();
result = Part2.Execute();
Console.WriteLine($"Task2 result:{result}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);