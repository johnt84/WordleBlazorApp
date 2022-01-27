namespace WordleBlazorApp.Shared
{
    public class IncorrectGuessHints
    {
        public List<char> LettersPresentInGuessAndInCorrectPosition { get; set; } = new List<char>();
        public List<char> LettersPresentInGuessButNotInCorrectPosition { get; set; } = new List<char>();
        public List<char> LettersNotPresentInGuess { get; set; } = new List<char>();
        public List<int> LetterPositionsPresentInGuessAndInCorrectPosition { get; set; } = new List<int>();
        public List<int> LetterPositionsPresentInGuessButNotInCorrectPosition { get; set; } = new List<int>();
        public List<int> LetterPositionsNotPresentInGuess { get; set; } = new List<int>();
    }
}
