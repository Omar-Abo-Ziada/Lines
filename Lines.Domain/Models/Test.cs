using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Domain.Models.Common;

namespace Lines.Domain.Models
{
    public class Example : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
