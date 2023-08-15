using LINQ;

LinqQueries queries = new();
//IEnumerable<Book>? books = queries.GetBooksByCategory("Python");
dynamic books = queries.GetBooksWithBasicInformation(3);

//LinqQueries.Animals();

PrintBooks(books);



static void PrintBooks(IEnumerable<Book>? books)
{
  Console.WriteLine("{0,-60} {1, 15} {2, 15}\n", "Title", "N. Pages", "Published Date");
  books?.ToList().ForEach(book => Console.WriteLine(book.ToString()));
}

