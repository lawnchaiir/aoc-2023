
namespace day_two;
class Program
{
    class GameRound
    {
        public readonly int RoundNumber = 0;

        List<Tuple<string, int>> CubeGroups = new List<Tuple<string, int>>();

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
                    string cubeColor = cubeSpan[(spacePos+1)..].ToString();
                    CubeGroups.Add(new Tuple<string, int> (cubeColor, cubeCount));
                }
            }
        }

        public bool IsPossible(int redCount,  int greenCount, int blueCount)
        {
            foreach (Tuple<string, int> cubes in CubeGroups)
            {
                switch (cubes.Item1)
                {
                    case "red":
                    {
                        if (cubes.Item2 > redCount)
                        {
                            return false;
                        }
                    }
                    break;
                    case "green":
                    {
                        if (cubes.Item2 > greenCount)
                        {
                            return false;
                        }
                    }
                    break;
                    case "blue":
                    {
                        if (cubes.Item2 > blueCount)
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

            foreach (Tuple<string, int> cubes in CubeGroups)
            {
                switch (cubes.Item1)
                {
                    case "red":
                    {
                        minReds = Math.Max(minReds, cubes.Item2);
                    }
                    break;
                    case "green":
                    {
                        minGreens = Math.Max(minGreens, cubes.Item2);
                    }
                    break;
                    case "blue":
                    {
                        minBlues = Math.Max(minBlues, cubes.Item2);
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

            if (round.IsPossible(12,13,14))
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
