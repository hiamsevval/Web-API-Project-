using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class DataContext : DbContext 
    {
        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options) : base(options) {
        
        }

        public DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        
    }
}
