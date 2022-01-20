using Microsoft.Extensions.Configuration;
using Shared;

namespace WorldleGameEngine
{
    public class GameEngine : IGameEngine
    {
        private const int NUMBER_OF_ALLLOWED_GUESSES = 6;
        private int WORDLE_LENGTH = 5;

        private string wordle = string.Empty;
        private int numberOfGuesses = 0;

        public GameState NewGame()
        {
            GetSelectedWordle();

            numberOfGuesses = 0;

            return new GameState()
            {
                IsGameComplete = false,
                HasPlayerWonGame = false,
                NumberOfGuesses = numberOfGuesses,
            };
        }

        public GameState PlayerWonGame()
        {
            string es = numberOfGuesses > 1 ? "es" : string.Empty;

            return new GameState()
            {
                IsGameComplete = true,
                HasPlayerWonGame = true,
                NumberOfGuesses = numberOfGuesses,
                GuessResult = new GuessResult()
                {
                    IsGuessSuccessful = false,
                    IncorrectGuessHints = null,
                    ResultMessage = $"Congratulations you correctly guessed the selected wordle { wordle } in { numberOfGuesses} guess{ es}",
                },
            };
        }

        public GameState GameOver()
        {
            return new GameState()
            {
                IsGameComplete = true,
                HasPlayerWonGame = false,
                NumberOfGuesses = numberOfGuesses,
                GuessResult = new GuessResult()
                {
                    IsGuessSuccessful = false,
                    IncorrectGuessHints = null,
                    ResultMessage = $"Unlucky you didn't guess the selected wordle {wordle}",
                },
            };
        }

        public GameState EnterGuess(string guess)
        {
            if (string.IsNullOrEmpty(guess) || guess.Length != WORDLE_LENGTH || guess.Any(char.IsDigit))
            {
                return new GameState()
                {
                    IsGameComplete = false,
                    HasPlayerWonGame = false,
                    NumberOfGuesses = numberOfGuesses,
                    GuessResult = new GuessResult()
                    {
                        IsGuessSuccessful = false,
                        IncorrectGuessHints = null,
                        ResultMessage = "Invalid guess.  Please enter a valid guess",
                    },
                };
            }

            string guessResult = string.Empty;
            IncorrectGuessHints? incorrectGuessHints = null;

            numberOfGuesses++;

            if (guess.ToLower().Equals(wordle))
            {
                return PlayerWonGame();
            }
            else
            {
                if (numberOfGuesses == NUMBER_OF_ALLLOWED_GUESSES)
                {
                    return GameOver();
                }
                else
                {
                    incorrectGuessHints = GetIncorrectGuessHints(guess, wordle);
                }
            }

            return new GameState()
            {
                IsGameComplete = false,
                HasPlayerWonGame = false,
                NumberOfGuesses = numberOfGuesses,
                GuessResult = new GuessResult()
                {
                    IsGuessSuccessful = true,
                    IncorrectGuessHints = incorrectGuessHints,
                    ResultMessage = guessResult,
                },
            };
        }

        private IncorrectGuessHints GetIncorrectGuessHints(string guess, string worldle)
        {
            var lettersPresentAndInCorrectPosition = new List<char>();
            var lettersPresentButNotInCorrectPosition = new List<char>();
            var lettersNotPresentInGuess = new List<char>();
            var letterPositionsPresentAndInCorrectPosition = new List<int>();
            var letterPositionsPresentButNotInCorrectPosition = new List<int>();
            var letterPositionsNotPresentInGuess = new List<int>();

            int letterIndex = 0;

            foreach (var letter in guess)
            {
                bool letterPresentInWordle = wordle.Contains(letter);
                bool letterPresentInWordleAndInCorrectPosition = guess[letterIndex].Equals(wordle[letterIndex]);

                if (letterPresentInWordleAndInCorrectPosition)
                {
                    lettersPresentAndInCorrectPosition.Add(letter);
                    letterPositionsPresentAndInCorrectPosition.Add(letterIndex);
                }
                else if (letterPresentInWordle)
                {
                    lettersPresentButNotInCorrectPosition.Add(letter);
                    letterPositionsPresentButNotInCorrectPosition.Add(letterIndex);
                }
                else
                {
                    lettersNotPresentInGuess.Add(letter);
                    letterPositionsNotPresentInGuess.Add(letterIndex);
                }

                letterIndex++;
            }

            return new IncorrectGuessHints()
            {
                LettersPresentInGuessAndInCorrectPosition = lettersPresentAndInCorrectPosition,
                LettersPresentInGuessButNotInCorrectPosition = lettersPresentButNotInCorrectPosition,
                LettersNotPresentInGuess = lettersNotPresentInGuess,
                LetterPositionsPresentInGuessAndInCorrectPosition = letterPositionsPresentAndInCorrectPosition,
                LetterPositionsPresentInGuessButNotInCorrectPosition = letterPositionsPresentButNotInCorrectPosition,
                LetterPositionsNotPresentInGuess = letterPositionsNotPresentInGuess,
            };
        }

        private void GetSelectedWordle()
        {
            var possibleWordles = GetPossibleWordles();

            var random = new Random();
            int wordlePostion = random.Next(0, possibleWordles.Count);

            wordle = possibleWordles[wordlePostion];
        }

        private List<string> GetPossibleWordles()
        {
            //IConfigurationRoot config = new ConfigurationBuilder()
            //        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName)
            //        .AddJsonFile("appSettings.json")
            //        .Build();

            //var possibleWordlesInput = config.
            //                            GetSection("PosssibleWordles")
            //                            .Get<List<string>>();

            var possibleWordlesInput = new List<string>()
            {
                "point",
                "weary",
                "bless",
                "start",
                "curry",
                "pores",
                "polar",
                "never",
                "newer",
                "magic",
                "farce",
                "blank",
                "force",
                "watch",
                "match"
            };

            return possibleWordlesInput
                                    .GroupBy(x => x)
                                    .Select(x => x.Key)
                                    .ToList()
                                    .OrderBy(x => x)
                                    .ToList();
        }
    }
}