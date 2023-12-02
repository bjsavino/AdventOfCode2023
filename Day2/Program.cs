using System.Diagnostics.CodeAnalysis;

var solver = new Solver();
var result = await solver.SolveRule1Async("Input.txt");
Console.WriteLine(result);

var result2 = await solver.SolveRule2Async("Input.txt");
Console.WriteLine(result2);
internal class Solver
{
    internal async Task<int> SolveRule1Async(string path)
    {
        List<Game> games = await GetGames(path);
        IEnumerable<Game> possibleGames = games.Where(game => game.IsPossibleByRule1);

        return possibleGames.Sum(game => game.GameId);
    }
    internal async Task<int> SolveRule2Async(string path)
    {
        List<Game> games = await GetGames(path);
         return games.Sum(game => game.Power);
    }

    private static async Task<List<Game>> GetGames(string path)
    {
        var lines = await File.ReadAllLinesAsync(path);
        var games = new List<Game>();
        foreach (var line in lines)
        {
            var game = new Game(line);
            games.Add(game);
        }

        return games;
    }
}

internal class Game
{
    public Game(string line)
    {
        GameId = int.Parse(line.Split(':')[0].Replace("Game ", string.Empty));
        Sets = new List<CubeSet>();
        var sets = line.Split(':')[1].Split(';');
        foreach (var set in sets)
        {
            var cubeSet = new CubeSet(set);
            Sets.Add(cubeSet);
        }
    }
    public int GameId { get; set; }
    public List<CubeSet> Sets { get; set; }
    public int FewRed { get => Sets.Max(set => set.Red); }
    public int FewBlue { get => Sets.Max(set => set.Blue); }
    public int FewGreen { get => Sets.Max(set => set.Green); }
    public int Power { get => FewBlue * FewRed * FewGreen; }

    public bool IsPossibleByRule1
    {
        get
        {
            return !Sets.Any(set => set.Red > 12)
                && !Sets.Any(set => set.Green > 13)
                && !Sets.Any(set => set.Blue > 14);
        }
    }
}

public class CubeSet
{
    private int getBallQty(string ballData)
    {
        var value = int.Parse(ballData.Trim().Split(' ')[0]);
        return value;
    }
    public CubeSet(string set)
    {
        var balls = set.Split(',');
        foreach (var ball in balls)
        {
            if (ball.Contains("red"))
                Red += getBallQty(ball);
            else if (ball.Contains("blue"))
                Blue += getBallQty(ball);
            else if (ball.Contains("green"))
                Green += getBallQty(ball);
        }
    }
    public int Red { get; set; }
    public int Blue { get; set; }
    public int Green { get; set; }
}