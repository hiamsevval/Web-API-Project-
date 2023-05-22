using AutoFixture;
using BookStore.Data;
using BookStore.Models;
using BookStore.Repository;


namespace BookStore.UnitTest
{
    public class BookStoreBookRepositoryTests
    {
        private Fixture fixture;
        private BookRepository repository;
        private readonly DataContext context;


        public BookStoreBookRepositoryTests()
        {
            fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            context = ContextGenerator.Generate();
            repository = new BookRepository(context);
        }


        [Test]
        public async Task GetBook_Returns_Book_Successfully()
        {
            // Arrange

            var book = fixture.Build<Book>().Create();


            context.Add(book);
            context.SaveChanges();

            //Act
            var result = repository.GetBook(book.Id);

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetBookNoTracking_Returns_Book_Successfully()
        {
            // Arrange

            var book = fixture.Build<Book>().Create();


            context.Add(book);
            context.SaveChanges();

            //Act
            var result = repository.GetBookNoTracking(book.Id);

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task CreateBook_Returns_True()
        {
            //Arrange
            var book = fixture.Build<Book>().Create();

            //Act
            var result = repository.CreateBook(book);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateBook_Returns_True()
        {
            //Arrange
            var book = fixture.Build<Book>().Create();
            repository.CreateBook(book);

            //Act
            var result = repository.UpdateBook(book);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteBook_Returns_True()
        {
            //Arrange
            var book = fixture.Build<Book>().Create();
            repository.CreateBook(book);

            //Act
            var result = repository.DeleteBook(book);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task BookExists_Returns_True()
        {
            //Arrange
            var book = fixture.Build<Book>().Create();
            repository.CreateBook(book);

            //Act
            var result = repository.BookExists(book.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetBook_Returns_Books_Successfully()
        {
            //Arrange
            var books = fixture.Build<Book>().CreateMany(4);
            for (int i = 0; i < books.Count(); i++)
            {
                repository.CreateBook(books.ElementAt(i));
            }
            string str = "a";
            str = str.Trim().ToUpper();

            //Act
            var result = await repository.GetBooks(x => x.Name.Trim().ToUpper().Contains(str));

            //Assert
            Assert.AreNotEqual(0, result.Count());
        }

        [Test]
        public async Task GetBooks_Returns_All_Books_Predicate_Is_Null()
        {
            //Arrange
            var books = fixture.Build<Book>().CreateMany(4);
            for (int i = 0; i < books.Count(); i++)
            {
                repository.CreateBook(books.ElementAt(i));
            }

            //Act
            var result = await repository.GetBooks(null);

            //Assert
            Assert.AreNotEqual(0, result.Count());
        }

    }
}
