using System;
using System.Collections.Generic;
using System.Text;

namespace Diganto.BookWithAuthors
{
    public class CreateUpdateBWADto
    {
        public Guid AuthorId { get; set; }
        public Guid PublisherId { get; set; }
    }
}
