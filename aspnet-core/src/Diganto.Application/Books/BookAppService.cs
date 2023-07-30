
using Diganto.Authors;
using Diganto.BookWithAuthors;
using Diganto.Permissions;
using Diganto.Publishers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Diganto.Books
{
    public class BookAppService :
        CrudAppService<
            Book, //The Book entity
            BookDto, //Used to show books
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateBookDto>, //Used to create/update a book
        IBookAppService //implement the IBookAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IRepository<Publisher, Guid> _publisherRepository;
        private readonly IRepository<BookWithAuthor, Guid> _bookWithAuthorRepository;
        public BookAppService(IRepository<Book, Guid> repository,
            IAuthorRepository authorRepository,
            IRepository<Publisher, Guid> publisherRepository,
            IRepository<BookWithAuthor, Guid> bookWithAuthorRepo)
            : base(repository)
        {
            _bookRepository = repository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _bookWithAuthorRepository = bookWithAuthorRepo;
            GetPolicyName = DigantoPermissions.Books.Default;
            GetListPolicyName = DigantoPermissions.Books.Default;
            //CreatePolicyName = DigantoPermissions.Books.Create;
            UpdatePolicyName = DigantoPermissions.Books.Edit;
            DeletePolicyName = DigantoPermissions.Books.Delete;
        }
        public override async Task<BookDto> GetAsync(Guid id)
        {
            //Get the IQueryable<Book> from the repository

            //Prepare a query to join books and authors
            var query = from book in _bookRepository
                        join publisher in _publisherRepository on book.PublisherId equals publisher.Id
                        where book.Id == id
                        select new { book, publisher };

            //Execute the query and get the book with author
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Book), id);
            }

            var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
            bookDto.PublisherName = queryResult.publisher.Name;
            return bookDto;
        }

        public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            //Get the IQueryable<Book> from the repository
            var queryable = await Repository.GetQueryableAsync();

            //Prepare a query to join books and authors
            var query = from book in _bookRepository
                        join publisher in _publisherRepository on book.PublisherId equals publisher.Id
                        select new { book, publisher };

            //Paging
            

            //Execute the query and get a list
            var queryResult = await AsyncExecuter.ToListAsync(query);

            //Convert the query result to a list of BookDto objects
            var bookDtos = queryResult.Select(x => 
            {
                var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
                bookDto.PublisherName = x.publisher.Name;
                return bookDto;
            }).ToList();

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<BookDto>(
                totalCount,
                bookDtos
            );
        }
        public async Task<BookDto> CreateBookCustomAsync(CreateUpdateBookDto input)
        {
            var book = new Book();
            var bookWithAuthor = new BookWithAuthor();
            var bookWithAuthorList = new List<BookWithAuthor>();

            
            book.PublisherId = input.PublisherId;
            book.Name = input.Name;
            book.Type = input.Type;
            book.PublishDate = input.PublishDate;
            book.Price = input.Price;
            var returnObject = await Repository.InsertAsync(book, true);
            
            input.Authors.ForEach(x =>
            {
                
                bookWithAuthorList.Add(new BookWithAuthor()
                {
                    BookId = returnObject.Id,
                    AuthorId = x
                });
                
            });
            await _bookWithAuthorRepository.InsertManyAsync(bookWithAuthorList);



            return ObjectMapper.Map<Book, BookDto>(returnObject);
        }
        public async Task<BookDto> UpdateCustomAsync(Guid id, CreateUpdateBookDto input)
        {
            var book = await Repository.GetAsync(id);
            var bookWithAuthor = new BookWithAuthor();
            var bookWithAuthorList = new List<BookWithAuthor>();

            book.PublisherId = input.PublisherId;
            book.Name = input.Name;
            book.Type = input.Type;
            book.PublishDate = input.PublishDate;
            book.Price = input.Price;

            input.Authors.ForEach(x =>
            {

                bookWithAuthorList.Add(new BookWithAuthor()
                {
                    BookId = book.Id,
                    AuthorId = x
                });

            });
            await _bookWithAuthorRepository.DeleteAsync(x=>x.BookId==id);
            await _bookWithAuthorRepository.InsertManyAsync(bookWithAuthorList);
            var returnObject = await Repository.UpdateAsync(book);
            return ObjectMapper.Map<Book, BookDto>(returnObject);
        }

        private object NormalizeSorting(string sorting)
        {
            throw new NotImplementedException();
        }

    }
      
}
