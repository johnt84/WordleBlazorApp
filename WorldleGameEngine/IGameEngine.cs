using Shared;

namespace WorldleGameEngine
{
    public interface IGameEngine
    {
        GameState NewGame();
        GameState PlayerWonGame();
        GameState GameOver();
        GameState EnterGuess(string guess);
        int GetNumberOfGuesses();
    }
}
