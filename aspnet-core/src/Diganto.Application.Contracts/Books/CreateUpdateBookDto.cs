using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Diganto.Books
{
    public class CreateUpdateBookDto
    {
        public Guid PublisherId { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public BookType Type { get; set; } = BookType.Undefined;

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Required]
        public float Price { get; set; }
        public List<Guid> Authors { get; set; } = new List<Guid>();
    }
}
