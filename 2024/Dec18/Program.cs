using Dec18;
using System.Diagnostics;

var stopWatch = Stopwatch.StartNew();

var result = Part1.Execute();
Console.WriteLine($"Task1 result:{result}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);

stopWatch.Restart();
var resultTwo = Part2.Execute();
Console.WriteLine($"Task2 result:{resultTwo}");
Console.WriteLine(stopWatch.ElapsedMilliseconds);
