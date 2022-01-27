namespace WordleBlazorApp.Shared
{
    public class GameState
    {
        public bool IsGameComplete { get; set; }
        public bool HasPlayerWonGame { get; set; }
        public int NumberOfGuesses { get; set; }
        public GuessResult GuessResult { get; set; }
    }
}
