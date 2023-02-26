namespace DatabaseMovieApp.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public HashSet<Movie> Movies { get; set; }
}