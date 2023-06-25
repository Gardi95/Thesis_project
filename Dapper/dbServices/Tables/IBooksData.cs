using DapperAPI.Models;

namespace DapperAPI.dbServices.Tables
{
    public interface IBooksData
    {
        Task<List<Books>> getAllBooks();
        Task<Books> getBook(int id);
        Task<List<Books>> getBooksByAuthor(string author);
        Task<List<Books>> getBooksByTitle(string title);
        Task<int> insertBook(Books book);
        Task<bool> deleteBookById(int id);
        Task<bool> updateBook(Books book);
        Task updateBook();
    }
}
