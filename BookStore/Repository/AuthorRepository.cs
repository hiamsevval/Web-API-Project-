using BookStore.Data;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
        }
        public bool AuthorExists(int id)
        {
            return _context.Authors.Where(i=>i.Id == id).Any();
        }

        public bool CreateAuthor(Author author)
        {
            _context.Add(author);
            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _context.Remove(author);
            return Save();
        }

        public async Task<Author> GetAuthor(int id)
        {
            return await _context.Authors.Where(i => i.Id == id).Include(a => a.Book).FirstOrDefaultAsync();
        }

        public async Task<Author> GetAuthorNoTracking(int id)
        {
            return await _context.Authors.Where(i => i.Id == id).Include(a => a.Book).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthors(Expression<Func<Author, bool>> pred = null)
        {
            if (pred == null) return await _context.Authors.Include(a => a.Book).ToListAsync();
            else return await _context.Authors.Where(pred).Include(b => b.Book).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;    
        }

        public bool UpdateAuthor(Author author)
        {
            _context.Update(author);
            return Save();
        }
    }
}
