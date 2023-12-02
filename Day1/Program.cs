var solver = new Solver();
var result = await solver.SolveAsync("Input2.txt");
Console.WriteLine(result);

internal class Solver
{
    internal async Task<int> SolveAsync(string path)
    {
        var lines = await File.ReadAllLinesAsync(path);
        var result = 0;
        foreach (var line in lines)
        {
            var encodedLine = ReplaceTextNumber(line);
            result += GetCalibrationValue(encodedLine);
        }
        return result;
    }

    private string ReplaceTextNumber(string line)
    {
        var numbers = new Dictionary<string, char>()
        {
            {"one",   '1'},
            {"two",   '2'},
            {"three", '3'},
            {"four",  '4'},
            {"five",  '5'},
            {"six",   '6'},
            {"seven", '7'},
            {"eight", '8'},
            {"nine",  '9'},

        };

        var decodedLine = new Span<char>(line.ToCharArray());
        line.CopyTo(decodedLine);

        foreach (var n in numbers)
        {
            int index = -1;
            do
            {
                index = line.IndexOf(n.Key, index + 1);

                if (index >= 0)
                    decodedLine[index] = n.Value;
            }
            while (index >= 0);
        }

        return decodedLine.ToString();
    }

    private int GetCalibrationValue(string line)
    {
        var span = line.AsSpan();
        int first = 0;
        int second = 0;
        for (var i = 0; i < span.Length; i++)
        {
            if (char.IsDigit(span[i]))
            {
                first = span[i] - '0';
                break;
            }
        }
        for (var i = span.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(span[i]))
            {
                second = span[i] - '0';
                break;
            }
        }
        var value = first * 10 + second;
        //Console.WriteLine(value);
        return value;
    }
}
