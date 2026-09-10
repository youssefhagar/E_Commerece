using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Entites.Identity
{
    public class Address
    {

        public int Id { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        
        



        public ApplicationUser User { get; set; } = default!;

        [ForeignKey("User")]
        public string UserId { get; set; }


    }
}
