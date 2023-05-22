using Refit;
using SimpleUI.Models;

namespace SimpleUI.DataAccess

{
    public interface IAuthorData
    {
        [Get("/Author/{authorId}")]
        Task<AuthorModel> GetAuthor(int authorId);

        [Get("/Author/authorSearch")]
        Task <List<AuthorModel>> GetAuthors(string str);

        [Post("/Author")]
        Task<AuthorModel> CreateAuthor([Body] AuthorModel author);

        [Put("/Author/{authorId}")]
        Task<AuthorModel> UpdateAuthor(int authorId, [Body] AuthorModel author);

        [Delete("/Author/{authorId}")]
        Task<string> DeleteAuthor(int authorId);
    }
}
