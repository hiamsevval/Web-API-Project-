using AutoMapper;
using BookStore.DTO;
using BookStore.Models;
using BookStore.Queries;

namespace BookStore.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<Book, BookDto>();
            CreateMap<AuthorDto, Author>();
            CreateMap<BookDto, Book>();
        }
    }
}
