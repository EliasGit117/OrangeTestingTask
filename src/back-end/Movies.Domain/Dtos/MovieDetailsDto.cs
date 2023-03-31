using System.Text.Json.Serialization;

namespace Movies.Domain.Dtos;

public class MovieDetailsDto
{
    public int Id { get; set; }
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
    public dynamic? BelongsToCollection { get; set; }
    public int Budget { get; set; }
    public string? HomePage { get; set; }
    [JsonPropertyName("imdb_id")]
    public string ImdbId { get; set; }
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
    [JsonPropertyName("production_companies")]
    public IEnumerable<ProductionCompany> ProductionCompanies { get; set; }
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
    public bool Video { get; set; }
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }
    public string? Tagline { get; set; }
    
    public bool? IsFav { get; set; }
}

public class ProductionCompany
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("logo_path")]
    public string LogoPath { get; set; }
    [JsonPropertyName("origin_country")]
    public string OriginCountry { get; set; }
}

public class ProductionCountries
{
    public string ReleaseDate { get; set; }
    public int Revenue { get; set; }
    public int? Runtime { get; set; }
}

public class SpokenLanguages
{
    [JsonPropertyName("iso_639_1")]
    public string? ISO639 { get; set; }
    public string Name { get; set; }
}

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
}