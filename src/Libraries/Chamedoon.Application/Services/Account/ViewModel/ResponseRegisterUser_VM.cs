using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Account.ViewModel
{
    public class ResponseRegisterUser_VM
    {
        public string? Message { get; set; }
        public int Code { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
