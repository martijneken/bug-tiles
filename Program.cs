using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTiles
{
    class Program
    {
        class Places
        {
            public const int TILES = 8;
            public const int MAX_MOVES = 10;
            public int[] MOVES = new int[] { -1, 1 };

            List<int> guesses;
            bool[] possible;

            public Places(bool default_val)
            {
                guesses = new List<int>();
                possible = Enumerable.Repeat(default_val, TILES).ToArray();
            }
            public Places Check(int tile)
            {
                Places next = new Places(false);
                next.guesses = guesses.ToList();
                next.guesses.Add(tile);

                // For each existing possibility, move both left and right, as possible.
                // Be pessimistic: skip tiles where you will be found during the check.
                for (int t = 0; t < TILES; t++)
                {
                    if (this.possible[t])
                    {
                        foreach (int m in MOVES)
                        {
                            int n = t + m;
                            if (n >= 0 && n < TILES && n != tile)
                            {
                                next.possible[n] = true;
                            }
                        }
                    }
                }
                return next;
            }
            public bool IsComplete()
            {
                return possible.Sum(x => x ? 1 : 0) == 1;
            }
            public void PrintHistory()
            {
                Places state = new Places(true);
                foreach (var g in guesses)
                {
                    var s = state.Check(g);
                    Console.Out.WriteLine("CHECK: " + (g + 1) + " REMAIN: " + String.Join(",", s.possible.Select(p => p ? 1 : 0).ToArray()));
                    state = s;
                }
            }
            public void Print()
            {
                Console.Out.WriteLine("CHECKS: " + String.Join(",", guesses.Select(g => g + 1).ToArray()) + " REMAIN: " + String.Join(",", possible.Select(p => p ? 1 : 0).ToArray()));
            }
        }

        static void Main(string[] args)
        {
            List<Places> sequences = new List<Places>();
            sequences.Add(new Places(true));  // starting bugs could be anywhere

            for (int move = 1; move <= Places.MAX_MOVES; move++)
            {
                // Guess each spot in each sequence.
                List<Places> next = new List<Places>();
                foreach (var s in sequences)
                {
                    for (int t = 0; t < Places.TILES; t++) {
                        next.Add(s.Check(t));
                    }
                }
                sequences = next;

                // Check whether any sequences are complete and exit.
                bool found = false;
                foreach (var s in sequences)
                {
                    //s.Print();
                    if (s.IsComplete())
                    {
                        found = true;
                        Console.Out.WriteLine("SOLUTION FOUND!");
                        s.PrintHistory();
                    }
                }
                if (found) return;
            }
        }
    }
}
