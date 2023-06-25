using Dapper;
using DapperAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace DapperAPI.dbServices.Tables
{
    public class BooksData : IBooksData
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly IConfiguration _config;


        public BooksData(ISqlDataAccess sqlDataAccess, IConfiguration config)
        {
            _sqlDataAccess = sqlDataAccess;
            _config = config;

        }

        public async Task<List<Books>> getAllBooks()
        {
            string sql = "select * from books";

            return await _sqlDataAccess.LoadMany<Books>(sql);
        }

        public async Task<Books> getBook(int id)
        {
            string sql = $"select * from books where id = {id}";

            return await _sqlDataAccess.LoadSingle<Books>(sql);
        }

        public async Task<List<Books>> getBooksByAuthor(string author)
        {
            string sql = $"select * from books where author LIKE '%{author}%'";

            return await _sqlDataAccess.LoadMany<Books>(sql);
        }

        public async Task<List<Books>> getBooksByTitle(string title)
        {
            string sql = $"select * from books where Title = '{title}'";

            return await _sqlDataAccess.LoadMany<Books>(sql);
        }

        public async Task<int> insertBook(Books book)
        {
            //string sql = $"INSERT INTO Books VALUES ('{book.Id}', {book.Title}', '{book.Author}', '{book.ISBN}',  '{book.PublicationDate.ToString("yyyy-MM-dd")}', '{book.Publisher}' , {book.pages} , '{book.Description}' , '{book.ShelfLocation}')";
            var sql = "INSERT INTO Books output inserted.id VALUES ( @Title, @Author, @ISBN, @PublicationDate, @Publisher, @Pages, @Description, @ShelfLocation)";
            var parameters = new DynamicParameters();
            parameters.Add("Title", book.Title, DbType.String);
            parameters.Add("Author", book.Author, DbType.String);
            parameters.Add("ISBN", book.ISBN, DbType.String);
            parameters.Add("PublicationDate", book.PublicationDate, DbType.Date);
            parameters.Add("Publisher", book.Publisher, DbType.String);
            parameters.Add("Pages", book.pages, DbType.Int32);
            parameters.Add("Description", book.Description, DbType.String);
            parameters.Add("ShelfLocation", book.ShelfLocation, DbType.String);
            string connectionString = _config.GetConnectionString("LibraryDb");
            var connection =new SqlConnection(connectionString);
            var result=   await connection.ExecuteScalarAsync<int>(sql, parameters);
            return result;
   
        }

        public async Task<bool> insertManyBooks(List<Books> books)
        {
            string sql = string.Empty;

            foreach (Books book in books)
            {
                sql = (string)sql.Concat($"INSERT INTO Books VALUES ('{book.Id}', '{book.Title}', '{book.Author}', '{book.ISBN}',  '{book.PublicationDate.ToString("yyyy-MM-dd")}', '{book.Publisher}' , {book.pages} , '{book.Description}' , '{book.ShelfLocation}');\n");
            }

            return await _sqlDataAccess.insertData(sql);
        }

        public async Task<bool> deleteBookById(int id)
        {
            string sql = $"delete from Books where id = {id}";

            return await _sqlDataAccess.insertData(sql);
        }

        public async Task<bool> updateBook(Books book)
        {
            string sql = $"update books set title = '{book.Title}', author = '{book.Author}', ISBN = '{book.ISBN}', publisher = '{book.Publisher}', pages = {book.pages}, description = '{book.Description}', shelfLocation = '{book.ShelfLocation}' where id = {book.Id}";

            return await _sqlDataAccess.insertData(sql);
        }

        Task IBooksData.updateBook()
        {
            throw new NotImplementedException();
        }
    }
}
