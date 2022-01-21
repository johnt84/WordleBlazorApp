﻿using Shared;

namespace WorldleGameEngine
{
    public interface IGameEngine
    {
        GameState NewGame();
        GameState EnterGuess(string guess);
        int GetNumberOfGuesses();
    }
}
