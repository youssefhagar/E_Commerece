using E_Commerece.Application.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Contracts
{
    public interface IAcessTokenGenerator
    {

        string GenerateToken(UserDTo userInfo,IEnumerable<string>Roles);

    }
}
