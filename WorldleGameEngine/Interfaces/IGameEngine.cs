using WordleBlazorApp.Shared;

namespace WorldleGameEngine.Interfaces
{
    public interface IGameEngine
    {
        GameState NewGame();
        GameState EnterGuess(string guess);
        int GetNumberOfGuesses();
        GameGrid GetGameGrid();
    }
}
