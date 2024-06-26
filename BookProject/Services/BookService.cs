using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BookProject.Models;

public class BookService(string filePath)
{
    private readonly string _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath), "File path cannot be null.");

    private List<Book> ReadBooks()
    {
        var books = new List<Book>();

        if (!File.Exists(_filePath)) return books;

        var doc = XDocument.Load(_filePath);
        foreach (var element in doc.Descendants("book"))
        {
            books.Add(new Book
            {
                ISBN = element.Element("isbn")?.Value ?? string.Empty,
                Title = element.Element("title")?.Value ?? string.Empty,
                Cover = element.Attribute("cover")?.Value ?? string.Empty,
                Authors = string.Join(", ", element.Elements("author").Select(a => a.Value)),
                Category = element.Attribute("category")?.Value ?? string.Empty,
                Price = element.Element("price")?.Value ?? string.Empty
            });
        }

        return books;
    }

    private void WriteBooks(List<Book> books)
    {
        var doc = new XDocument(new XElement("bookstore",
            books.Select(b => new XElement("book",
                new XAttribute("category", b.Category),
                new XAttribute("cover", b.Cover),
                new XElement("isbn", b.ISBN),
                new XElement("title", b.Title),
                b.Authors.Split(", ").Select(a => new XElement("author", a)),
                new XElement("price", b.Price)
            ))
        ));
        doc.Save(_filePath);
    }

    public IEnumerable<Book> GetAllBooks()
    {
        Console.WriteLine(_filePath);
        return ReadBooks();
    }

    public Book? GetBookByISBN(string isbn)
    {
        return ReadBooks().FirstOrDefault(b => b.ISBN == isbn);
    }

    public void AddBook(Book book)
    {
        var books = ReadBooks();
        books.Add(book);
        WriteBooks(books);
    }

    public void UpdateBook(Book book)
    {
        var books = ReadBooks();
        var existingBook = books.FirstOrDefault(b => b.ISBN == book.ISBN);
        if (existingBook != null)
        {
            books.Remove(existingBook);
            books.Add(book);
            WriteBooks(books);
        }
    }

    public void DeleteBook(string isbn)
    {
        var books = ReadBooks();
        var book = books.FirstOrDefault(b => b.ISBN == isbn);
        if (book != null)
        {
            books.Remove(book);
            WriteBooks(books);
        }
    }

    public string GenerateHtmlReport()
    {
        var books = ReadBooks();
        var sb = new StringBuilder();
        sb.Append("<html><body><h1>Book Report</h1><table border='1'>");
        sb.Append("<tr><th>ISBN</th><th>Title</th><th>Authors</th><th>Category</th><th>Cover</th><th>Price</th></tr>");
        
        foreach (var book in books)
        {
            sb.Append("<tr>");
            sb.Append($"<td>{book.ISBN}</td>");
            sb.Append($"<td>{book.Title}</td>");
            sb.Append($"<td>{book.Authors}</td>");
            sb.Append($"<td>{book.Category}</td>");
            sb.Append($"<td>{book.Cover}</td>");
            sb.Append($"<td>{book.Price}</td>");
            sb.Append("</tr>");
        }

        sb.Append("</table></body></html>");
        return sb.ToString();
    }
}
