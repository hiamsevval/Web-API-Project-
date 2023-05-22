
using BookStore.Models;
using System.Linq.Expressions;

namespace BookStore.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooks(Expression<Func<Book, bool>> pred = null);   
        Task<Book> GetBook(int id);
        Task<Book> GetBookNoTracking(int id);
        bool BookExists(int id);
        bool CreateBook(Book book);
        bool UpdateBook(Book book);
        bool DeleteBook(Book book);
        bool Save();
    }
}
