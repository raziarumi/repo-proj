using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Calculation : BaseModel
    {
        public long UserId { get; set; }
        public string Number1 { get; set; }
        public string Number2 { get; set; }
        public string Summation { get; set; }
    }
}
