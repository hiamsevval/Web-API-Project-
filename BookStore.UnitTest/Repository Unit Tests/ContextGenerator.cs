using BookStore.Data;
using Microsoft.EntityFrameworkCore;


namespace BookStore.UnitTest
{
    public class ContextGenerator
    {
        public static DataContext Generate()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new DataContext(optionsBuilder.Options);
        }

    }
}
