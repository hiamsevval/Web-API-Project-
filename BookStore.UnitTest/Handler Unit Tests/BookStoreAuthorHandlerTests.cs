using AutoFixture;
using AutoMapper;
using BookStore.DTO;
using BookStore.Handlers;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Queries;
using Moq;
using System.Linq.Expressions;

namespace BookStore.UnitTest
{
    public class BookStoreAuthorHandlerTests
    {
        private Mock<IAuthorRepository> _authorRepositoryMock;
        private IMapper _mapper;
        private Fixture fixture;
        
        public BookStoreAuthorHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            fixture = new Fixture();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<Helpers.MappingProfiles>();
            });
            _mapper=mapperConfig.CreateMapper();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        
        
        [Test]
        public async Task AuthorHandler_GetAuthor_ReturnsAuthor()
        {   //Arrange
           
            var handler = new GetAuthorHandler(_authorRepositoryMock.Object, _mapper);

            var author = fixture.Build<Author>().Create();
            
            _authorRepositoryMock.Setup(r => r.GetAuthor(author.Id)).ReturnsAsync(author);

            //Act
            var result = await handler.Handle(new GetAuthorQuery(author.Id), CancellationToken.None);

            //Assert
            Assert.IsInstanceOf<AuthorDto>(result);

        }

        
        [Test]
        public async Task AuthorHandler_GetAuthor_ReturnsAuthor_Null()
        {
            var handler = new GetAuthorHandler(_authorRepositoryMock.Object, _mapper);

            var author = fixture.Build<Author>().Create();

            _authorRepositoryMock.Setup(r => r.GetAuthor(author.Id)).ReturnsAsync(author);

            //Act
            var result = await handler.Handle(new GetAuthorQuery(0), CancellationToken.None);

            //Assert
            Assert.IsNull(result);

        }

        
        [Test]
        public async Task AuthorHandler_GetAuthors_ReturnsAuthors_Successfully()
        {
            var handler = new GetAuthorsHandler(_authorRepositoryMock.Object, _mapper);

            var authors = fixture.Build<Author>().CreateMany(3).ToList();

            string str = "a";
            str=str.Trim().ToUpper();
            _authorRepositoryMock.Setup(r => r.GetAuthors(It.IsAny<Expression<Func<Author, bool>>>()))
                .ReturnsAsync((Expression<Func<Author, bool>> predicate) => authors.Where(x => x.Name.Trim().ToUpper().Contains(str)));
            
            //Act
            var result = await handler.Handle(new GetAuthorsQuery(str), CancellationToken.None);
            
            //Assert
            Assert.AreNotEqual(0, result.Count());

        }

        [Test]
        public async Task AuthorHandler_GetAuthors_ReturnsAuthors_Empty()
        {
            var handler = new GetAuthorsHandler(_authorRepositoryMock.Object, _mapper);

            var authors = fixture.Build<Author>().CreateMany(3).ToList();

            string str = "ð";
            str = str.Trim().ToUpper();
            _authorRepositoryMock.Setup(r => r.GetAuthors(It.IsAny<Expression<Func<Author, bool>>>()))
                .ReturnsAsync((Expression<Func<Author, bool>> predicate) => authors.Where(x => x.Name.Trim().ToUpper().Contains(str)));


            //Act
            var result = await handler.Handle(new GetAuthorsQuery(str), CancellationToken.None);

            //Assert
            Assert.IsEmpty(result);

        }

        [Test]
        public async Task AuthorHandler_GetAuthors_ReturnsAuthors_String_Is_Empty()
        {
            var handler = new GetAuthorsHandler(_authorRepositoryMock.Object, _mapper);

            var authors = fixture.Build<Author>().CreateMany(3).ToList();

            string str = null;
            
            _authorRepositoryMock.Setup(r => r.GetAuthors(null))
                .ReturnsAsync(authors);


            //Act
            var result = await handler.Handle(new GetAuthorsQuery(str), CancellationToken.None);

            //Assert
            Assert.IsInstanceOf<List<AuthorDto>>(result);

        }


        [Test]
        public async Task AuthorHandler_CreateAuthor_Successfully()
        {   //Arrange
            var handler = new CreateAuthorCommandHandler(_mapper,_authorRepositoryMock.Object);
            
            _authorRepositoryMock.Setup(r => r.CreateAuthor(It.IsAny<Author>())).Returns(true);
            
            //Act
            var result = await handler.Handle(new Commands.CreateAuthorCommand("Halide Edip",  "Adývar"), CancellationToken.None);

            //Assert
            Assert.IsInstanceOf<Author>(result);
        }

        [Test]
        public async Task AuthorHandler_UpdateAuthor_Successfully()
        {   //Arrange
            var handler = new UpdateAuthorCommandHandler(_authorRepositoryMock.Object, _mapper);

            _authorRepositoryMock.Setup(r => r.UpdateAuthor(It.IsAny<Author>())).Returns(true);

            var author = fixture.Build<Author>().Create();
            
            _authorRepositoryMock.Setup(r => r.GetAuthorNoTracking(author.Id)).ReturnsAsync(author);
            //Act
            var result = await handler.Handle(new Commands.UpdateAuthorCommand(author.Id,"Halide Edip", "Adývar"), 
                                                CancellationToken.None);

            //Assert
            Assert.IsInstanceOf<AuthorDto>(result);
        }


        [Test]
        public async Task AuthorHandler_DeleteAuthor_Successfully()
        {   //Arrange
            var handler = new DeleteAuthorCommandHandler(_authorRepositoryMock.Object);

            _authorRepositoryMock.Setup(r => r.DeleteAuthor(It.IsAny<Author>())).Returns(true);

            var author = fixture.Build<Author>().Create();

            _authorRepositoryMock.Setup(r => r.GetAuthorNoTracking(author.Id)).ReturnsAsync(author);
            //Act
            var result = await handler.Handle(new Commands.DeleteAuthorCommand(author.Id),
                                                CancellationToken.None);

            //Assert
            Assert.AreEqual("Successfully deleted", result);
        }

        [Test]
        public async Task AuthorHandler_DeleteAuthor_Author_Does_Not_Exist()
        {   //Arrange
            var handler = new DeleteAuthorCommandHandler(_authorRepositoryMock.Object);


            _authorRepositoryMock.Setup(r => r.DeleteAuthor(It.IsAny<Author>())).Returns(true);

            var author = fixture.Build<Author>().Create();

            _authorRepositoryMock.Setup(r => r.GetAuthorNoTracking(author.Id)).ReturnsAsync(author);
            //Act
            var result = await handler.Handle(new Commands.DeleteAuthorCommand(35),
                                                CancellationToken.None);

            //Assert
            Assert.AreEqual("Author does not exist", result);
        }
    }
}