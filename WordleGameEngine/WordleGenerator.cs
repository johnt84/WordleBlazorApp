using Microsoft.Extensions.Configuration;
using WordleGameEngine.Interfaces;

namespace WordleGameEngine;

public class WordleGenerator : IWordleGenerator
{
    private readonly List<string>? _possibleWordles = null;

    public WordleGenerator(IConfiguration configuration)
    {
        _possibleWordles = configuration.GetSection("PossibleWordles").Get<List<string>>();
    }

    public string? GenerateSelectedWordle()
    {
        if (_possibleWordles is null)
            return null;
        
        var possibleWordles = GetPossibleWordles();

        var random = new Random();
        int wordlePostion = random.Next(0, possibleWordles.Count);

        return possibleWordles[wordlePostion];
    }

    private List<string> GetPossibleWordles()
    {
        return _possibleWordles!
                    .GroupBy(x => x)
                    .Select(x => x.Key.ToLower())
                    .ToList()
                    .OrderBy(x => x)
                    .ToList();
    }
}
