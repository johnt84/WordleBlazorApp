namespace WordleBlazorServerApp.Shared
{
    public class GuessResult
    {
        public bool IsGuessSuccessful { get; set; }
        public string ResultMessage { get; set; } = string.Empty;
        public List<string> ErrorMessages { get; set; }
        public IncorrectGuessHints? IncorrectGuessHints { get; set; }
    }
}