using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace LINQ
{
  public class LinqQueries
  {
    private readonly List<Book> _books;
    private readonly List<Animal> _animals;
    private const string FILE_PATH = "books.json";

    public LinqQueries()
    {
      _animals = new()
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
      _books = new();
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
    public IEnumerable<Book> GetBooksWithBasicInformation(int count)
    {
      return _books.Take(count).Select(book => new Book
      {
        Title = book.Title,
        PageCount = book.PageCount
      });
    }

    /// <summary>
    /// Count Operator
    /// </summary>
    /// <param name="rangeOne"></param>
    /// <param name="rangeTwo"></param>
    /// <returns></returns>
    public int GetCountBooksBetweenRangePages(int rangeOne, int rangeTwo)
    {
      return _books.Count(book => book.PageCount >= rangeOne && book.PageCount <= rangeTwo);
    }

    public DateTime? GetMinPublishedDate() => _books.Min(book => book.PublishedDate);
    public int? GetMaxPageCount() => _books.Max(book => book.PageCount);
    public Book? GetBookWithMinPages() => _books.Where(book => book.PageCount > 0).MinBy(book => book.PageCount);
    public Book? GetBookWithPublishedDateReciently() => _books.MaxBy(book => book.PublishedDate);
    public int? GetSumBookPagesBetweenPageCount(int rageOne, int rangeTwo)
    {
      return _books.Where(book => book.PageCount >= rageOne && book.PageCount <= rangeTwo)
                   .Sum(book => book.PageCount);
    }


    public string GetBookTitlesSince(int year = 2015)
    => string.Join(" - ", _books.Where(book => book.PublishedDate?.Year > year).Select(book => book.Title));

    public string GetBookTitlesSince2(int year = 2015)
    => _books.Where(book => book.PublishedDate?.Year > year)
             .Aggregate("", (acum, next) => !string.IsNullOrEmpty(acum) ? $" - {next.Title}" : next.Title);

    public double GetAvarageTitleLengthCharacteres() => _books.Average(book => book.Title.Length);

    public IEnumerable<IGrouping<int, Book>> GetBooksByYear(int minYear = 2000)
    => _books.Where(book => book?.PublishedDate?.Year >= minYear)
             .GroupBy(book => book.PublishedDate.Value.Year);

    public ILookup<char,Book> GetBookDictionary() 
    => _books.ToLookup(book => book.Title[0], p => p);

    public IEnumerable<Book> GetBooksJoin()
    {
      return _books.Where(book => book.PageCount > 500)
      .Join(_books.Where(book2 => book2.PublishedDate.Value.Year > 2005), book => book.Title, book2 => book2.Title, (book1, book2) => book1);
    }

    public void Animals()
    {
      List<Animal> animals = _animals;
      char[] vocals = new[] { 'A', 'E', 'I', 'O', 'U' };
      animals = animals.Where(animal => animal.Color.Equals("Verde") && vocals.Contains(animal.Name[0])).OrderBy(animal => animal.Name).ToList();

      Console.WriteLine("{0,-20} {1, 10}\n", "Animal", "Color");
      animals.ForEach(animal => Console.WriteLine(animal.ToString()));
    }

    public void AnimalsGropyByColor() 
    {
      IEnumerable<IGrouping<string?, Animal>> groups = _animals.GroupBy(animal => animal.Color);
      foreach (var group in groups)
      {
        Console.WriteLine($".........................{group.Key}........................");
        foreach (var animal in group)
        {
           Console.WriteLine("{0,-20} {1, 10}", animal.Name, animal.Color);
        }
      }
    }
  }
}