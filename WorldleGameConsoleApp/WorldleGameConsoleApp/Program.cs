using WorldleGameEngine;

var gameEngine = new GameEngine();

bool keepPlaying = true;

Console.WriteLine("Welcome to Wordle.  The aim of the game is to guess the randomly generated 5 letter word called a wordle");

while(keepPlaying)
{
    var gameState = gameEngine.NewGame();

    while (!gameState.IsGameComplete)
    {
        Console.WriteLine("\nPlease enter your guess for the selected wordle?");
        string guess = Console.ReadLine();

        gameState = gameEngine.EnterGuess(guess);

        if (!gameState.GuessResult.IsGuessSuccessful || gameState.IsGameComplete)
        {
            continue;
        }

        if (gameState.GuessResult.IncorrectGuessHints.LettersPresentInGuessAndInCorrectPosition.Count > 0)
        {
            Console.WriteLine($"\nThe following letters from your guess are in the same position as the selected wordle: {string.Join(" ", gameState.GuessResult.IncorrectGuessHints.LettersPresentInGuessAndInCorrectPosition)}");
            Console.WriteLine($"\nThe following letter positions from your guess are in the same position as the selected wordle: {string.Join(" ", gameState.GuessResult.IncorrectGuessHints.LetterPositionsPresentInGuessAndInCorrectPosition)}");
        }

        if (gameState.GuessResult.IncorrectGuessHints.LettersPresentInGuessButNotInCorrectPosition.Count > 0)
        {
            Console.WriteLine($"\nThe following letters from your guess are in the selected wordle but not in the correct position: {string.Join(" ", gameState.GuessResult.IncorrectGuessHints.LettersPresentInGuessButNotInCorrectPosition)}");
            Console.WriteLine($"\nThe following letter positions from your guess are in the selected wordle but not in the correct position: {string.Join(" ", gameState.GuessResult.IncorrectGuessHints.LetterPositionsPresentInGuessButNotInCorrectPosition)}");
        }

        if (gameState.GuessResult.IncorrectGuessHints.LettersNotPresentInGuess.Count > 0)
        {
            Console.WriteLine($"\nThe following letters from your guess are not in the selected wordle: {string.Join(" ", gameState.GuessResult.IncorrectGuessHints.LettersNotPresentInGuess)}");
            Console.WriteLine($"\nThe following letter positions from your guess are not in the selected wordle: {string.Join(" ", gameState.GuessResult.IncorrectGuessHints.LetterPositionsNotPresentInGuess)}");
        }
    }

    if (gameState.HasPlayerWonGame)
    {
        Console.WriteLine($"\n{gameState.GuessResult.ResultMessage}");
    }
    else
    {
        Console.WriteLine($"\n{gameState.GuessResult.ResultMessage}");
    }

    string keepPlayingChoice = string.Empty;

    while (string.IsNullOrEmpty(keepPlayingChoice))
    {
        Console.WriteLine($"\nWould you like another game of wordle? (y/n)?");
        keepPlayingChoice = Console.ReadLine();
    }

    keepPlaying = keepPlayingChoice == "y";
}

Console.WriteLine($"\nThanks for playing wordle");