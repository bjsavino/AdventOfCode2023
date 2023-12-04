
using System.Text.RegularExpressions;

var solver = new Solver();
var result = await solver.SolveRule2Async("Input.txt");
Console.WriteLine(result);

internal class Solver
{
    internal async Task<int> SolveRule1Async(string path)
    {
        var text = await File.ReadAllTextAsync(path);
        var lineLength = text.IndexOf("\r\n");
        text = text.Replace("\r\n", string.Empty);
        //get number positions
        string patternNumber = @"(\d+)";
        string patternSymbol = @"([^\d.])";
        RegexOptions options = RegexOptions.Multiline;
        var matchNumbers = Regex.Matches(text, patternNumber, options);
        var matchSymbols = Regex.Matches(text, patternSymbol, options);

        var validNumbers = new List<int>();
        foreach (Match match in matchNumbers)
        {
            if (match.Index > 0 && matchSymbols.Any(m => m.Index == (match.Index - 1)))
            {
                //check on left
                validNumbers.Add(int.Parse(match.Value));
                Console.WriteLine(match);
                continue;
            }
            if ((match.Index + match.Length) < text.Length && matchSymbols.Any(m => (m.Index) == (match.Index + match.Length)))
            {
                //check on right
                validNumbers.Add(int.Parse(match.Value));
                Console.WriteLine(match);
                continue;
            }
            if (matchSymbols.Any(m => m.Index >= match.Index - 1 + lineLength
                            && m.Index <= match.Index + match.Length + lineLength
                            && (int)(m.Index / lineLength) - (int)(match.Index / lineLength) == 1))
            {
                //check line bottom
                validNumbers.Add(int.Parse(match.Value));
                Console.WriteLine(match);
                continue;
            }
            if (matchSymbols.Any(m => m.Index >= match.Index - 1 - lineLength
                && m.Index <= match.Index + match.Length - lineLength
                && (int)(m.Index / lineLength) - (int)(match.Index / lineLength) == -1))
            {
                //check line top
                validNumbers.Add(int.Parse(match.Value));
                Console.WriteLine(match);
                continue;
            }

        }
        return validNumbers.Sum();
    }
    internal async Task<int> SolveRule2Async(string path)
    {
        var text = await File.ReadAllTextAsync(path);
        var lineLength = text.IndexOf("\r\n");
        text = text.Replace("\r\n", string.Empty);
        //get number positions
        string patternNumber = @"(\d+)";
        string patternSymbol = @"(\*)";
        RegexOptions options = RegexOptions.Multiline;
        var matchNumbers = Regex.Matches(text, patternNumber, options);
        var matchSymbols = Regex.Matches(text, patternSymbol, options);

        var ratioSum = 0;
        foreach (Match match in matchSymbols)
        {
            var beforeNumber = matchNumbers.Where(n => (n.Index + n.Length == match.Index)
                                                        || (n.Index >= match.Index - lineLength - n.Length
                                                            && n.Index <= match.Index - lineLength + 1
                                                            && (int)(match.Index / lineLength) - (int)(n.Index / lineLength) == 1))
                                                        .ToList();


            var afterNumber = matchNumbers.Where(n => (n.Index == match.Index + 1)
                                                            || (n.Index >= match.Index + lineLength - n.Length
                                                                && n.Index <= match.Index + lineLength + 1
                                                                && (int)(match.Index / lineLength) - (int)(n.Index / lineLength) == -1))
                                                         .ToList();

            var numbers = beforeNumber.Union(afterNumber).ToList();
            if (numbers.Count == 2)
            {
                
                Console.WriteLine($"Gears {numbers[0]}*{numbers[1]}");
                ratioSum += int.Parse(numbers[0].Value) * int.Parse(numbers[1].Value);
            }
        }
        return ratioSum;
    }
}