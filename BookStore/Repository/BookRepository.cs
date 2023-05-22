using BookStore.Data;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BookStore.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _dataContext;

        public BookRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool BookExists(int id)
        {
            return _dataContext.Books.Where(i=>i.Id== id).Any();
        }

        public bool CreateBook(Book book)
        {
            
            _dataContext.Add(book);
            return Save();
        }

        public bool DeleteBook(Book book)
        {
            _dataContext.Remove(book);
            return Save();
        }

        public async Task<Book> GetBook(int id)
        {
            return await _dataContext.Books.Where(i => i.Id == id).Include(a => a.Author).FirstOrDefaultAsync();
        }

        public async Task<Book> GetBookNoTracking(int id)
        {
            return await _dataContext.Books.Where(i => i.Id == id).Include(a => a.Author).AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooks(Expression<Func<Book, bool>> pred=null)
        {
            if (pred==null) return await _dataContext.Books.Include(a => a.Author).ToListAsync();
            else return await _dataContext.Books.Where(pred).Include(b=>b.Author).ToListAsync();               
        }

        
        public bool Save()
        {
            var saved=_dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateBook(Book book)
        {
            _dataContext.Update(book);
            return Save();
        }
    }
}
