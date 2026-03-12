using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using WordleBlazorApp.Shared;
using WordleGameEngine;

namespace WordleBlazorServerApp.Pages;

public partial class Wordle
{
    private GameState gameState = new GameState();
    private GameGrid gameGrid = new GameGrid();

    private List<string>? errorMessages = null;

    private string GuessCharacter { get; set; } = string.Empty;

    private MudTextField<string>[,] InputFields { get; set; } = null!;

    private bool[,] InputFieldsEnabled { get; set; } = null!;

    private string GetTextStyle(int rowIndex, int colIndex) =>
        $"{GetPlayerColour(rowIndex, colIndex)}; text-align: center;";

    private string GetPlayerColour(int rowIndex, int colIndex) =>
        gameGrid.IncorrectGuessHintColours[rowIndex, colIndex];

    private bool GetIsFieldDisabled(int rowIndex, int colIndex) => 
        GetIsGameComplete() || !InputFieldsEnabled[rowIndex, colIndex];

    private bool GetIsGameComplete() => gameState != null && gameState.IsGameComplete;

    protected override async Task OnInitializedAsync()
    {
        InputFields = new MudTextField<string>[GameEngine.NUMBER_OF_ALLLOWED_GUESSES, GameEngine.WORDLE_LENGTH];
        InputFieldsEnabled = new bool[GameEngine.NUMBER_OF_ALLLOWED_GUESSES, GameEngine.WORDLE_LENGTH];

        await NewGameAsync();
    }
        
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        await InputFields[0, 0].FocusAsync();
    }

    private async Task OnTextEntryAsync(string newValue, int rowIndex, int colIndex)
    {
        GuessCharacter = newValue;
        
        gameGrid.Guesses[rowIndex, colIndex] = GuessCharacter;

        if (colIndex == GameEngine.WORDLE_LENGTH - 1)
        {
            return;
        }

        await InputFields[rowIndex, colIndex + 1].FocusAsync();
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs e, int rowIndex, int colIndex)
    {
        const string Enter = nameof(Enter);
        const string Backspace = nameof(Backspace);

        if (e.Key != Enter && e.Key != Backspace)
        {
            return;
        }

        if (e.Key == Enter)
        {
            await EnterGuessAsync();
        }
        else
        {
            await HandleBackspaceAsync(rowIndex, colIndex);
        }
    }

    private async Task EnterGuessAsync()
    {
        string currentGuess = string.Empty;

        for (int colIndex = 0; colIndex < GameEngine.WORDLE_LENGTH; colIndex++)
        {
            currentGuess += gameGrid.Guesses[gameEngine.GetNumberOfGuesses(), colIndex];
        }

        gameState = gameEngine.EnterGuess(currentGuess);

        errorMessages = gameState?.GuessResult?.ErrorMessages;

        gameGrid = gameEngine.GetGameGrid();

        if (gameState.IsGameComplete)
            return;

        for (int colIndex = 0; colIndex < GameEngine.WORDLE_LENGTH; colIndex++)
        {
            InputFieldsEnabled[gameState.NumberOfGuesses, colIndex] = true;
        }

        await InputFields[gameState.NumberOfGuesses, 0].FocusAsync();
    }

    private async Task NewGameAsync()
    {
        gameState = gameEngine.NewGame();
        gameGrid = gameEngine.GetGameGrid();

        for (int rowIndex = 0; rowIndex < GameEngine.NUMBER_OF_ALLLOWED_GUESSES; rowIndex++)
        {
            for (int colIndex = 0; colIndex < GameEngine.WORDLE_LENGTH; colIndex++)
            {
                InputFieldsEnabled[rowIndex, colIndex] = rowIndex == 0;
            }
        }
    }

    private async Task HandleBackspaceAsync(int rowIndex, int colIndex)
    {
        if (!string.IsNullOrEmpty(gameGrid.Guesses[rowIndex, colIndex]) || colIndex == 0)
        {
            return;
        }

        var previousIndex = colIndex - 1;

        gameGrid.Guesses[rowIndex, previousIndex] = string.Empty;
        await InputFields[rowIndex, previousIndex].FocusAsync();
    }
}
