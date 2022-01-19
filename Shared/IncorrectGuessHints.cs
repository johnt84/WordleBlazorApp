namespace Shared
{
    public class IncorrectGuessHints
    {
        public List<char> LettersPresentInGuessAndInCorrectPosition { get; set; } = new List<char>();
        public List<char> LettersPresentInGuessButNotInCorrectPosition { get; set; } = new List<char>();
        public List<char> LettersNotPresentInGuess { get; set; } = new List<char>();
    }
}
