using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Application.DTOs
{
    public class PendingUserDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredOn { get; set; }
    }
}
