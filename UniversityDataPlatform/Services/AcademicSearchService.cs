using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniversityDataPlatform.Services
{
    public class AcademicSearchService
    {
        private readonly HttpClient _httpClient;

        public AcademicSearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<bool> IsUrlWorkingAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            try
            {
                using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromMilliseconds(1500));
                using var request = new HttpRequestMessage(HttpMethod.Head, url);
                request.Headers.Add("User-Agent", "UniversityDataPlatform/1.0 (mailto:samet@karabuk.edu.tr)");
                
                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                
                int statusCode = (int)response.StatusCode;
                bool isSuccess = response.IsSuccessStatusCode || (statusCode >= 300 && statusCode < 400);

                if (!isSuccess)
                {
                    // Fallback to GET since many servers block or return errors (e.g. 403, 405) for HEAD requests
                    using var getRequest = new HttpRequestMessage(HttpMethod.Get, url);
                    getRequest.Headers.Add("User-Agent", "UniversityDataPlatform/1.0 (mailto:samet@karabuk.edu.tr)");
                    using var getCts = new System.Threading.CancellationTokenSource(TimeSpan.FromMilliseconds(1500));
                    var getResponse = await _httpClient.SendAsync(getRequest, HttpCompletionOption.ResponseHeadersRead, getCts.Token);
                    
                    int getStatus = (int)getResponse.StatusCode;
                    return getResponse.IsSuccessStatusCode || (getStatus >= 300 && getStatus < 400);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<AcademicSearchResult> SearchPapersAsync(string query)
        {
            var result = new AcademicSearchResult();
            if (string.IsNullOrWhiteSpace(query))
                return result;

            try
            {
                // Requesting 15 rows from CrossRef to filter and find 4 high-quality papers with valid links
                var url = $"https://api.crossref.org/works?query={Uri.EscapeDataString(query)}&rows=15";
                
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "UniversityDataPlatform/1.0 (mailto:samet@karabuk.edu.tr)");
                request.Headers.Add("Accept", "application/json");

                var response = await _httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = $"Akademik arama servisi geçici olarak yanıt vermedi (Hata {response.StatusCode}).";
                    return result;
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<CrossRefResponse>(new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var candidates = new List<AcademicSearchPaper>();
                if (apiResponse?.Message?.Items != null)
                {
                    foreach (var item in apiResponse.Message.Items)
                    {
                        // Skip items that have no URL and no DOI (cannot link to them)
                        if (string.IsNullOrEmpty(item.DOI) && string.IsNullOrEmpty(item.URL))
                            continue;

                        // Guarantee a working URL link using DOI redirect if URL is empty
                        var paperUrl = !string.IsNullOrEmpty(item.URL) ? item.URL : $"https://doi.org/{item.DOI}";

                        int? pubYear = null;
                        if (item.Created?.DateParts != null && item.Created.DateParts.Any())
                        {
                            var parts = item.Created.DateParts.First();
                            if (parts != null && parts.Any())
                            {
                                pubYear = parts.First();
                            }
                        }

                        var paper = new AcademicSearchPaper
                        {
                            paperId = item.DOI ?? Guid.NewGuid().ToString(),
                            title = item.Title?.FirstOrDefault() ?? "Başlıksız Makale",
                            venue = item.ContainerTitle?.FirstOrDefault() ?? "Akademik Yayın",
                            url = paperUrl,
                            year = pubYear,
                            authors = new List<AcademicSearchAuthor>()
                        };

                        // Map authors list
                        if (item.Author != null)
                        {
                            foreach (var auth in item.Author)
                            {
                                string authorName = $"{auth.Given} {auth.Family}".Trim();
                                if (!string.IsNullOrEmpty(authorName))
                                {
                                    paper.authors.Add(new AcademicSearchAuthor { name = authorName });
                                }
                            }
                        }

                        if (!paper.authors.Any())
                        {
                            paper.authors.Add(new AcademicSearchAuthor { name = "Belirtilmemiş" });
                        }

                        // Map DOI external ID
                        if (!string.IsNullOrEmpty(item.DOI))
                        {
                            paper.externalIds = new AcademicSearchExternalIds
                            {
                                DOI = item.DOI
                            };
                        }

                        candidates.Add(paper);
                    }
                }

                // Verify working URLs in parallel
                var checkTasks = candidates.Select(async p =>
                {
                    bool isWorking = await IsUrlWorkingAsync(p.url);
                    return new { Paper = p, IsWorking = isWorking };
                });

                var checkResults = await Task.WhenAll(checkTasks);

                foreach (var res in checkResults)
                {
                    if (res.IsWorking)
                    {
                        result.Papers.Add(res.Paper);
                        // Only return exactly 4 papers
                        if (result.Papers.Count >= 4)
                            break;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "Akademik arama sunucusuyla bağlantı kurulamadı.";
                Console.WriteLine($"CrossRef Search Error: {ex}");
                return result;
            }
        }
    }

    // --- CROSSREF API RESPONSE OBJECTS ---
    public class CrossRefResponse
    {
        public string Status { get; set; } = "";
        public CrossRefMessage Message { get; set; } = new();
    }

    public class CrossRefMessage
    {
        public List<CrossRefItem> Items { get; set; } = new();
    }

    public class CrossRefItem
    {
        public List<string> Title { get; set; } = new();
        public List<CrossRefAuthor> Author { get; set; } = new();
        public CrossRefDate Created { get; set; } = new();
        
        [JsonPropertyName("container-title")]
        public List<string> ContainerTitle { get; set; } = new();
        
        public string DOI { get; set; } = "";
        public string URL { get; set; } = "";
    }

    public class CrossRefAuthor
    {
        public string Given { get; set; } = "";
        public string Family { get; set; } = "";
    }

    public class CrossRefDate
    {
        [JsonPropertyName("date-parts")]
        public List<List<int>> DateParts { get; set; } = new();
    }

    // --- FRONTEND INTEGRATION COMPATIBILITY MODELS ---
    public class AcademicSearchResult
    {
        public List<AcademicSearchPaper> Papers { get; set; } = new();
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; } = "";
        public bool IsRateLimited { get; set; } = false;
    }

    public class AcademicSearchResponse
    {
        public List<AcademicSearchPaper> data { get; set; } = new();
    }

    public class AcademicSearchPaper
    {
        public string paperId { get; set; } = "";
        public string title { get; set; } = "";
        public string venue { get; set; } = "";
        public int? year { get; set; }
        public string url { get; set; } = "";
        public List<AcademicSearchAuthor> authors { get; set; } = new();
        public AcademicSearchExternalIds externalIds { get; set; } = new();
    }

    public class AcademicSearchAuthor
    {
        public string name { get; set; } = "";
    }

    public class AcademicSearchExternalIds
    {
        public string DOI { get; set; } = "";
    }
}
