using Diganto.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Diganto.BookWithAuthors
{
    public class BWAAppService :
         CrudAppService<
             BookWithAuthor,
             BookWithAuthorDto,
             Guid,
             PagedAndSortedResultRequestDto,
             CreateUpdateBWADto>,
        IBookWithAuthorAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IRepository<BookWithAuthor, Guid> _bookwaRepository;
        public BWAAppService(IRepository<BookWithAuthor, Guid> repository,
            IAuthorRepository authorRepository)
            : base(repository)
        {
            _bookwaRepository = repository;
            _authorRepository = authorRepository;
        }
        public async Task<List<BookWithAuthorDto>> GetCustomAsyn(Guid id)
        {
            
            var query = from book in _bookwaRepository
                        join author in _authorRepository on book.AuthorId equals author.Id
                        where book.BookId == id
                        select new { book, author};
            var qr = query.Select(x=> new BookWithAuthorDto() { 
                AuthorName=x.author.Name,
                AuthorId=x.author.Id
            }).ToList();
            return qr;

            //Execute the query and get the book with author
            //var queryResult = await AsyncExecuter.ToListAsync(query);

            //var bookWADtos = queryResult.Select(x =>
            //{
            //    var bookWADto = ObjectMapper.Map<BookWithAuthor, BookWithAuthorDto>(x.book);
            //    bookWADto.AuthorName = x.author.Name;
            //    return bookWADto;
            //}).ToList();
            //return new List<bookWADtos>;
          
        }
    }
}
