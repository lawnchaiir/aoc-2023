
namespace day_two;
class Program
{
    enum Color
    {
        Red,
        Green,
        Blue
    }

    class GameRound
    {
        public readonly int RoundNumber = 0;

        private readonly struct CubeSelection
        {
            public readonly Color Color;
            public readonly int CubeCount;

            public CubeSelection(Color color, int cubeCount)
            {
                Color = color;
                CubeCount = cubeCount;
            }
        }

        readonly List<CubeSelection> CubeGroups = new List<CubeSelection>();

        public GameRound(string line)
        {
            ReadOnlySpan<char> lineSpan = line.AsSpan();
            int colonPos = line.IndexOf(':');
            ReadOnlySpan<char> roundNum = lineSpan["Game ".Length..colonPos];
            RoundNumber = int.Parse(roundNum);
            int moveStart = colonPos + 2;

            string[] moves = lineSpan[moveStart..].ToString().Split("; ");

            foreach (string move in moves)
            {
                string[] cubes = move.Split(", ");

                foreach (string cube in cubes)
                {
                    int spacePos = cube.IndexOf(" ");
                    ReadOnlySpan<char> cubeSpan = cube.AsSpan();

                    int cubeCount = int.Parse(cubeSpan[..spacePos]);
                    ReadOnlySpan<char> cubeColorStr = cubeSpan[(spacePos+1)..];
                    Color? cubeColor = GetColorFromString(cubeColorStr);
                    if (cubeColor != null)
                    {
                        CubeGroups.Add(new CubeSelection(cubeColor.Value, cubeCount));
                    }
                }
            }
        }

        private static Color? GetColorFromString(ReadOnlySpan<char> cubeColor)
        {
            if (cubeColor.SequenceEqual("red".AsSpan()))
            {
                return Color.Red;
            }
            if (cubeColor.SequenceEqual("green".AsSpan()))
            {
                return Color.Green;
            }
            if (cubeColor.SequenceEqual("blue".AsSpan()))
            {
                return Color.Blue;
            }

            return null;
        }

        public bool IsPossible(int redCount,  int greenCount, int blueCount)
        {
            foreach (CubeSelection cubes in CubeGroups)
            {
                switch (cubes.Color)
                {
                    case Color.Red:
                    {
                        if (cubes.CubeCount > redCount)
                        {
                            return false;
                        }
                    }
                    break;
                    case Color.Green:
                    {
                        if (cubes.CubeCount > greenCount)
                        {
                            return false;
                        }
                    }
                    break;
                    case Color.Blue:
                    {
                        if (cubes.CubeCount > blueCount)
                        {
                            return false;
                        }
                    }
                    break;
                }

            }
            return true;
        }

        public int CalculateMinimumPowers()
        {
            int minReds = 0;
            int minGreens = 0;
            int minBlues = 0;

            foreach (CubeSelection cubes in CubeGroups)
            {
                switch (cubes.Color)
                {
                    case Color.Red:
                    {
                        minReds = Math.Max(minReds, cubes.CubeCount);
                    }
                    break;
                    case Color.Green:
                    {
                        minGreens = Math.Max(minGreens, cubes.CubeCount);
                    }
                    break;
                    case Color.Blue:
                    {
                        minBlues = Math.Max(minBlues, cubes.CubeCount);
                    }
                    break;
                }
            }
            return minReds * minBlues * minGreens;
        }

        public override string ToString()
        {
            return $"Round: {RoundNumber}";
        }
    }

    static void Main()
    {
        string input = LoadInput("input.txt");

        string[] lines = input.Split(Environment.NewLine);

        int validGameScore = 0;
        int powersSum = 0;
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            GameRound round = new GameRound(line);

            if (round.IsPossible(12, 13, 14))
            {
                validGameScore += round.RoundNumber;
            }

            int minimumPower = round.CalculateMinimumPowers();
            powersSum += minimumPower;
        }


        Console.WriteLine(validGameScore);
        Console.WriteLine(powersSum);
    }

    static string LoadInput(string path)
    {
        using FileStream fs = File.OpenRead(path);
        using StreamReader sr = new StreamReader(fs);
        return sr.ReadToEnd();
    }
}
