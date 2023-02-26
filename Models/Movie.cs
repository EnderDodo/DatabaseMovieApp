namespace DatabaseMovieApp.Models;

public class Movie
{
    public int Id { get; set; }
    public string? IdImdb { get; set; }
    public HashSet<Person>? Persons { get; set; }
    public HashSet<Tag>? Tags { get; set; }
    public float Rating { get; set; }
    public List<Title>? Titles { get; set; }
    public string? OriginalTitle { get; set; }
    
    public List<Movie>? Top { get; set; }
}