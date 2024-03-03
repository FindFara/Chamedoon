using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Common.Models
{
    public class ThrowException : Exception
    {
        public string MethodName { get; }

        public ThrowException(string message, string methodName) : base(message)
        {
            MethodName = methodName;
        }
    }
}
