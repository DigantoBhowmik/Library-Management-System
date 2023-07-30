using AutoMapper;
using Diganto.Authors;
using Diganto.Books;
using Diganto.BookWithAuthors;
using Diganto.Publishers;

namespace Diganto
{
    public class DigantoApplicationAutoMapperProfile : Profile
    {
        public DigantoApplicationAutoMapperProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateUpdateBookDto, Book>();
            CreateMap<Author, AuthorDto>();
            CreateMap<Publisher, PublisherDto>();
            CreateMap<CreateUpdatePublisherDto, Publisher>();
            CreateMap<BookWithAuthor, BookWithAuthorDto>();
            CreateMap<CreateUpdateBWADto, BookWithAuthor>();
        }
    }
}
