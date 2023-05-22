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
    public class BookStoreBookHandlerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IAuthorRepository> _authorRepositoryMock;
        private IMapper _mapper;
        private Fixture fixture;

        public BookStoreBookHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            fixture = new Fixture();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<Helpers.MappingProfiles>();
            });
            _mapper = mapperConfig.CreateMapper();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }


        [Test]
        public async Task BookHandler_GetBook_ReturnsBook()
        {   //Arrange

            var handler = new GetBookHandler(_bookRepositoryMock.Object, _mapper);

            var book = fixture.Build<Book>().Create();

            _bookRepositoryMock.Setup(r => r.GetBook(book.Id)).ReturnsAsync(book);

            //Act
            var result = await handler.Handle(new GetBookQuery(book.Id), CancellationToken.None);

            //Assert
            Assert.IsInstanceOf<BookDto>(result);

        }

        
        [Test]
        public async Task BookHandler_GetBook_ReturnsBook_Null()
        {
            var handler = new GetBookHandler(_bookRepositoryMock.Object, _mapper);
          
            var book = fixture.Build<Book>().Create();

            _bookRepositoryMock.Setup(r => r.GetBook(book.Id)).ReturnsAsync(book);

            //Act
            var result = await handler.Handle(new GetBookQuery(0), CancellationToken.None);

            //Assert
            Assert.IsNull(result);

        }

        
        [Test]
        public async Task BookHandler_GetBooks_ReturnsBooks()
        {
            var handler = new GetBooksHandler(_bookRepositoryMock.Object, _mapper);

            var books = fixture.Build<Book>().CreateMany(3).ToList();

            string str = "a";
            str = str.Trim().ToUpper();
            
            _bookRepositoryMock.Setup(r => r.GetBooks(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync((Expression<Func<Book, bool>> predicate) => books.Where(x => x.Name.Trim().ToUpper().Contains(str)));

          

            //Act
            var result = await handler.Handle(new GetBooksQuery(str), CancellationToken.None);

            //Assert
            Assert.AreNotEqual(0, result.Count());

        }

        [Test]
        public async Task BookHandler_GetBooks_ReturnsBooks_Empty()
        {
            var handler = new GetBooksHandler(_bookRepositoryMock.Object, _mapper);

            var books = fixture.Build<Book>().CreateMany(3).ToList();

            string str = "ö";
            str = str.Trim().ToUpper();

            _bookRepositoryMock.Setup(r => r.GetBooks(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync((Expression<Func<Book, bool>> predicate) => books.Where(x => x.Name.Trim().ToUpper().Contains(str)));


            //Act
            var result = await handler.Handle(new GetBooksQuery(str), CancellationToken.None);

            //Assert
            Assert.IsEmpty(result);

        }

        [Test]
        public async Task BookHandler_GetBooks_ReturnsBooks_String_Is_Empty()
        {
            var handler = new GetBooksHandler(_bookRepositoryMock.Object, _mapper);

            var books = fixture.Build<Book>().CreateMany(3).ToList();

            string str = null;

            _bookRepositoryMock.Setup(r => r.GetBooks(null))
                .ReturnsAsync(books);


            //Act
            var result = await handler.Handle(new GetBooksQuery(str), CancellationToken.None);

            //Assert
            Assert.IsInstanceOf<List<BookDto>>(result);

        }

        [Test]
        public async Task BookHandler_CreateBook_Successfully()
        {   //Arrange
            var handler = new CreateBookCommandHandler(_bookRepositoryMock.Object, _authorRepositoryMock.Object, _mapper);

            _bookRepositoryMock.Setup(r => r.CreateBook(It.IsAny<Book>())).Returns(true);
           
            var book = fixture.Build<BookDto>().Create(); 

            //Act
            var result = await handler.Handle(new Commands.CreateBookCommand(book.Name, book.Author, book.PublishYear), CancellationToken.None);

            //Assert
            Assert.IsInstanceOf<BookDto>(result);
        }

    

        [Test]
        public async Task BookHandler_UpdateAuthor_Successfully()
        {   //Arrange
            var handler = new UpdateBookCommandHandler(_bookRepositoryMock.Object, _mapper);


            _bookRepositoryMock.Setup(r => r.UpdateBook(It.IsAny<Book>())).Returns(true);

            var book = fixture.Build<Book>().Create();
            var mappedBook = _mapper.Map<BookDto>(book);

            _bookRepositoryMock.Setup(r => r.GetBookNoTracking(book.Id)).ReturnsAsync(book);
            //Act
            var result = await handler.Handle(new Commands.UpdateBookCommand(book.Id, "Kafamda Bir Tuhaflık", 
                mappedBook.Author, "2016"), CancellationToken.None);

            //Assert
            Assert.IsInstanceOf<BookDto>(result);
        }

        
        [Test]
        public async Task BookHandler_DeleteAuthor_Successfully()
        {   //Arrange
            var handler = new DeleteBookCommandHandler(_bookRepositoryMock.Object);

            _bookRepositoryMock.Setup(r => r.DeleteBook(It.IsAny<Book>())).Returns(true);

            var book = fixture.Build<Book>().Create();

            _bookRepositoryMock.Setup(r => r.GetBookNoTracking(book.Id)).ReturnsAsync(book);
            //Act
            var result = await handler.Handle(new Commands.DeleteBookCommand(book.Id),
                                                CancellationToken.None);

            //Assert
            Assert.AreEqual("Successfully deleted", result);
        }


        [Test]
        public async Task BookHandler_DeleteAuthor_Does_Not_Exist()
        {   //Arrange
            var handler = new DeleteBookCommandHandler(_bookRepositoryMock.Object);

            _bookRepositoryMock.Setup(r => r.DeleteBook(It.IsAny<Book>())).Returns(true);

            var book = fixture.Build<Book>().Create();

            _bookRepositoryMock.Setup(r => r.GetBookNoTracking(book.Id)).ReturnsAsync(book);
            //Act
            var result = await handler.Handle(new Commands.DeleteBookCommand(66),
                                                CancellationToken.None);

            //Assert
            Assert.AreEqual("Book does not exist", result);
        }
    }
}