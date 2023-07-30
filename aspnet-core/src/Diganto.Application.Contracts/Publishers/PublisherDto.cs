using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Diganto.Publishers
{
    public class PublisherDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
