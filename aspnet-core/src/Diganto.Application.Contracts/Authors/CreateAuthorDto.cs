using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Diganto.Authors
{
    public class CreateAuthorDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }
    }
}
