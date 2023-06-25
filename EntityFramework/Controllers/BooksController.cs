using EntityFramework.dbServeicer;
using EntityFramework.Models;
using EntityFramework.ThesisTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly SqlDataAcceess context;
        private static int currentIndexUpdateMethod = 0; 
        private int numberOfItemsToDelete = 0;

        public BooksController(SqlDataAcceess context) => this.context = context;


        [HttpGet]
        public async Task<ActionResult<CallResults>> Get()
        {
            int startTick = Environment.TickCount;
            CallResults results = new CallResults();

            var book = await context.Books.ToListAsync();
            results.data = book;

            int endTick = Environment.TickCount;
            int elapsedTime = endTick - startTick;
            results.stats.milliseconds = elapsedTime;


            return results;

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CallResults), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CallResults>> GetById(int id)
        {
            CallResults results = new CallResults();
            int startTick = Environment.TickCount;

            var book = await context.Books.FindAsync(id);

            int endTick = Environment.TickCount;
            int elapsedTime = endTick - startTick;
            results.stats.milliseconds = elapsedTime;

            if (book == null || book == new Books())
            {
                return NotFound($"book with id: {id} was not found");
            }

            results.data = new List<Books> { book };

            return Ok(results);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CallResults>> Create(Books book)
        {
            var entity = (await context.Books.AddAsync(book)).Entity;
            await context.SaveChangesAsync();
            CreatedAtAction(nameof(GetById), new { id = entity.Id }, book);
            return Ok();
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CallResults>> UpdateTitle()
        {
            var books = await context.Books.ToListAsync();

            if (books == null || books.Count == 0)
                return NotFound();

            if (currentIndexUpdateMethod >= books.Count)
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

            await context.SaveChangesAsync();

            currentIndexUpdateMethod++; 

            return NoContent();
        }



        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CallResults>> Delete()
        {
            if (numberOfItemsToDelete == 0)
            {
   
                numberOfItemsToDelete = await context.Books.CountAsync();
            }

            if (numberOfItemsToDelete > 0)
            {
         
                var bookToDelete = await context.Books.FindAsync(numberOfItemsToDelete);

                if (bookToDelete != null)
                {
                    context.Books.Remove(bookToDelete);
                    await context.SaveChangesAsync();
                }

                numberOfItemsToDelete--;
            }

            return Ok();
        }


      
    }
}


