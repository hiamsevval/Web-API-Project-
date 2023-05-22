using AutoFixture;
using AutoMapper;
using BookStore.Commands;
using BookStore.Controllers;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Queries;
using BookStore.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;


namespace BookStore.UnitTest
{
    public class BookStoreBookControllerTests
    {
        private Mock<IMediator> _mediator;
        private Mock<IBookRepository> _bookRepository;
        private IMapper _mapper;
        private Fixture fixture;

        public BookStoreBookControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _bookRepository = new Mock<IBookRepository>();
            fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<Helpers.MappingProfiles>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task GetBook_Returns_Ok()
        {
            //Arrange
            var book = fixture.Build<Book>().Create();
            var mappedBook = _mapper.Map<BookDto>(book);

            var controller = new BookController(_mediator.Object);

            _bookRepository.Setup(a => a.GetBook(book.Id)).ReturnsAsync(book);
            _mediator.Setup(x => x.Send(It.IsAny<GetBookQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mappedBook);

            //Act
            var result = await controller.GetBook(book.Id);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);
        }

        [Test]
        public async Task GetBook_Returns_NotFound()
        {
            //Arrange
            var book = fixture.Build<Book>().Create();
            var mappedBook = _mapper.Map<BookDto>(book);
            mappedBook = null;

            var controller = new BookController(_mediator.Object);

            _bookRepository.Setup(a => a.GetBook(book.Id)).ReturnsAsync(book);
            _mediator.Setup(x => x.Send(It.IsAny<GetBookQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mappedBook);

            //Act
            var result = await controller.GetBook(book.Id);

            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
        }

        [Test]
        public async Task GetBooks_Returns_Ok()
        {
            //Arrange
            var books = fixture.Build<Book>().CreateMany(4);
            var mappedBooks = _mapper.Map<List<BookDto>>(books);

            string str = "a";
            str = str.Trim().ToUpper();

            var controller = new BookController(_mediator.Object);

            _bookRepository.Setup(r => r.GetBooks(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync((Expression<Func<Book, bool>> predicate) => books.Where(x => x.Name.Trim().ToUpper().Contains(str)));

            var resultBooks = new List<BookDto>();
            for (int i = 0; i < mappedBooks.Count(); i++)
            {
                if (mappedBooks.ElementAt(i).Name.Trim().ToUpper().Contains(str))
                {
                    resultBooks.Add(mappedBooks.ElementAt(i));
                }
            }

            if (resultBooks.Count == 0) { resultBooks = null; }

            _mediator.Setup(x => x.Send(It.IsAny<GetBooksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultBooks);

            //Act
            var result = await controller.GetBooks(str);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);
        }

        [Test]
        public async Task GetBooks_Returns_NotFound()
        {
            //Arrange
            var books = fixture.Build<Book>().CreateMany(4);
            var mappedBooks = _mapper.Map<List<BookDto>>(books);

            string str = "ğ";
            str = str.Trim().ToUpper();

            var controller = new BookController(_mediator.Object);

            _bookRepository.Setup(r => r.GetBooks(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync((Expression<Func<Book, bool>> predicate) => books.Where(x => x.Name.Trim().ToUpper().Contains(str)));

            var resultBooks = new List<BookDto>();
            for (int i = 0; i < mappedBooks.Count(); i++)
            {
                if (mappedBooks.ElementAt(i).Name.Trim().ToUpper().Contains(str))
                {
                    resultBooks.Add(mappedBooks.ElementAt(i));
                }
            }

            if (resultBooks.Count == 0) { resultBooks = null; }

            _mediator.Setup(x => x.Send(It.IsAny<GetBooksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultBooks);

            //Act
            var result = await controller.GetBooks(str);
            var obj = result as ObjectResult;

            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
        }

        [Test]
        public async Task Create_Book_Returns_Ok()
        {
            //Arrange
            var book = fixture.Build<BookDto>().Create();
          
            _bookRepository.Setup(c => c.CreateBook(It.IsAny<Book>())).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            var controller = new BookController(_mediator.Object);
            //Act
            var result = await controller.CreateBook(book);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);

        }

        [Test]
        public async Task Create_Book_Returns_BadRequest()
        {
            //Arrange
            var book = fixture.Build<BookDto>().Create();

            _bookRepository.Setup(c => c.CreateBook(It.IsAny<Book>())).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            var controller = new BookController(_mediator.Object);
            book = null;
            //Act
            var result = await controller.CreateBook(book);

            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.BadRequestResult>(result);

        }

        [Test]
        public async Task Update_Book_Returns_Ok()
        {
            //Arrange
            var book = fixture.Build<BookDto>().Create();
            var bookUpdate = fixture.Build<BookDto>().With(a => a.Id, book.Id).Create();

            _bookRepository.Setup(c => c.UpdateBook(It.IsAny<Book>())).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<UpdateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<BookDto>());
            var controller = new BookController(_mediator.Object);

            //Act
            var result = await controller.UpdateBook(book.Id, bookUpdate);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);
        }

        [Test]
        public async Task Update_Book_Returns_BadRequest_ID_Not_Match()
        {
            //Arrange
            var book = fixture.Build<BookDto>().Create();
            var bookUpdate = fixture.Build<BookDto>().Create();

            _bookRepository.Setup(c => c.UpdateBook(It.IsAny<Book>())).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<UpdateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<BookDto>());
            var controller = new BookController(_mediator.Object);

            //Act
            var result = await controller.UpdateBook(book.Id, bookUpdate);
            
            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_Book_Returns_Ok()
        {
            //Arrange
            var book = fixture.Build<Book>().Create();

            _bookRepository.Setup(x => x.DeleteBook(book)).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<DeleteBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Successfully deleted");

            //Act
            var controller = new BookController(_mediator.Object);
            var result = await controller.DeleteBook(book.Id);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);
        }

        [Test]
        public async Task Delete_Book_Returns_BadRequest_Author_Does_Not_Exist()
        {
            //Arrange
            var book = fixture.Build<Book>().Create();

            _bookRepository.Setup(x => x.DeleteBook(book)).Returns(false);

            _mediator.Setup(x => x.Send(It.IsAny<DeleteBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Book does not exist");

            //Act
            var controller = new BookController(_mediator.Object);
            var result = await controller.DeleteBook(book.Id);

            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.BadRequestResult>(result);
        }
    }
}
