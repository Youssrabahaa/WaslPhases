namespace phase_1.BLL.Services
{
    public interface ICaseAiAssistService
    {
        Task<string?> SuggestTitleAsync(string description);
    }
}
