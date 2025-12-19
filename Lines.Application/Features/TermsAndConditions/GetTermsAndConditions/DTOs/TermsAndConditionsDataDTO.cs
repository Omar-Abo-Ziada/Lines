using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs
{
    public class TermsAndConditionsDataDTO
    {
        public Guid Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
