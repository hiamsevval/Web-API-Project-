using AutoFixture;
using BookStore.Data;
using BookStore.Models;
using BookStore.Repository;


namespace BookStore.UnitTest
{
    public class BookStoreAuthorRepositoryTests
    {
        private Fixture fixture;
        private AuthorRepository repository;
        private readonly DataContext context;
        

        public BookStoreAuthorRepositoryTests()
        {
            fixture= new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            context = ContextGenerator.Generate();
            repository = new AuthorRepository(context);
        }


        [Test]
        public async Task GetAuthor_Returns_Author_Successfully()
        {
            // Arrange
            
            var author = fixture.Build<Author>().Create();
            

            context.Add(author);
            context.SaveChanges();

            //Act
            var result = repository.GetAuthor(author.Id);

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetAuthorNoTracking_Returns_Author_Successfully()
        {
            // Arrange

            var author = fixture.Build<Author>().Create();


            context.Add(author);
            context.SaveChanges();

            //Act
            var result = repository.GetAuthorNoTracking(author.Id);

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task CreateAuthor_Returns_True()
        {
            //Arrange
            var author = fixture.Build<Author>().Create();

            //Act
            var result = repository.CreateAuthor(author);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAuthor_Returns_True()
        {
            //Arrange
            var author = fixture.Build<Author>().Create();
            repository.CreateAuthor(author);

            //Act
            var result = repository.UpdateAuthor(author);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAuthor_Returns_True()
        {
            //Arrange
            var author = fixture.Build<Author>().Create();
            repository.CreateAuthor(author);

            //Act
            var result = repository.DeleteAuthor(author);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AuthorExists_Returns_True()
        {
            //Arrange
            var author = fixture.Build<Author>().Create();
            repository.CreateAuthor(author);

            //Act
            var result = repository.AuthorExists(author.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAuthors_Returns_Authors_Successfully()
        {
            //Arrange
            var authors = fixture.Build<Author>().CreateMany(4);
            for(int i=0; i<authors.Count(); i++)
            {
                repository.CreateAuthor(authors.ElementAt(i));
            }
            string str = "a";
            str=str.Trim().ToUpper();

            //Act
            var result = await repository.GetAuthors(x=>x.Name.Trim().ToUpper().Contains(str));

            //Assert
            Assert.AreNotEqual(0, result.Count());
        }

        [Test]
        public async Task GetAuthors_Returns_All_Authors_Predicate_Is_Null()
        {
            //Arrange
            var authors = fixture.Build<Author>().CreateMany(4);
            for (int i = 0; i < authors.Count(); i++)
            {
                repository.CreateAuthor(authors.ElementAt(i));
            }

            //Act
            var result = await repository.GetAuthors(null);

            //Assert
            Assert.AreNotEqual(0, result.Count());
        }

    }
}
