using BookStore.Models;

namespace BookStore.Data
{
    public class Seed
    {
        private readonly DataContext _dataContext;
        public Seed(DataContext context)
        {
            _dataContext = context;
        }/*
        public void SeedDataContext()
        {
            if (!_dataContext.Books.Any())
            {
                var books = new List<Book>()
                {
                    new Book()
                    {
                        Name="A Book Of Days",
                        Author=new Author()
                        {
                            Name="Patti",
                            Surname="Smith"
                        },
                        PublishYear="2022"
                    },
                    new Book()
                    {
                        Name="Veba Geceleri",
                        Author=new Author()
                        {
                            Name="Orhan",
                            Surname="Pamuk"
                        },
                        PublishYear="2021"
                    },
                    new Book()
                    {
                        Name="Harry Potter",
                        Author=new Author()
                        {
                            Name="J.K.",
                            Surname="Rowling"
                        },
                        PublishYear="2021"
                    }
                };
                _dataContext.Books.AddRange(books);
                _dataContext.SaveChanges();
            }
        }*/
    }
}
