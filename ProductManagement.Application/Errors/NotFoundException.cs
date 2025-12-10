using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Errors
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string? message) : base(message)
        {
        }

        public NotFoundException(string? message, string v) : base(message)
        {
        }
    }
}
