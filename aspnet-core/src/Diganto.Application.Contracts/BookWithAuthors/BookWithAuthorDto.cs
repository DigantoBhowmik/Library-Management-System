using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Diganto.BookWithAuthors
{
    public class BookWithAuthorDto : AuditedEntityDto<Guid>
    {
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public Guid PublisherId { get; set; }
    }
}
