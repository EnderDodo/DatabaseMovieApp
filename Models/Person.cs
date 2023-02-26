namespace DatabaseMovieApp.Models;

public class Person
{
    public int Id { get; set; }
    public string IdImdb { get; set; }
    public string Name { get; set; }
    public string? Category { get; set; }
    
    public HashSet<Movie> Movies { get; set; }
    
}