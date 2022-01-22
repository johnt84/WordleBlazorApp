using Microsoft.Extensions.Configuration;
using Shared;

namespace WorldleGameEngine
{
    public class GameEngine : IGameEngine
    {
        public const int NUMBER_OF_ALLLOWED_GUESSES = 6;
        public const int WORDLE_LENGTH = 5;
        private int numberOfGuesses = 0;

        private string wordle = string.Empty;

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

        public GameState EnterGuess(string guess)
        {
            if (string.IsNullOrEmpty(guess) || guess.Length != WORDLE_LENGTH || guess.Any(char.IsDigit))
            {
                string errorMessage = string.Empty;

                if(string.IsNullOrEmpty(guess))
                {
                    errorMessage = "No guess entered";
                }
                else if(guess.Length != WORDLE_LENGTH)
                {
                    errorMessage = $"The guess entered has not got enough letters.  Guess should have {WORDLE_LENGTH} letters";
                }
                else
                {
                    errorMessage = "The guess entered is invalid as it contains numbers";
                }
                
                return new GameState()
                {
                    IsGameComplete = false,
                    HasPlayerWonGame = false,
                    NumberOfGuesses = numberOfGuesses,
                    GuessResult = new GuessResult()
                    {
                        IsGuessSuccessful = false,
                        IncorrectGuessHints = null,
                        ResultMessage = errorMessage,
                    },
                };
            }

            string guessResult = string.Empty;
            IncorrectGuessHints? incorrectGuessHints = null;
            
            string guessInLowerCase = guess.ToLower();

            numberOfGuesses++;

            if (guessInLowerCase.Equals(wordle))
            {
                return PlayerWonGame(guessInLowerCase);
            }
            else
            {
                if (numberOfGuesses == NUMBER_OF_ALLLOWED_GUESSES)
                {
                    return GameOver(guessInLowerCase);
                }
                else
                {
                    incorrectGuessHints = GetIncorrectGuessHints(guessInLowerCase, wordle);
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

        private GameState PlayerWonGame(string guess)
        {
            string es = numberOfGuesses > 1 ? "es" : string.Empty;

            var positionsInGuess = new List<int>();

            for(int i = 0; i < WORDLE_LENGTH; i++)
            {
                positionsInGuess.Add(i);
            }

            return new GameState()
            {
                IsGameComplete = true,
                HasPlayerWonGame = true,
                NumberOfGuesses = numberOfGuesses,
                GuessResult = new GuessResult()
                {
                    IsGuessSuccessful = false,
                    IncorrectGuessHints = new IncorrectGuessHints()
                    {
                        LettersPresentInGuessAndInCorrectPosition = guess.ToCharArray().ToList(),
                        LetterPositionsPresentInGuessAndInCorrectPosition = positionsInGuess,
                    },
                    ResultMessage = $"Congratulations you correctly guessed the selected wordle { wordle } in { numberOfGuesses} guess{ es}",
                },
            };
        }

        private GameState GameOver(string guess)
        {
            return new GameState()
            {
                IsGameComplete = true,
                HasPlayerWonGame = false,
                NumberOfGuesses = numberOfGuesses,
                GuessResult = new GuessResult()
                {
                    IsGuessSuccessful = false,
                    IncorrectGuessHints = GetIncorrectGuessHints(guess, wordle),
                    ResultMessage = $"Unlucky you didn't guess the selected wordle {wordle}",
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

        public int GetNumberOfGuesses()
        {
            return numberOfGuesses;
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
                "match",
                "fuzzy",
                "agent",
                "prick",
                "chair",
                "child",
                "adult",
                "cycle",
                "fight",
                "issue",
                "knife",
                "money",
                "model",
                "motor",
                "pilot",
                "pound",
                "shape",
                "total",
                "white",
                "woman",
                "youth"
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
