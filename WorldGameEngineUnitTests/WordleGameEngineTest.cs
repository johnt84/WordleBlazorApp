namespace WordleUnitTests
{
    [TestClass]
    public class WordleGameEngineTest
    {
        private Mock<IWordleGenerator> wordleGenerator = new Mock<IWordleGenerator>();
        private GameEngine gameEngine = null;
        private string correctWordle = "brain";
        private string invalidGuessWithNoMatchingLetters = "queue";
        int oneGuess = 1;

        [TestInitialize]
        public void Init()
        {
            wordleGenerator.Setup(x => x.GenerateSelectedWordle()).Returns(correctWordle);

            gameEngine = new GameEngine(wordleGenerator.Object);

            gameEngine.NewGame();
        }

        [TestMethod]
        public void WhenEmptyGuessEntered_ThenNoGuessEnteredError()
        {
            var gameState = gameEngine.EnterGuess("");

            Assert.IsNotNull(gameState);
            Assert.IsFalse(gameState.IsGameComplete);
            Assert.IsFalse(gameState.HasPlayerWonGame);
            Assert.AreEqual(0, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsFalse(gameState.GuessResult.IsGuessSuccessful);
            Assert.IsNull(gameState.GuessResult.IncorrectGuessHints);
            Assert.AreEqual(1, gameState.GuessResult.ErrorMessages.Count);
            Assert.AreEqual("No guess entered", gameState.GuessResult.ErrorMessages.First());
        }

        [TestMethod]
        public void WhenGuessNotLongEnough_ThenNoGuessEnteredError()
        {
            var gameState = gameEngine.EnterGuess("brai");

            Assert.IsNotNull(gameState);
            Assert.IsFalse(gameState.IsGameComplete);
            Assert.IsFalse(gameState.HasPlayerWonGame);
            Assert.AreEqual(0, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsFalse(gameState.GuessResult.IsGuessSuccessful);
            Assert.IsNull(gameState.GuessResult.IncorrectGuessHints);
            Assert.AreEqual(1, gameState.GuessResult.ErrorMessages.Count);
            Assert.AreEqual($"The guess entered has not got enough letters.  Guess should have {correctWordle.Length} letters", gameState.GuessResult.ErrorMessages.First());
        }

        [TestMethod]
        public void WhenGuessContainsANumericCharacter_ThenGuessContainsNumbersErrorReturned()
        {
            var gameState = gameEngine.EnterGuess("brai0");

            Assert.IsNotNull(gameState);
            Assert.IsFalse(gameState.IsGameComplete);
            Assert.IsFalse(gameState.HasPlayerWonGame);
            Assert.AreEqual(0, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsFalse(gameState.GuessResult.IsGuessSuccessful);
            Assert.IsNull(gameState.GuessResult.IncorrectGuessHints);
            Assert.AreEqual(1, gameState.GuessResult.ErrorMessages.Count);
            Assert.AreEqual("The guess entered is invalid as it contains numbers", gameState.GuessResult.ErrorMessages.First());
        }

        [TestMethod]
        public void WhenCorrectGuessEnteredOn1stAttempt_ThenGameWonByPlayerOn1stAttempt()
        {
            var gameState = gameEngine.EnterGuess(correctWordle);

            Assert.IsNotNull(gameState);
            Assert.IsTrue(gameState.IsGameComplete);
            Assert.IsTrue(gameState.HasPlayerWonGame);
            Assert.AreEqual(oneGuess, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsTrue(gameState.GuessResult.IsGuessSuccessful);
            Assert.AreEqual(correctWordle.Length, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition.Count);
            Assert.AreEqual(correctWordle, string.Join("", gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition));
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition.Count);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess.Count);
            Assert.AreEqual($"Congratulations you correctly guessed the selected wordle of {correctWordle} in {oneGuess} guess", gameState.GuessResult.ResultMessage);
        }

        [TestMethod]
        public void WhenCorrectGuessEnteredOn2ndAttempt_ThenGameWonByPlayerOn1stAttempt()
        {
            gameEngine.EnterGuess(invalidGuessWithNoMatchingLetters);

            var gameState = gameEngine.EnterGuess(correctWordle);

            int numberOfGuesses = 2;

            Assert.IsNotNull(gameState);
            Assert.IsTrue(gameState.IsGameComplete);
            Assert.IsTrue(gameState.HasPlayerWonGame);
            Assert.AreEqual(numberOfGuesses, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsTrue(gameState.GuessResult.IsGuessSuccessful);
            Assert.AreEqual(correctWordle.Length, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition.Count);
            Assert.AreEqual(correctWordle, string.Join("", gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition));
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition.Count);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess.Count);
            Assert.AreEqual($"Congratulations you correctly guessed the selected wordle of {correctWordle} in {numberOfGuesses} guesses", gameState.GuessResult.ResultMessage);
        }

        [TestMethod]
        public void WhenIncorrectCorrectGuessEnteredWithNoCorrectLetter_ThenGameNotWonByPlayerAndGameNotCompleteAndNoCorrectLetters()
        {
            var gameState = gameEngine.EnterGuess(invalidGuessWithNoMatchingLetters);

            Assert.IsNotNull(gameState);
            Assert.IsFalse(gameState.IsGameComplete);
            Assert.IsFalse(gameState.HasPlayerWonGame);
            Assert.AreEqual(oneGuess, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsTrue(gameState.GuessResult.IsGuessSuccessful);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition.Count);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition.Count);
            Assert.AreEqual(invalidGuessWithNoMatchingLetters.Length, gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess.Count);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition.Count);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition.Count);
            Assert.AreEqual(invalidGuessWithNoMatchingLetters.Length, gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess.Count);
            Assert.AreEqual(invalidGuessWithNoMatchingLetters, string.Join("", gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess));
        }

        [TestMethod]
        public void WhenIncorrectCorrectGuessEnteredWithOneCorrectLetterButInWrongPosition_ThenGameNotWonByPlayerAndGameNotCompleteAndOneCorrectLetterButInWrongPosition()
        {
            string invalidGuess = "force";

            var gameState = gameEngine.EnterGuess(invalidGuess);

            Assert.IsNotNull(gameState);
            Assert.IsFalse(gameState.IsGameComplete);
            Assert.IsFalse(gameState.HasPlayerWonGame);
            Assert.AreEqual(oneGuess, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsTrue(gameState.GuessResult.IsGuessSuccessful);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition.Count);
            Assert.AreEqual(1, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition.Count);
            Assert.AreEqual(invalidGuess[2], gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition[0]);
            Assert.AreEqual(invalidGuess.Length - 1, gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess.Count);
            Assert.AreEqual("foce", string.Join("", gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess));
        }

        [TestMethod]
        public void WhenIncorrectCorrectGuessEnteredWithOneCorrectLetterInCorrectPosition_ThenGameNotWonByPlayerAndGameNotCompleteAndOneCorrectLetterInCorrectPosition()
        {
            string invalidGuess = "stack";

            var gameState = gameEngine.EnterGuess(invalidGuess);

            Assert.IsNotNull(gameState);
            Assert.IsFalse(gameState.IsGameComplete);
            Assert.IsFalse(gameState.HasPlayerWonGame);
            Assert.AreEqual(oneGuess, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsTrue(gameState.GuessResult.IsGuessSuccessful);
            Assert.AreEqual(1, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition.Count);
            Assert.AreEqual(invalidGuess[2], gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition[0]);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition.Count);
            Assert.AreEqual(invalidGuess.Length - 1, gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess.Count);
            Assert.AreEqual("stck", string.Join("", gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess));
        }

        [TestMethod]
        public void WhenIncorrectCorrectGuessEnteredWithOneCorrectLetterInCorrectPositionAndOneCorrectLetterButInWrongPosition_ThenGameNotWonByPlayerAndGameNotCompleteAndOneLetterInCorrectPositionAndOneCorrectLetterButInWrongPosition()
        {
            string invalidGuess = "trend";

            var gameState = gameEngine.EnterGuess(invalidGuess);

            Assert.IsNotNull(gameState);
            Assert.IsFalse(gameState.IsGameComplete);
            Assert.IsFalse(gameState.HasPlayerWonGame);
            Assert.AreEqual(oneGuess, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsTrue(gameState.GuessResult.IsGuessSuccessful);
            Assert.AreEqual(1, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition.Count);
            Assert.AreEqual(invalidGuess[1], gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition[0]);
            Assert.AreEqual(1, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition.Count);
            Assert.AreEqual(invalidGuess[3], gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition[0]);
            Assert.AreEqual(invalidGuess.Length - 2, gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess.Count);
            Assert.AreEqual("ted", string.Join("", gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess));
        }

        [TestMethod]
        public void WhenIncorrectCorrectGuessEnteredAndNumberOfAllowedAttemptsAlreadyMade_ThenGameNotWonByPlayerAndGameOver()
        {
            gameEngine.NewGame();

            int numberOfAllowedGuesses = 6;

            for (int i = 0; i < numberOfAllowedGuesses - 1; i++)
            {
                gameEngine.EnterGuess(invalidGuessWithNoMatchingLetters);
            }

            var gameState = gameEngine.EnterGuess(invalidGuessWithNoMatchingLetters);

            Assert.IsNotNull(gameState);
            Assert.IsTrue(gameState.IsGameComplete);
            Assert.IsFalse(gameState.HasPlayerWonGame);
            Assert.AreEqual(numberOfAllowedGuesses, gameState.NumberOfGuesses);
            Assert.IsNotNull(gameState.GuessResult);
            Assert.IsFalse(gameState.GuessResult.IsGuessSuccessful);
            Assert.AreEqual(invalidGuessWithNoMatchingLetters.Length, gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess.Count);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessAndInCorrectPosition.Count);
            Assert.AreEqual(0, gameState.GuessResult.IncorrectGuessHints?.LettersPresentInGuessButNotInCorrectPosition.Count);
            Assert.AreEqual(invalidGuessWithNoMatchingLetters.Length, gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess.Count);
            Assert.AreEqual(invalidGuessWithNoMatchingLetters, string.Join("", gameState.GuessResult.IncorrectGuessHints?.LettersNotPresentInGuess));
            Assert.AreEqual($"Unlucky you didn't guess the selected wordle of {correctWordle}", gameState.GuessResult.ResultMessage);
        }
    }
}