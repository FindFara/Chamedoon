using System;
using System.Collections.Generic;

namespace ChamedoonWebUI.Models
{
    public class BlogSearchFormViewModel
    {
        public string? Search { get; set; }
        public string? Writer { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public IEnumerable<string>? Writers { get; set; }
        public string? FieldIdSuffix { get; set; }
        public string? SubmitLabel { get; set; }
    }
}
