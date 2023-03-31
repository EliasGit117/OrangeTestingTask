using System.Text.Json.Serialization;

namespace Movies.Domain.Dtos;

public class MovieDto
{
    public int Id { get; set; }
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
    public bool Adult { get; set; }
    public string Overview { get; set; }
    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; }
    [JsonPropertyName("original_title")]
    public string OriginalTitle { get; set; }
    [JsonPropertyName("original_language")]
    public string OriginalLanguage { get; set; }
    public string Title { get; set; }
    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }
    public double Popularity { get; set; }
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
    public bool Video { get; set; }
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }
}