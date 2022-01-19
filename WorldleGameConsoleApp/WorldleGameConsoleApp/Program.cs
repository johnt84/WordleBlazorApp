using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName)
    .AddJsonFile("appSettings.json")
    .Build();

var possibleWordles = GetPossibleWordles();
                       
string wordle = string.Empty;

string guess = string.Empty;

bool keepPlaying = true;

bool gameComplete = false;

bool playerWonGame = false;

int numberOfGuesses = 0;

int worldleLength = 5;

int numberOfAllowedGuesses = 6;

Console.WriteLine("Welcome to Wordle.  The aim of the game is to guess the randomly generated 5 letter word called a wordle");

while(keepPlaying)
{
    NewGame();

    while (!gameComplete)
    {
        Console.WriteLine("\nPlease enter your guess for the selected wordle?");
        guess = Console.ReadLine();

        if (string.IsNullOrEmpty(guess) || guess.Length != worldleLength || guess.Any(char.IsDigit))
        {
            continue;
        }

        numberOfGuesses++;

        if (guess.ToLower().Equals(wordle))
        {
            PlayerWonGame();
        }
        else
        {
            if (numberOfGuesses == numberOfAllowedGuesses)
            {
                GameOver();
            }
            else
            {
                var incorrectGuessHints = GetIncorrectGuessHints(guess, wordle);

                if (incorrectGuessHints.LettersPresentInGuessAndInCorrectPosition.Count > 0)
                {
                    Console.WriteLine($"\nThe following letters from your guess are in the same position as the selected wordle: {string.Join(" ", incorrectGuessHints.LettersPresentInGuessAndInCorrectPosition)}");
                }

                if (incorrectGuessHints.LettersPresentInGuessButNotInCorrectPosition.Count > 0)
                {
                    Console.WriteLine($"\nThe following letters from your guess are in the selected wordle but not in the correct position: {string.Join(" ", incorrectGuessHints.LettersPresentInGuessButNotInCorrectPosition)}");
                }

                if (incorrectGuessHints.LettersNotPresentInGuess.Count > 0)
                {
                    Console.WriteLine($"\nThe following letters from your guess are not in the selected wordle: {string.Join(" ", incorrectGuessHints.LettersNotPresentInGuess)}");
                }
            }
        }
    }

    if (playerWonGame)
    {
        string es = numberOfGuesses > 1 ? "es" : string.Empty;

        Console.WriteLine($"\nCongratulations you correctly guessed the selected wordle {wordle} in {numberOfGuesses} guess{es}");
    }
    else
    {
        Console.WriteLine($"\nUnlucky you didn't guess the selected wordle {wordle}");
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

void NewGame()
{
    var random = new Random();
    int wordlePostion = random.Next(0, possibleWordles.Count);

    wordle = possibleWordles[wordlePostion];

    gameComplete = false;
    playerWonGame = false;
    numberOfGuesses = 0;
}

void PlayerWonGame()
{
    gameComplete = true;
    playerWonGame = true;
}

void GameOver()
{
    gameComplete = true;
    playerWonGame = false;
}

IncorrectGuessHints GetIncorrectGuessHints(string guess, string worldle)
{
    var lettersPresentInGuess = guess.Intersect(wordle).ToList();
    var lettersNotPresentInGuess = guess.Except(wordle).ToList();
    var lettersPresentAndInCorrectPosition = new List<char>();
    var lettersPresentButNotInCorrectPosition = new List<char>();

    if (lettersPresentInGuess.Count() > 0)
    {
        foreach (var letter in lettersPresentInGuess)
        {
            int positionOfLetterInGuess = guess.IndexOf(letter);
            int positionOfLetterInWordle = wordle.IndexOf(letter);

            if (positionOfLetterInGuess == positionOfLetterInWordle)
            {
                lettersPresentAndInCorrectPosition.Add(letter);
            }
            else
            {
                lettersPresentButNotInCorrectPosition.Add(letter);
            }
        }
    }

    return new IncorrectGuessHints()
    {
        LettersPresentInGuessAndInCorrectPosition = lettersPresentAndInCorrectPosition,
        LettersPresentInGuessButNotInCorrectPosition = lettersPresentButNotInCorrectPosition,
        LettersNotPresentInGuess = lettersNotPresentInGuess,
    };
}

List<string> GetPossibleWordles()
{
    var possibleWordlesInput = config.GetSection("PosssibleWordles").Get<List<string>>();

    return possibleWordlesInput
                            .GroupBy(x => x)
                            .Select(x => x.Key)
                            .ToList()
                            .OrderBy(x => x)
                            .ToList();
}

public class IncorrectGuessHints
{
    public List<char> LettersPresentInGuessAndInCorrectPosition { get; set; }
    public List<char> LettersPresentInGuessButNotInCorrectPosition { get; set; }
    public List<char> LettersNotPresentInGuess { get; set; }
}