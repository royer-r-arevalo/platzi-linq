using System;
using System.Collections.Generic;

namespace LINQ
{
  public class Book
  {
    public string? Title { get; set; }
    public int? PageCount { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? ShortDescription { get; set; }
    public string? Status { get; set; }
    public string[]? Authors { get; set; }
    public string[]? Categories { get; set; }

    public override string ToString()
    {
      return string.Format("{0,-60} {1, 15} {2, 15}",
        Title,
        PageCount,
        PublishedDate?.ToString("yyyy-MM-dd"));
    }
  }
}

