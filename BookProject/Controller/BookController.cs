using BookProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


[ApiController]
[Route("api/[Controller]")]
public class BooksController(BookService bookService) : ControllerBase
{
    private readonly BookService _bookService = bookService;

    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        var books = _bookService.GetAllBooks();
        return Ok(books);
    }

    [HttpGet("{isbn}")]
    public ActionResult<Book> Get(string isbn)
    {
        var book = _bookService.GetBookByISBN(isbn);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    public ActionResult Add([FromBody] Book book)
    {
        _bookService.AddBook(book);
        return Ok();
    }

    [HttpPut]
    public ActionResult Update([FromBody] Book book)
    {
        _bookService.UpdateBook(book);
        return Ok();
    }

    [HttpDelete("{isbn}")]
    public ActionResult Delete(string isbn)
    {
        _bookService.DeleteBook(isbn);
        return Ok();
    }

    [HttpGet("report")]
    public ActionResult<string> GetReport()
    {
        var report = _bookService.GenerateHtmlReport();
        return Content(report, "text/html");
    }
}
