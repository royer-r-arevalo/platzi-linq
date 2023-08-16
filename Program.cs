using System.Collections.Generic;
using LINQ;

LinqQueries queries = new();
IEnumerable<Book>? books = queries.GetBooksByCategory("Python");
//dynamic books = queries.GetBooksWithBasicInformation(3);

//LinqQueries.Animals();

//PrintBooks(books);

IEnumerable<IGrouping<int, Book>> groups = queries.GetBooksByYear(2000);
//PrintBooksByYear(groups);

//queries.AnimalsGropyByColor();

ILookup<char, Book> dictionary = queries.GetBookDictionary();
//PrintDictionary(dictionary, 'S');


IEnumerable<Book> books2 = queries.GetBooksJoin();
PrintBooks(books2);

static void PrintDictionary(ILookup<char, Book> dictionary, char letter)
{
  PrintBooks(dictionary[letter]);
}

static void PrintBooks(IEnumerable<Book>? books)
{
  Console.WriteLine("{0,-60} {1, 15} {2, 15}\n", "Title", "N. Pages", "Published Date");
  books?.ToList().ForEach(book => Console.WriteLine(book.ToString()));
}

static void PrintBooksByYear(IEnumerable<IGrouping<int, Book>> groups)
{
  foreach (var group in groups)
  {
    Console.WriteLine($"...............{group.Key}...............\n");
    foreach (var book in group)
    {
      Console.WriteLine("{0,-60} {1, 15} {2, 15}", book.Title, book.PageCount, book.PublishedDate.Value.ToString("yyyy-MM-dd"));
    }
  }
}

