using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System.Text.Json;

namespace SchoolManagementMAUI.Services
{
    public class ApiPublicCatalogService : IPublicCatalogService
    {
        private readonly HttpClient _client;
        private const string ApiBaseUrl = "http://keniasoaresapi.somee.com/api";
        
        private static readonly string[] PossibleApiUrls = {
            "http://keniasoaresapi.somee.com/api",         // API online 
            "https://keniasoaresapi.somee.com/api"         // API online HTTPS
        };
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiPublicCatalogService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
                UseProxy = false, 
                UseCookies = false 
            };
            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "SchoolManagementMAUI/1.0");
            _client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _client.Timeout = TimeSpan.FromSeconds(60); 
            
            // Configura para usar HTTP se for preciso
            _client.DefaultRequestVersion = new Version(1, 1);
        }

        public async Task<IReadOnlyList<Course>> GetCoursesAsync()
        {
            foreach (var baseUrl in PossibleApiUrls)
            {
                try
                {
                    var url = $"{baseUrl}/public/courses";
                    var resp = await _client.GetAsync(url);
                    
                    if (!resp.IsSuccessStatusCode) 
                    {
                        continue; 
                    }
                    
                    var json = await resp.Content.ReadAsStringAsync();
                    // multiplas resposta items, results, courses, data
                    var list = ParseList<Course>(json, "items", "results", "courses", "data");
                    return list;
                }
                catch (Exception)
                {
                    continue; 
                }
            }
            
            return Array.Empty<Course>();
        }

        public async Task<IReadOnlyList<StudentClass>> GetClassesAsync(int? courseId = null, string? year = null, string? shift = null)
        {
            var qp = new List<string>();
            if (courseId.HasValue) qp.Add($"courseId={courseId.Value}");
            if (!string.IsNullOrWhiteSpace(year)) qp.Add($"year={Uri.EscapeDataString(year)}");
            if (!string.IsNullOrWhiteSpace(shift)) qp.Add($"shift={Uri.EscapeDataString(shift)}");
            var qs = qp.Count > 0 ? "?" + string.Join("&", qp) : string.Empty;
            var resp = await _client.GetAsync($"{ApiBaseUrl}/public/classes{qs}");
            if (!resp.IsSuccessStatusCode) return Array.Empty<StudentClass>();
            var json = await resp.Content.ReadAsStringAsync();
            var list = ParseList<StudentClass>(json, "items", "results", "classes", "data");
            return list;
        }

        public async Task<Course?> GetCourseAsync(int id)
        {
            var resp = await _client.GetAsync($"{ApiBaseUrl}/public/courses/{id}");
            if (!resp.IsSuccessStatusCode) return null;
            var json = await resp.Content.ReadAsStringAsync();
            try { return JsonSerializer.Deserialize<Course>(json, JsonOptions); } catch { }
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    if (doc.RootElement.TryGetProperty("course", out var c))
                        return c.Deserialize<Course>(JsonOptions);
                }
            }
            catch { }
            return null;
        }

        public async Task<StudentClass?> GetClassAsync(int id)
        {
            var resp = await _client.GetAsync($"{ApiBaseUrl}/public/classes/{id}");
            if (!resp.IsSuccessStatusCode) return null;
            var json = await resp.Content.ReadAsStringAsync();
            try { return JsonSerializer.Deserialize<StudentClass>(json, JsonOptions); } catch { }
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    if (doc.RootElement.TryGetProperty("class", out var c))
                        return c.Deserialize<StudentClass>(JsonOptions);
                }
            }
            catch { }
            return null;
        }

        public async Task<IReadOnlyList<PublicSubject>> GetSubjectsAsync()
        {
            var resp = await _client.GetAsync($"{ApiBaseUrl}/public/subjects");
            if (!resp.IsSuccessStatusCode) return Array.Empty<PublicSubject>();
            var json = await resp.Content.ReadAsStringAsync();
            var list = ParseList<PublicSubject>(json, "items", "results", "subjects", "data");
            return list;
        }

        public async Task<IReadOnlyList<PublicSubject>> GetCourseSubjectsAsync(int courseId)
        {
            var resp = await _client.GetAsync($"{ApiBaseUrl}/public/courses/{courseId}/subjects");
            if (!resp.IsSuccessStatusCode) return Array.Empty<PublicSubject>();
            var json = await resp.Content.ReadAsStringAsync();
            var list = ParseList<PublicSubject>(json, "items", "results", "subjects", "data");
            return list;
        }

        private static IReadOnlyList<T> ParseList<T>(string json, params string[] arrayPropertyNames)
        {
            try
            {
                // Array direto
                var direct = JsonSerializer.Deserialize<List<T>>(json, JsonOptions);
                if (direct != null)
                    return direct;
            }
            catch {}

            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (var name in arrayPropertyNames)
                    {
                        if (doc.RootElement.TryGetProperty(name, out var arr) ||
                            doc.RootElement.TryGetProperty(name.ToLower(), out arr) ||
                            doc.RootElement.TryGetProperty(name.ToUpper(), out arr))
                        {
                            if (arr.ValueKind == JsonValueKind.Array)
                            {
                                var list = new List<T>();
                                foreach (var item in arr.EnumerateArray())
                                {
                                    var obj = item.Deserialize<T>(JsonOptions);
                                    if (obj != null) list.Add(obj);
                                }
                                return list;
                            }
                        }
                    }
                }
            }
            catch { }
            return Array.Empty<T>();
        }
    }
}
