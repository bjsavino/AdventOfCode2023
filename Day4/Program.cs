using System.Dynamic;

var solver = new Solver();
var result = await solver.SolveRule2Async("Input.txt");
Console.WriteLine(result);
internal class Solver
{
    internal async Task<int> SolveRule1Async(string path)
    {
        List<Card> cards = await GetCards(path);
        return cards.Sum(c => c.Score);
    }

    internal async Task<int> SolveRule2Async(string path)
    {
        List<Card> cards = await GetCards(path);
        var cardsQty = cards.Select(c => new CardInventory(c)).ToList();

        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < cards[i].HaveWinner.Count; j++)
            {
                cardsQty[i + j + 1].Qty += cardsQty[i].Qty;
            }   
        }
        return cardsQty.Sum(c => c.Qty);
    }
    private static async Task<List<Card>> GetCards(string path)
    {
        var lines = await File.ReadAllLinesAsync(path);
        var cards = new List<Card>();
        foreach (var line in lines)
        {
            var card = new Card(line);
            cards.Add(card);
        }

        return cards;
    }
}
internal record CardInventory
{
    public Card Card { get; set; }
    public int Qty { get; set; }

    public CardInventory(Card card)
    {
        Card = card;
        Qty = 1;
    }
}
internal class Card
{
    public Card(string line)
    {
        CardNumber = int.Parse(line.Split(':')[0].Replace("Card ", string.Empty));
        Winners = line.Split(':')[1].Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n.Trim())).ToList();
        Have = line.Split(':')[1].Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n.Trim())).ToList();
    }
    public int CardNumber { get; set; }
    public List<int> Winners { get; set; }
    public List<int> Have { get; set; }
    public List<int> HaveWinner { get => Have.Where(c => Winners.Contains(c)).ToList(); }
    public int Score
    {
        get
        {

            if (HaveWinner.Any())
            {
                var score = (int)Math.Pow(2, (HaveWinner.Count() - 1));
                return score;
            }
            return 0;
        }
    }
}
