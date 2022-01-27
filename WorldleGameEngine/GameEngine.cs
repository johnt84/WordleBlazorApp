using WordleBlazorApp.Shared;

namespace WorldleGameEngine
{
    public class GameEngine : IGameEngine
    {
        public const int NUMBER_OF_ALLLOWED_GUESSES = 6;
        public const int WORDLE_LENGTH = 5;
        private int numberOfGuesses = 0;
        private GameGrid gameGrid = new GameGrid();

        private string wordle = string.Empty;

        private readonly List<string> _possibleWordles;

        public int GetNumberOfGuesses() => numberOfGuesses;
        public GameGrid GetGameGrid() => gameGrid;

        public GameEngine(List<string> possibleWordles)
        {
            _possibleWordles = possibleWordles;
        }

        public GameState NewGame()
        {
            GetSelectedWordle();

            numberOfGuesses = 0;

            gameGrid = new GameGrid()
            {
                Guesses = new string[NUMBER_OF_ALLLOWED_GUESSES, WORDLE_LENGTH],
                IncorrectGuessHintColours = new string[NUMBER_OF_ALLLOWED_GUESSES, WORDLE_LENGTH],
            };

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
                return GetInvalidGuessEntry(guess);
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
                    incorrectGuessHints = ManageIncorrectGuessHints(guessInLowerCase, wordle);
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

            var letterPositionsInGuess = new List<int>();

            for (int i = 0; i < WORDLE_LENGTH; i++)
            {
                letterPositionsInGuess.Add(i);
            }

            return new GameState()
            {
                IsGameComplete = true,
                HasPlayerWonGame = true,
                NumberOfGuesses = numberOfGuesses,
                GuessResult = new GuessResult()
                {
                    IsGuessSuccessful = true,
                    IncorrectGuessHints = ManageIncorrectGuessHints(guess, wordle),
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
                    IncorrectGuessHints = ManageIncorrectGuessHints(guess, wordle),
                    ResultMessage = $"Unlucky you didn't guess the selected wordle {wordle}",
                },
            };
        }

        private IncorrectGuessHints ManageIncorrectGuessHints(string guess, string worldle)
        {
            var incorrectGuessHints = GetIncorrectGuessHints(guess, wordle);
            ApplyIncorrectGuessHints(incorrectGuessHints);

            return incorrectGuessHints;
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

            return _possibleWordles
                        .GroupBy(x => x)
                        .Select(x => x.Key)
                        .ToList()
                        .OrderBy(x => x)
                        .ToList();
        }

        private void ApplyIncorrectGuessHints( IncorrectGuessHints incorrectGuessHints)
        {
            if (incorrectGuessHints != null)
            {
                gameGrid.IncorrectGuessHintColours = ApplyIncorrectGuessHintColours(
                                                    gameGrid.IncorrectGuessHintColours
                                                    , incorrectGuessHints.LetterPositionsPresentInGuessAndInCorrectPosition
                                                    , "background-color: #0080009c;");

                gameGrid.IncorrectGuessHintColours = ApplyIncorrectGuessHintColours(
                                                        gameGrid.IncorrectGuessHintColours
                                                        , incorrectGuessHints.LetterPositionsPresentInGuessButNotInCorrectPosition
                                                        , "background-color: yellow;");

                gameGrid.IncorrectGuessHintColours = ApplyIncorrectGuessHintColours(
                                                        gameGrid.IncorrectGuessHintColours
                                                        , incorrectGuessHints.LetterPositionsNotPresentInGuess
                                                        , "background-color: #80808078;");
            }
        }

        private string[,] ApplyIncorrectGuessHintColours(string[,] incorrectGuessHintColours, List<int> letterPositions, string backgroundColour)
        {
            if (letterPositions.Count > 0)
            {
                foreach (int letterPosition in letterPositions)
                {
                    incorrectGuessHintColours[GetNumberOfGuesses() - 1, letterPosition] = backgroundColour;
                }
            }

            return incorrectGuessHintColours;
        }

        private GameState GetInvalidGuessEntry(string guess)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(guess))
            {
                errors.Add("No guess entered");
            }
            else if (guess.Length != WORDLE_LENGTH)
            {
                errors.Add($"The guess entered has not got enough letters.  Guess should have {WORDLE_LENGTH} letters");
            }

            if(guess.Any(char.IsDigit))
            {
                errors.Add("The guess entered is invalid as it contains numbers");
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
                    ErrorMessages = errors,
                },
            };
        }
    }
}
