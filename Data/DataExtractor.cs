using DatabaseMovieApp.Models;
using IMDbApiLib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DatabaseMovieApp.Data;

public class DataExtractor
{
    private readonly DatabaseContext _context;
    private const string ApiKey = "3ccd902a";
    private const string ApiUrl = $"https://www.omdbapi.com/?apikey={ApiKey}&";

    public DataExtractor()
    {
        _context = new DatabaseContext();
    }

    #region GetByText
    public List<Movie> GetMoviesByText(string text)
    {
        return _context.Movies
            .Where(item =>
                item.Titles != null && item.Titles.Any(title => title.Name.ToLower().Contains(text.ToLower())))
            .OrderByDescending(item => item.Rating)
            .ToList();
    }
    public List<Tag> GetTagsByText(string text)
    {
        return _context.Tags
            .Where(item => item.Name.Contains(text))
            .OrderByDescending(item => item.Movies.Count)
            .ToList();
    }
    public List<Person> GetPersonsByText(string text)
    {
        return _context.Persons
            .Where(item => item.Name.Contains(text))
            .OrderByDescending(item => item.Movies.Count)
            .ToList();
    }
    #endregion
    
    #region GetById
    public Movie? GetMovieById(int movieId)
    {
        return _context.Movies
            .Where(item => item.Id == movieId)
            .Include(item => item.Tags)
            .Include(item => item.Persons)
            .Include(item => item.Top)
            .ThenInclude(item => item.Tags)
            .Include(item => item.Top)
            .ThenInclude(item => item.Persons)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefault();
    }
    public Tag? GetTagById(int tagId)
    {
        return _context.Tags
            .FirstOrDefault(tag => tag.Id == tagId);
    }
    public Person? GetPersonById(int personId)
    {
        return _context.Persons
            .FirstOrDefault(person => person.Id == personId);
    }
    #endregion

    public List<Movie> GetMoviesByTagName(string tagName)
    {
        return _context.Movies
            .Where(m => m.Tags.Any(t => t.Name == tagName))
            .Include(m => m.Tags)
            .Include(m => m.Persons)
            .AsNoTracking()
            .ToList();
    }
    public List<Movie?> GetMoviesByPersonName(string name)
    {
        return _context.Movies
            .Where(item => item.Persons.Any(person => person.Name == name))
            .Include(item => item.Tags)
            .Include(item => item.Persons)
            .AsNoTracking()
            .ToList();
    }

    public async Task<MovieImdb?> GetImdbMovieByIdImdbAsync(string idImdb)
    {
        var url = ApiUrl + "&i=" + idImdb;

        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url)
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<MovieImdb>(jsonString);
    }

    public async Task<string?> GetPersonImageLinkByNameAsync(string name)
    {
        var apiLib = new ApiLib("k_h74ph619");

        var data = await apiLib.SearchNameAsync(name.Replace(" ", "%20"));

        return data.Results.FirstOrDefault()?.Image;
    }
}