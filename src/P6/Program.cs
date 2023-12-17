namespace P6; // most

internal class Program
{
    private static List<int[]> numbers;
    // Dictionary<row, List<(row, col, bitmap, startingRow, visitedTiles)>>
    private static Dictionary<int, List<Tuple<int, int, int, int, List<int>>>> backlog = new ();
    private static int globalMax;

    static void Main(string[] args)
    {
        var tiles = new int[24, 8];
        numbers = File.ReadAllLines("./bridge.txt")
            .Where(line => !string.IsNullOrEmpty(line))
            .Select(line => line.Replace(" ", "").Split(',').Select(int.Parse).ToArray())
            .ToList();

        for (int i = 0; i < 24; i++)
        {
            backlog[i] = new List<Tuple<int, int, int, int, List<int>>>();
        }

        // The idea was first to find starting tile (0-7) for which there exists a solution.
        // During that pass the order of visited tiles was not recorded
        // and only bitmap of whether a tile was visited for being used.
        // Second run was to be limited only to "correct" starting tile, and this time also
        // recording the order of visited tiles.
        // After realizing that the movement can only for forward, this optimization was
        // not strictly necessary.
        for (int cc = 0; cc < 1; cc++)
        {
            AddToBacklog(0, 0, numbers[0][0], 0, 0, new List<int>());
            AddToBacklog(0, 1, numbers[0][1], 0, 1, new List<int>());
            AddToBacklog(0, 2, numbers[0][2], 0, 2, new List<int>());
            AddToBacklog(0, 3, numbers[0][3], 0, 3, new List<int>());
            AddToBacklog(0, 4, numbers[0][4], 0, 4, new List<int>());
            AddToBacklog(0, 5, numbers[0][5], 0, 5, new List<int>());
            AddToBacklog(0, 6, numbers[0][6], 0, 6, new List<int>());
            AddToBacklog(0, 7, numbers[0][7], 0, 7, new List<int>());

            int counter = 0;
            while (true)
            {
                // The original implementation was considering also movement backwards and horizontally
                // (incorrect assumption), so the idea was to process those attempts
                // that were the furthest ahead on the bridge.
                int i;
                bool isFound = false;
                for (i = 23 - 1; i >= 0; i--)
                {
                    if (backlog[i].Count > 0)
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    break;
                }

                var item = backlog[i][^1];
                backlog[i].RemoveAt(backlog[i].Count - 1);

                counter++;
                if (counter == 1000)
                {
                    counter = 0;
                    Console.WriteLine($"Max = {globalMax}, Last Max = {item.Item1}, Last = {item.Item3}");
                }

                Solve(item);
            }
        }
    }

    public static void Solve(Tuple<int, int, int, int, List<int>> item)
    {
        //if (item.Item1 == 23)
        //{
        //    throw new Exception("solved");
        //}
        int next;

        //if (item.Item1 > 0)
        //{
        //    next = numbers[item.Item1 - 1][item.Item2]; // up
        //    AddToBacklog(item.Item1 - 1, item.Item2, next, item.Item3, item.Item4);
        //}

        //if (item.Item1 > 0 && item.Item2 < 7)
        //{
        //    next = numbers[item.Item1 - 1][item.Item2 + 1]; // up right
        //    AddToBacklog(item.Item1 - 1, item.Item2 + 1, next, item.Item3, item.Item4);
        //}

        //if (item.Item1 > 0 && item.Item2 > 0)
        //{
        //    next = numbers[item.Item1 - 1][item.Item2 - 1]; // up left
        //    AddToBacklog(item.Item1 - 1, item.Item2 - 1, next, item.Item3, item.Item4);
        //}

        next = numbers[item.Item1 + 1][item.Item2]; // down
        AddToBacklog(item.Item1 + 1, item.Item2, next, item.Item3, item.Item4, item.Item5);
        globalMax = Math.Max(globalMax, item.Item1 + 1);

        if (item.Item2 < 7)
        {
            next = numbers[item.Item1 + 1][item.Item2 + 1]; // down right
            AddToBacklog(item.Item1 + 1, item.Item2 + 1, next, item.Item3, item.Item4, item.Item5);

            //next = numbers[item.Item1][item.Item2 + 1]; // right
            //AddToBacklog(item.Item1, item.Item2 + 1, next, item.Item3, item.Item4);
        }

        if (item.Item2 > 0)
        {
            next = numbers[item.Item1 + 1][item.Item2 - 1]; // down left
            AddToBacklog(item.Item1 + 1, item.Item2 - 1, next, item.Item3, item.Item4, item.Item5);

            //next = numbers[item.Item1][item.Item2 - 1]; // left
            //AddToBacklog(item.Item1, item.Item2 - 1, next, item.Item3, item.Item4);
        }
    }

    public static void AddToBacklog(int row, int col, int next, int current, int startCol, List<int> prev)
    {
        if ((current >> next & 0x1) == 0x1)
        {
            return; // already used
        }
        //if (current.Contains(next))
        //{
        //    return; // already used
        //}

        int copy = current | 0x1 << next;
        var copy2 = new List<int>(prev) { next };
        backlog[row].Add(new Tuple<int, int, int, int, List<int>>(row, col, copy, startCol, copy2));

        if (row == 23) // upon reaching the last row, solution is found
        {
            Console.WriteLine($"StartCol = {startCol}");
            for (int i = 0; i < copy2.Count; i++)
            {
                Console.Write(copy2[i]);
                Console.Write(",");
            }
            throw new Exception("done");
        }
    }
}