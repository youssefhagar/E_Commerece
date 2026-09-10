using E_Commerece.Application.Dtos;
using E_Commerece.Application.Dtos.AuthDtos;
using E_Commerece.Domain.Entites.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Contract
{
    public interface IUserStore
    {

        Task<UserDTo> FindUserbyEmailAsync(string email);

        Task<UserDTo> CreateUserAsync(ApplicationUser user, string password);
        Task<List<String>> GetRoles(string email);

        Task<bool> CheckUserPasswordAsync(string password, string email);
        Task<bool> CheckEmailExist(string  email);


        Task<AddressDto> GetAddressAsync(string email);
        Task<AddressDto> UpsertAddress(string email, AddressDto address);

    }
}
