namespace WordleBlazorApp.Shared;

public class GameGrid
{
    public string[,] Guesses { get; set; } = null!;
    public string[,] IncorrectGuessHintColours { get; set; } = null!;
}
