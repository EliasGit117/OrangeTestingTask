using System.Text.Json.Serialization;

namespace Movies.Domain.Common;

public class PaginatedResponse<T>
{
    [JsonPropertyName("page")]
    public int Page { get; set; } = 1;
    
    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; } = 0;
    
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; } = 1;
    
    [JsonPropertyName("results")]
    public IEnumerable<T> Results { get; set; } = new List<T>();
    
    public int? ItemsPerPage { get; set; }
}