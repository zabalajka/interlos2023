namespace P1; // plotr

public class Program
{
    static void Main(string[] args)
    {
        FirstPass();
        SecondPass();
    }

    private static void FirstPass()
    {
        // 1. read input file
        // 2. for each line:
        //    - record whether current position is "colored"
        //    - move cursor/head to new position (left, right, top, down)
        // 3. write result to output file

        var lines = File.ReadAllLines("losi-plotr.txt");
        var pixels = new List<Tuple<int, int>>();
        int x = 0, y = 0;

        foreach (string line in lines)
        {
            bool print = line[0] == '#';
            char direction = line[1];

            if (print)
            {
                pixels.Add(new Tuple<int, int>(x, y));
            }

            if (direction == 'L')
            {
                x--;
            }
            else if (direction == 'R')
            {
                x++;
            }
            else if (direction == 'U')
            {
                y--;
            }
            else
            {
                y++;
            }
        }

        int width = pixels.Max(v => v.Item1) + 1;
        int height = pixels.Max(v => v.Item2) + 1;

        using (var fs = File.OpenWrite("./output.txt"))
        {
            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width; x++)
                {
                    if (pixels.Any(p => p.Item1 == x && p.Item2 == y))
                    {
                        fs.Write(new[] { (byte)'#' });
                    }
                    else
                    {
                        fs.Write(new[] { (byte)' ' });
                    }
                }
                fs.Write(new[] { (byte)'\n' });
            }
        }
    }

    private static void SecondPass()
    {
        // same as first pass, but each instruction spans 6 lines and is "drawn" with hash (#)

        var lines = File.ReadAllLines("output.txt");
        var pixels = new List<Tuple<int, int>>();
        int x = 0, y = 0;

        for (var i = 0; i < lines.Length; i+= 6)
        {
            var line = lines[i];
            bool print = lines[i][3] == '#';
            char direction = line[1];

            if (print)
            {
                pixels.Add(new Tuple<int, int>(x, y));
            }

            if (lines[i][13] == '#') // D
            {
                y++;
            }
            if (lines[i][12] == '#') // R
            {
                x++;
            }
            else if (lines[i][15] == '#') // U
            {
                y--;
            }
            else if (lines[i + 4][9] == '#') // L
            {
                x--;
            }
            else
            {
                throw new Exception();
            }
            
        }

        int width = pixels.Max(v => v.Item1) + 1;
        int height = pixels.Max(v => v.Item2) + 1;

        using (var fs = File.OpenWrite("./output2.txt"))
        {
            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width; x++)
                {
                    if (pixels.Any(p => p.Item1 == x && p.Item2 == y))
                    {
                        fs.Write(new[] { (byte)'#' });
                    }
                    else
                    {
                        fs.Write(new[] { (byte)' ' });
                    }
                }
                fs.Write(new[] { (byte)'\n' });
            }
        }
    }
}