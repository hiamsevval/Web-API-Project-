using AutoFixture;
using AutoMapper;
using BookStore.Commands;
using BookStore.Controllers;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;


namespace BookStore.UnitTest
{
    public class BookStoreAuthorControllerTests
    {
        private Mock<IMediator> _mediator;
        private Mock<IAuthorRepository> _authorRepository;
        private IMapper _mapper;
        private Fixture fixture;

        public BookStoreAuthorControllerTests()
        {
            _mediator= new Mock<IMediator>();
            _authorRepository= new Mock<IAuthorRepository>();
            fixture= new Fixture();

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
        public async Task GetAuthor_Returns_Ok()
        {
            //Arrange
            var author = fixture.Build<Author>().Create();
            var mappedAuthor= _mapper.Map<AuthorDto>(author);

            var controller = new AuthorController(_mediator.Object);

            _authorRepository.Setup(a => a.GetAuthor(author.Id)).ReturnsAsync(author);
            _mediator.Setup(x => x.Send(It.IsAny<GetAuthorQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mappedAuthor);

            //Act
            var result = await controller.GetAuthor(author.Id);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);
        }

        [Test]
        public async Task GetAuthor_Returns_NotFound()
        {
            //Arrange
            var author = fixture.Build<Author>().Create();
            var mappedAuthor = _mapper.Map<AuthorDto>(author);
            mappedAuthor = null;

            var controller = new AuthorController(_mediator.Object);

            _authorRepository.Setup(a => a.GetAuthor(author.Id)).ReturnsAsync(author);
            _mediator.Setup(x => x.Send(It.IsAny<GetAuthorQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mappedAuthor);

            //Act
            var result = await controller.GetAuthor(author.Id);
            
            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
        }

        [Test]
        public async Task GetAuthors_Returns_Ok()
        {
            //Arrange
            var authors = fixture.Build<Author>().CreateMany(4);
            var mappedAuthors = _mapper.Map<List<AuthorDto>>(authors);
           
            string str = "a";
            str = str.Trim().ToUpper();

            var controller = new AuthorController(_mediator.Object);

            _authorRepository.Setup(r => r.GetAuthors(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync((Expression<Func<Author, bool>> predicate) => authors.Where(x => x.Name.Trim().ToUpper().Contains(str)));

            var resultAuthors= new List<AuthorDto>();
            for(int i=0; i<mappedAuthors.Count(); i++)
            {
                if (mappedAuthors.ElementAt(i).Name.Trim().ToUpper().Contains(str))
                {
                    resultAuthors.Add(mappedAuthors.ElementAt(i));
                }
            }

            if(resultAuthors.Count == 0) { resultAuthors = null; }

            _mediator.Setup(x => x.Send(It.IsAny<GetAuthorsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultAuthors);
           
            //Act
            var result = await controller.GetAuthors(str);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);
        }


        [Test]
        public async Task GetAuthors_Returns_NotFound()
        {
            //Arrange
            var authors = fixture.Build<Author>().CreateMany(4);
            var mappedAuthors = _mapper.Map<List<AuthorDto>>(authors);

            string str = "ğ";
            str = str.Trim().ToUpper();

            var controller = new AuthorController(_mediator.Object);

            _authorRepository.Setup(r => r.GetAuthors(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync((Expression<Func<Author, bool>> predicate) => authors.Where(x => x.Name.Trim().ToUpper().Contains(str)));

            var resultAuthors = new List<AuthorDto>();
            for (int i = 0; i < mappedAuthors.Count(); i++)
            {
                if (mappedAuthors.ElementAt(i).Name.Trim().ToUpper().Contains(str))
                {
                    resultAuthors.Add(mappedAuthors.ElementAt(i));
                }
            }

            if (resultAuthors.Count == 0) { resultAuthors = null; }

            _mediator.Setup(x => x.Send(It.IsAny<GetAuthorsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultAuthors);

            //Act
            var result = await controller.GetAuthors(str);
            var obj = result as ObjectResult;

            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
        }


        [Test]
        public async Task Create_Author_Returns_Ok()
        {
            //Arrange
            var author = fixture.Build<AuthorDto>().Create();
            var mappedAuthor = _mapper.Map<Author>(author);
            _authorRepository.Setup(c => c.CreateAuthor(It.IsAny<Author>())).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<CreateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mappedAuthor);

            var controller = new AuthorController(_mediator.Object);
            //Act
            var result = await controller.CreateAuthor(author);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);

        }

        [Test]
        public async Task Create_Author_Returns_BadRequest()
        {   
            //Arrange
            var author = fixture.Build<AuthorDto>().Create();

            _authorRepository.Setup(c => c.CreateAuthor(It.IsAny<Author>())).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<CreateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<Author>());

            //Act
            var controller = new AuthorController(_mediator.Object);
            author = null;
            var result = await controller.CreateAuthor(author);
           
            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.BadRequestResult>(result);

        }

        [Test]
        public async Task Update_Author_Returns_Ok()
        {
            //Arrange
            var author = fixture.Build<AuthorDto>().Create();
            var authorUpdate = fixture.Build<AuthorDto>().With(a=>a.Id, author.Id).Create();

            _authorRepository.Setup(c => c.UpdateAuthor(It.IsAny<Author>())).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<UpdateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<AuthorDto>());
            
            //Act
            var controller = new AuthorController(_mediator.Object);
            var result = await controller.UpdateAuthor(author.Id, authorUpdate);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);
        }

        [Test]
        public async Task Update_Author_Returns_BadRequest_ID_Not_Match()
        {
            //Arrange
            var author = fixture.Build<AuthorDto>().Create();
            var authorUpdate = fixture.Build<AuthorDto>().Create();

            _authorRepository.Setup(c => c.UpdateAuthor(It.IsAny<Author>())).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<UpdateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<AuthorDto>());

            //Act
            var controller = new AuthorController(_mediator.Object);
            var result = await controller.UpdateAuthor(author.Id, authorUpdate);
            

            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_Author_Return_Ok()
        {
            //Arrange
            var author = fixture.Build<Author>().Create();

            _authorRepository.Setup(x=>x.DeleteAuthor(author)).Returns(true);

            _mediator.Setup(x => x.Send(It.IsAny<DeleteAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Successfully deleted");

            //Act
            var controller = new AuthorController(_mediator.Object);
            var result = await controller.DeleteAuthor(author.Id);
            var obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, obj.StatusCode);
        }

        [Test]
        public async Task Delete_Author_Return_BadRequest_Author_Does_Not_Exist()
        {
            //Arrange
            var author = fixture.Build<Author>().Create();

            author = null;
            _authorRepository.Setup(x => x.DeleteAuthor(author)).Returns(false);

            _mediator.Setup(x => x.Send(It.IsAny<DeleteAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Author does not exist");

            //Act
            var controller = new AuthorController(_mediator.Object);
            var result = await controller.DeleteAuthor(0);

            //Assert
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.BadRequestResult>(result);
        }
    }
}
