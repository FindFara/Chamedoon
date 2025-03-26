using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Common.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Guid.NewGuid().ToString("N")
                .ToUpper()
                .Where(c => chars.Contains(c))
                .Take(length)
                .ToArray());
        }
    }
}
