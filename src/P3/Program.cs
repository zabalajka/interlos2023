using System.Drawing;

namespace P3; // logo / monitor

public class Program
{
    private static bool[][] monitor;

    static void Main(string[] args)
    {
        monitor = File.ReadAllLines("./logo.csv")
            .Select(line => line.Replace(";", ""))
            .Select(line => line.Select(c => c == '_').ToArray())
            .ToArray(); // [y][x]

        decimal L = 365, T = 342, R = L + 42, B = T + 42;
        int moveX = -8, moveY = -5;




        Console.WriteLine("Hello, World!");
    }
}