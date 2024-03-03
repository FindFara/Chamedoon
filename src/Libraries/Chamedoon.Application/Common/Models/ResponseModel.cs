using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Common.Models
{
    public class ResponseModel
    {
        public bool? Success { get; set; }
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
