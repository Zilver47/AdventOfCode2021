using System;
using System.Diagnostics;
using System.IO;
using AdventOfCode.Day25;

var timer = new Stopwatch();
timer.Start();

var lines = File.ReadAllLines("Day25\\input.txt");
var generator = new AnswerGenerator(lines);
            
Console.Write("Answer 1: ");
WriteAnswer(generator.Part1().ToString());
Console.WriteLine();
            
timer.Stop();
Console.WriteLine($"Verstreken tijd: {timer.Elapsed.TotalSeconds}");
            
timer.Restart();

Console.WriteLine();
Console.Write("Answer 2: ");
WriteAnswer(generator.Part2().ToString());
Console.WriteLine();
        
timer.Stop();
Console.WriteLine($"Verstreken tijd: {timer.Elapsed.TotalSeconds}");

Console.ReadLine();

static void WriteAnswer(string value)
{
    var originalForegroundColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(value);
    Console.ForegroundColor = originalForegroundColor;
}
