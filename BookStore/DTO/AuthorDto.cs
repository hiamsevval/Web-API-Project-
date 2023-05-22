namespace BookStore.DTO
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public AuthorDto(string name, string surname)
        {
            
            Name = name;
            Surname = surname;
        }
    }
}
