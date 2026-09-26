using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace CopilotTokenTracker
{
    internal sealed record CopilotUsage(decimal Quantity, decimal GrossAmount, decimal IncludedRequests);

    internal sealed class GitHubUsageClient : IDisposable
    {
        private readonly HttpClient _http;

        public GitHubUsageClient(string pat)
        {
            _http = new HttpClient { BaseAddress = new Uri("https://api.github.com/") };
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pat);
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            _http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("copilot-token-poller", "1.0"));
            _http.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        }

        public async Task<CopilotUsage> GetCopilotUsageAsync(string user, CancellationToken token)
        {
            var now = DateTimeOffset.UtcNow;
            var url = $"users/{Uri.EscapeDataString(user)}/settings/billing/usage?year={now.Year}&month={now.Month}";

            using var response = await _http.GetAsync(url, token);
            response.EnsureSuccessStatusCode();

            var report = await response.Content.ReadFromJsonAsync<UsageReport>(cancellationToken: token)
                         ?? throw new InvalidOperationException("Empty usage report.");

            var copilot = report.UsageItems
                .Where(i => i.Product.Contains("copilot", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var included = copilot.Sum(i => i.GrossAmount > 0m
                ? i.Quantity * (i.DiscountAmount / i.GrossAmount)
                : i.Quantity);

            return new CopilotUsage(
                copilot.Sum(i => i.Quantity),
                copilot.Sum(i => i.GrossAmount),
                included);
        }

        public void Dispose() => _http.Dispose();
    }

    internal sealed record UsageReport
    {
        [JsonPropertyName("usageItems")]
        public List<UsageItem> UsageItems { get; init; } = [];
    }

    internal sealed record UsageItem
    {
        [JsonPropertyName("product")]
        public string Product { get; init; } = "";

        [JsonPropertyName("sku")]
        public string Sku { get; init; } = "";

        [JsonPropertyName("quantity")]
        public decimal Quantity { get; init; }

        [JsonPropertyName("grossAmount")]
        public decimal GrossAmount { get; init; }

        [JsonPropertyName("discountAmount")]
        public decimal DiscountAmount { get; init; }

        [JsonPropertyName("netAmount")]
        public decimal NetAmount { get; init; }
    }
}
