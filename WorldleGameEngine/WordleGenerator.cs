using WorldleGameEngine.Interfaces;

namespace WorldleGameEngine
{
    public class WordleGenerator : IWordleGenerator
    {
        private readonly List<string> _possibleWordles;

        public WordleGenerator(List<string> possibleWordles)
        {
            _possibleWordles = possibleWordles;
        }

        public string GenerateSelectedWordle()
        {
            var possibleWordles = GetPossibleWordles();

            var random = new Random();
            int wordlePostion = random.Next(0, possibleWordles.Count);

            return possibleWordles[wordlePostion];
        }

        private List<string> GetPossibleWordles()
        {
            return _possibleWordles
                        .GroupBy(x => x)
                        .Select(x => x.Key)
                        .ToList()
                        .OrderBy(x => x)
                        .ToList();
        }
    }
}
