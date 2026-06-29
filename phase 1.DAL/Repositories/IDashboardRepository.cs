namespace phase_1.Repositories
{
    // Repository مخصص لتجميع الإحصائيات من جداول متعددة (Case, Match, Session, Review, Report, NoShowStrike)
    // ده الاستثناء الوحيد اللي بيقرا من جداول فريق تاني، لكنه قراءة فقط (Read-Only) ومفيش فيه أي Business Logic
    public interface IDashboardRepository
    {
        Task<int> GetTotalCasesAsync();
        Task<int> GetActiveMatchesCountAsync();
        Task<int> GetCompletedMatchesCountAsync();
        Task<int> GetCancelledMatchesCountAsync();
        Task<int> GetTotalSessionsAsync();
        Task<int> GetCompletedSessionsCountAsync();
        Task<int> GetTotalNoShowStrikesAsync();
    }
}
