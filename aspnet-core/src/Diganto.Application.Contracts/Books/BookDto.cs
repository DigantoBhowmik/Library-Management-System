using Diganto.Authors;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Diganto.Books
{
    public class BookDto : AuditedEntityDto<Guid>
    {
        public Guid PublisherId { get; set; }
        public string PublisherName { get; set; }
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    }
}
