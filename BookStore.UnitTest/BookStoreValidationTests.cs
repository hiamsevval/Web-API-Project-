using AutoFixture;
using BookStore.Commands;
using BookStore.DTO;
using BookStore.Validation;
using FluentValidation;
using Shouldly;


namespace BookStore.UnitTest
{
    public class BookStoreValidationTests
    {
     

        [Test]
        public void BookCreate_Validate_Without_Error()
        {
            //Arrange
            var validator = new CreateBookCommandValidator();

            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var book = fixture.Build<CreateBookCommand>().With(a => a.PublishYear, "2001").Create();

            //Act
            var result = validator.Validate(book).IsValid;

            //Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void AuthorCreate_Validate_Without_Error()
        {
            //Arrange
            var validator = new CreateAuthorCommandValidator();

            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var author = fixture.Build<CreateAuthorCommand>().Create();

            //Act
            var result = validator.Validate(author).IsValid;

            //Assert
            Assert.IsTrue(result);
        }
    }
}
