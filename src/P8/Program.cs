using System.Numerics;

namespace P8; // house numbers

internal class Program
{
    static void Main(string[] args)
    {
        long max = 12345678987654321;
        string binary = Convert.ToString(max, 2);

        long maxB = 0b10_1011_1101_1100_0101_0100_0110_0010_1001_0001_1111_0100_1011_0001; // 26x1, 27x0
        long minB = 0b10_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;

        BigInteger count = 0;
        for (int k = 1; k <= 17; k++)
        {
            int zeros = k;
            int ones = 2 * k - 1; // always leading one
            count += Choose(ones + zeros, ones);
        }

        // 18:
        {
            int k = 18;
            int zeros = k;
            int ones = 2 * k;

            // 0b10_0
            //int onesEx = ones - 1;
            //int zerosEx = zeros - 2;
            //count += Choose(onesEx + zerosEx, onesEx);

            // 0b10_10

            string numSource = "10_1011_1101_1100_0101_0100_0110_0010_1001_0001_1111_0100_1011_0001".Replace("_", "");
            for (int i = 1; i < numSource.Length; i++)
            {
                string num = numSource.Substring(i);
                if (num[0] == '0')
                {
                    continue; // if 0 is first, number cannot be higher
                }

                // count except the first 1
                string numStart = numSource.Substring(0, i);
                int onesEx = ones - numStart.Count(c => c == '1');
                int zerosEx = zeros - numStart.Count(c => c == '0') - 1; // treat as starting with zero
                count += Choose(onesEx + zerosEx, onesEx);
            }
        }

        Console.WriteLine(count);
        Console.WriteLine(binary);
    }

    // taken from https://visualstudiomagazine.com/articles/2022/07/20/math-combinations-using-csharp.aspx
    static BigInteger Choose(int n, int k)
    {
        // number combinations
        if (n < 0 || k < 0)
            throw new Exception("Negative argument in Choose()");
        if (n < k) return 0; // special
        if (n == k) return 1; // short-circuit

        int delta, iMax;

        if (k < n - k) // ex: Choose(100,3)
        {
            delta = n - k; iMax = k;
        }
        else           // ex: Choose(100,97)
        {
            delta = k; iMax = n - k;
        }

        BigInteger ans = delta + 1;
        for (int i = 2; i <= iMax; ++i)
            ans = (ans * (delta + i)) / i;

        return ans;
    }
}