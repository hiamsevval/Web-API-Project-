using BookStore.DTO;
using BookStore.Models;
using System.Linq.Expressions;

namespace BookStore.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAuthors(Expression<Func<Author, bool>> pred=null);
        Task<Author> GetAuthor(int id);
        Task<Author> GetAuthorNoTracking(int id);
        bool AuthorExists(int id);
        bool CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(Author author);
        bool Save();
    }
}
