using Digital.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Schema.Dto.User
{
    public class AdminUserResponse : BaseResponse
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte Status { get; set; }
        public string Role { get; set; }
        public decimal DigitalWallet { get; set; }
    }
}
