using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Diganto.Publishers
{
    public interface IPublisherAppService:
         ICrudAppService<
             PublisherDto,
             Guid,
             PagedAndSortedResultRequestDto,
             CreateUpdatePublisherDto>
    {
    }
}
