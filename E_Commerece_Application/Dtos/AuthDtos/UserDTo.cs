using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Dtos.AuthDtos
{
    public class UserDTo
    {
        public string Id { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Eamil { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Token { get; set; } = default!;

    }
}
