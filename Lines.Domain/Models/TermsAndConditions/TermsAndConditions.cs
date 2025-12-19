using Lines.Domain.Enums;
using Lines.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Domain.Models.TermsAndConditions
{
    public class TermsAndConditions : BaseModel
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public TermsType TermsType { get; set; }

    }
}
