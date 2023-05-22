using Refit;
using SimpleUI.Models;

namespace SimpleUI.DataAccess
{
    public interface IBookData
    {
        [Get("/Book/{bookId}")]
        Task<BookModel> GetBook(int bookId);

        [Get("/Book/bookSearch")]
        Task<List<BookModel>> GetBooks(string str);

        [Post("/Book")]
        Task<BookModel> CreateBook([Body] BookModel book);

        [Put("/Book/{bookId}")]
        Task<BookModel> UpdateBook(int bookId, [Body] BookModel book);

        [Delete("/Book/{bookId}")]
        Task<string> DeleteBook(int bookId);
    }
}
