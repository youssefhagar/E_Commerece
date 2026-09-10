using E_Commerece.Application.Common;
using E_Commerece.Application.Dtos;
using E_Commerece.Application.Dtos.AuthDtos;
using E_Commerece.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Contracts
{
    public interface IAuthService
    {
        public Task<Result<UserDTo>> Login(LoginDto loginDto);
        //Task<Result<UserDTo>> FindUserByEmail(string email);
        //Task<Result<bool>> CheckUserPassword(string Password, string email);
        Task<Result<bool>> EmailExist(string email);

        public Task<Result<UserDTo>> Register(RegisterDto RegisterDto);

        Task<Result<AddressDto>> GetAddress(string email);
        Task<Result<AddressDto>> UpsertAddress(string email,AddressDto address);
    }
}
