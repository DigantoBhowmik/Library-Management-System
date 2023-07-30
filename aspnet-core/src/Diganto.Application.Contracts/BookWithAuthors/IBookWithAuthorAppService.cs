using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Diganto.BookWithAuthors
{
    public interface IBookWithAuthorAppService :
        ICrudAppService< //Defines CRUD methods
            BookWithAuthorDto, //Used to show books
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateBWADto>
    {
    }
}
