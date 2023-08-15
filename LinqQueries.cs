using System.Collections.Generic;
using System.Text.Json;

namespace LINQ
{
  public class LinqQueries
  {
    private readonly List<Book> _books;
    private const string FILE_PATH = "books.json";

    public LinqQueries()
    {
      _books = new List<Book>();
      if (File.Exists(FILE_PATH))
      {
        using var reader = new StreamReader(FILE_PATH);
        string json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        List<Book>? books = JsonSerializer.Deserialize<List<Book>>(json, options);
        if (books != null && books.Any())
        {
          _books = books;
        }
      }
    }

    public IEnumerable<Book> GetBooks() => _books;

    public IEnumerable<Book> GetCustomFilter(int? pageCount = null, int? year = null, string? title = null)
    {
      IEnumerable<Book> books = _books;
      if (pageCount != null)
        books = books.Where(book => book.PageCount > pageCount.Value);
      if (year != null)
        books = books.Where(book => book?.PublishedDate?.Year > year.Value);
      if (!string.IsNullOrEmpty(title))
        books = books.Where(book => book.Title.Contains(title));
      return books;
    }

    /// <summary>
    /// All or TrueForAll is when a condition is sucessfull for all items in the collection.
    /// </summary>
    /// <returns></returns>
    public bool AllBooksHasStatus() => _books.All(book => string.IsNullOrEmpty(book.Status));
    public bool AllBooksHasStatus2() => _books.TrueForAll(book => string.IsNullOrEmpty(book.Status));

    /// <summary>
    /// Any or Exists is when a condition is sucessful for a unless a item in the collection.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public bool ThereIsABookPublishedIn(int year) => _books.Any(book => book.PublishedDate.Value.Year == year);
    public bool ThereIsABookPublishedIn2(int year) => _books.Exists(book => book.PublishedDate.Value.Year == year);

    /// <summary>
    /// The Contains Operator allow us to search a value in a sub collection of us collection.
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public IEnumerable<Book> GetBooksByCategory(string category) => _books.Where(book => book.Categories.Contains(category));

    public IEnumerable<Book> GetBooksByCategoryOrderByAscending(string category)
    {
      return _books.Where(book => book.Categories.Contains(category)).OrderBy(book => book.Title);
    }

    public IEnumerable<Book> GetBooksByPageCountOrderByDescending(int pageCount)
    {
      return _books.Where(book => book.PageCount > pageCount).OrderByDescending(book => book.PageCount);
    }

    /// <summary>
    /// Take Operator
    /// </summary>
    /// <param name="countBooksToShow"></param>
    /// <param name="category"></param>
    /// <returns></returns>
    public IEnumerable<Book> GetBooksPublishedRecently(int countBooksToShow, string category)
    {
      return _books.Where(book => book.Categories.Contains(category))
                  .OrderByDescending(book => book.PublishedDate) 
                  .Take(countBooksToShow);
    }

    /// <summary>
    /// Skip Operator
    /// </summary>
    /// <param name="pageCount"></param>
    /// <returns></returns>
    public IEnumerable<Book> GetThirdAndQuaterBook(int pageCount)
    {
      return _books.Where(book => book?.PageCount > pageCount) 
                  .Take(4)
                  .Skip(2);
    }

    /// <summary>
    /// Select Operator
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public IEnumerable<dynamic> GetBooksWithBasicInformation(int count) 
    {
      return _books.Take(count).Select(book => new { book.Title, book.PageCount });
    }

    public static void Animals()
    {
      List<Animal> animals = new()
       {
           new Animal() { Name = "Hormiga", Color = "Rojo" },
           new Animal() { Name = "Lobo", Color = "Gris" },
           new Animal() { Name = "Elefante", Color = "Gris" },
           new Animal() { Name = "Pantegra", Color = "Negro" },
           new Animal() { Name = "Gato", Color = "Negro" },
           new Animal() { Name = "Iguana", Color = "Verde" },
           new Animal() { Name = "Sapo", Color = "Verde" },
           new Animal() { Name = "Camaleon", Color = "Verde" },
           new Animal() { Name = "Gallina", Color = "Blanco" }
       };

      char[] vocals = new[] { 'A', 'E', 'I', 'O', 'U' };
      animals = animals.Where(animal => animal.Color.Equals("Verde") && vocals.Contains(animal.Name[0])).OrderBy(animal => animal.Name).ToList();

      Console.WriteLine("{0,-20} {1, 10}\n", "Animal", "Color");
      animals.ForEach(animal => Console.WriteLine(animal.ToString()));
    }
  }
}