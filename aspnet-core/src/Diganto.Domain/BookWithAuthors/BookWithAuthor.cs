﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Diganto.BookWithAuthors
{
    public class BookWithAuthor : AuditedAggregateRoot<Guid>
    {
        public Guid AuthorId { get; set; }
        public Guid BookId { get; set; }
    }
}
