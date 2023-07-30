using Diganto.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Diganto.Publishers
{
    public class PublisherAppService :
         CrudAppService<
             Publisher,
             PublisherDto,
             Guid,
             PagedAndSortedResultRequestDto,
             CreateUpdatePublisherDto>,
        IPublisherAppService
    {
        public PublisherAppService(IRepository<Publisher, Guid> repository)
            : base(repository)
        {

        }
    }
}
