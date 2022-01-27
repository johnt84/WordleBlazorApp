namespace WordleBlazorApp.Shared
{
    public class GameGrid
    {
        public string[,] Guesses { get; set; }
        public string[,] IncorrectGuessHintColours { get; set; }
    }
}
