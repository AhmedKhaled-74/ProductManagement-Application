using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs
{
    public class UserDTOForMicro
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string RoleName { get; set; } = null!;
    }
}
