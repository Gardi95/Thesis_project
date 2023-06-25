using DapperAPI.dbServices.Tables;
using DapperAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DapperAPI.ThesisTools;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace DapperAPI.Controllers
{
    [ApiController]
    [Route("book")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksData _booksData;
        private Books book;

        private int numberOfItemsToDelete = 0;
        private static int currentIndexUpdateMethod = 0;

        public BooksController(IBooksData booksData)
        {
            _booksData = booksData;
        }

        [HttpGet]
        public async Task<ActionResult<CallResults>> getBooks()
        {
            int startTick = Environment.TickCount;
            CallResults results = new CallResults();

            List<Books> list = await _booksData.getAllBooks();
            results.data = list;

            
            int endTick = Environment.TickCount;
            int elapsedTime = endTick - startTick;
            results.stats.milliseconds = elapsedTime ;

            return results;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CallResults>> getBookById(int id)
        {

            CallResults results = new CallResults();
            int startTick = Environment.TickCount;

            Books? book = await _booksData.getBook(id);

            if (book == null || book == new Books())
            {
                return NotFound($"book with id: {id} was not found");
            }
            results.data = new List<Books> { book };

            int endTick = Environment.TickCount;
            int elapsedTime = endTick - startTick;
            results.stats.milliseconds = elapsedTime ;
            return results;
        }

        [HttpPost]
        public async Task<ActionResult<CallResults>> insertBook(Books book)
        {
            string m = "All Book insert ";

            CallResults results = new CallResults();
            int startTick = Environment.TickCount;

            int id = await _booksData.insertBook(book);


            int endTick = Environment.TickCount;
            int elapsedTime = endTick - startTick;
            results.stats.milliseconds = elapsedTime;

            book.Id = id;
            results.data = new List<Books> { book };

            return results;

        }


        [HttpDelete]
        public async Task<ActionResult<CallResults>> deleteAllBooks()
        {
            List<Books> books = await _booksData.getAllBooks();

            foreach (Books book in books)
            {
                await _booksData.deleteBookById(book.Id);
            }

            return Ok();
        }


        [HttpPut]
        public async Task<ActionResult<CallResults>> UpdateTitle()
        {
            var books = await _booksData.getAllBooks();

            if (books == null || books.Count == 0)
                return NotFound();


            if(currentIndexUpdateMethod >= books.Count)
            {
                currentIndexUpdateMethod = 0;
                return NoContent();
            }

            var bookToUpdate = books[currentIndexUpdateMethod];
            bookToUpdate.Title = "A Game  Thrones";
            bookToUpdate.Author = "George R. R. Martin";
            bookToUpdate.ISBN = "AudioBook";
            bookToUpdate.Publisher = "Bantam Spectra (US) Voyager Books (UK)";
            bookToUpdate.Description = "A Game of Thrones is the first novel in A Song " +
                "of Ice and Fire, a series of fantasy novels by American author George R. R. " +
                "Martin. It was first published on August 1, 1996. The novel won the 1997 " +
                "Locus Award and was nominated for both the 1997 Nebula Award and the 1997 " +
                "World Fantasy Award.";
            bookToUpdate.ShelfLocation = "Northern Ireland ";


            await _booksData.updateBook(bookToUpdate);

            currentIndexUpdateMethod++;
            return NoContent();
        }





    }
}

