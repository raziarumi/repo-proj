using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    [Table("Users")]
    public class User : BaseModel
    {
        public string Name { get; set; }
    }
}
