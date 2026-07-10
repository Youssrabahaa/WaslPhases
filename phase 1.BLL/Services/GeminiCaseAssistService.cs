using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace phase_1.BLL.Services
{
    public class GeminiCaseAssistService : ICaseAiAssistService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiCaseAssistService> _logger;

        public GeminiCaseAssistService(HttpClient httpClient, IConfiguration configuration, ILogger<GeminiCaseAssistService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string?> SuggestTitleAsync(string description)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning("Gemini:ApiKey is not configured; skipping case title suggestion.");
                return null;
            }

            var model = _configuration["Gemini:Model"];
            if (string.IsNullOrWhiteSpace(model))
                model = "gemini-2.5-flash";

            var prompt =
                "مريض وصف حالته الطبية بالنص التالي بين علامتي اقتباس: \"" + description + "\". " +
                "اكتب عنوانًا واحدًا قصيرًا جدًا (أقل من 8 كلمات) باللغة العربية يصلح كاسم مختصر لهذه الحالة الطبية " +
                "في تطبيق حجز مواعيد أسنان. أعد العنوان فقط بدون أي شرح أو علامات تنصيص.";

            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                },
                generationConfig = new
                {
                    thinkingConfig = new { thinkingBudget = 0 }
                }
            };

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            try
            {
                using var response = await _httpClient.PostAsJsonAsync(url, requestBody);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Gemini API returned {StatusCode} while suggesting a case title.", response.StatusCode);
                    return null;
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text?.Trim().Trim('"', '\n', '\r', ' ');
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get a case title suggestion from Gemini.");
                return null;
            }
        }
    }
}
